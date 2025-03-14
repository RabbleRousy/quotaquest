using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    public InventoryLayout layout;
    private int width, height;
    public int Width => width; public int Height => height;
    [SerializeField] private string storage;
    
    [SerializeField] private InventoryCell cellPrefab;
    
    public Vector2 UICellSize => GetComponent<GridLayoutGroup>().cellSize;
    
    [SerializeField]
    private Item[] items;
    private int itemCount;
    private int nextID;

    private void AddItem(Item item)
    {
        items[nextID] = item;
        itemCount++;
        UpdateNextID();
    }

    private void RemoveItem(int at)
    {
        if (items[at] == null)
        {
            Debug.LogError("Tried removing item at items[" + at + "] which is empty!");
            return;
        }
        items[at] = null;
        itemCount--;
        nextID = at;
    }

    void UpdateNextID()
    {
        for (int i = 0; i < width * height; i++)
        {
            int candidate = (nextID + i) % (width * height);
            if (items[candidate] == null)
            {
                nextID = candidate;
                return;
            }
        }
    }

    public void SetLayout(InventoryLayout newLayout)
    {
        layout = newLayout;
        layout.GetMaxDimensions(out width, out height);
        storage += new string('0', width * height - storage.Length);
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
        PopulateUI();
    }

    void SetDimensions()
    {
        layout.GetMaxDimensions(out width, out height);
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
        items = new Item[width * height];
        itemCount = 0;
    }

    private void Awake()
    {
        SetDimensions();
        storage = new string('0', width * height);
        PopulateUI();
    }

    private void OnDisable()
    {
        foreach (var item in items)
        {
            item?.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        foreach (var item in items)
        {
            item?.gameObject.SetActive(true);
        }
    }

    void PopulateUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        RectTransform rect = GetComponent<RectTransform>();
        float uiWidth = rect.rect.width - grid.padding.left - grid.padding.right;
        float uiHeight = rect.rect.height - grid.padding.top - grid.padding.bottom;
        
        layout.GetMaxDimensions(out width, out height);
        grid.cellSize = new Vector2(uiWidth / width - grid.spacing.x, uiHeight / height - grid.spacing.y);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                InventoryCell cell = Instantiate(cellPrefab, transform);
                cell.GetComponent<Image>().color = layout.GetCell(x, y) != "0" ? Color.white : new Color(0, 0, 0, 0);
                cell.CellPos = new Vector2(x, y);
            }
        }
    }

    public void UpdateCellsUI()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GetIndex(x, y);
                Image cellSprite = transform.GetChild(index).GetComponent<Image>();
                cellSprite.color = layout.GetCell(x, y) != "0" ? Color.white : new Color(0, 0, 0, 0);
            }
        }
    }

    int GetIndex( int x, int y)
    {
        if (x < 0) return -1;
        if (y < 0) return -1;
        if (x >= width) return -1;
        if (y >= height) return -1;
        return x + y * width;
    }
    
    public string GetCell( int x, int y)
    {
        int n = GetIndex( x, y);
        return storage.Substring( n, 1);
    }

    public Vector2 GetRandomCell()
    {
        int x, y;
        do
        {
            x = Random.Range(0, width);
            y = Random.Range(0, height);
        } while (layout.GetCell(x, y) == "0");
        return new Vector2(x, y);
    }

    public Item GetItemAtCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return null;
        int index = (int)(GetCell(x, y).ToCharArray()[0] - '1');
        if (index < 0) return null;
        return items[index];
    }

    public Vector3 GetWorldPos(Vector2 cell)
    {
        int n = GetIndex((int)cell.x, (int)cell.y);
        return transform.GetChild(n).position;
    }
    
    void ToggleCell( int x, int y)
    {
        int n = GetIndex(x, y);
        if (n >= 0)
        {
            var cell = storage.Substring( n, 1);

            int c = 0;
            int.TryParse( cell, out c);
            c = (c == 0) ? 1 : 0;

            cell = c.ToString();

#if UNITY_EDITOR
            Undo.RecordObject( this, "Toggle Cell");
#endif
            // reassemble
            storage = storage.Substring( 0, n) + cell + storage.Substring( n + 1);
#if UNITY_EDITOR
            EditorUtility.SetDirty( this);
#endif
        }
    }

    bool IsCellAvailable(int x, int y)
    {
        bool isInLayout = layout.GetCell(x, y) != "0";
        bool isOccupied = GetCell(x, y) != "0";
        return isInLayout && !isOccupied;
    }

    public bool TryInsertItem(ItemRotation item, int x, int y)
    {
        char potentialIndex = (char)('1' + nextID);
        
        char[] newStorage = storage.ToCharArray();
        // Loop over all fields the item can cover
        for (int i = 0; i < ItemRotation.SIZE; i++)
        {
            for (int j = 0; j < ItemRotation.SIZE; j++)
            {
                if (item.GetCell(i, j) == "0")
                    continue;

                // Item body covers this cell
                int cellX = item.HasCornerLeft ? i : ItemRotation.SIZE - 1 - i;
                int cellY = item.HasCornerTop ? j : ItemRotation.SIZE - 1 - j;
                int targetX = x + (item.HasCornerLeft ? cellX : -cellX);
                int targetY = y + (item.HasCornerTop ? cellY : -cellY);
                
                if (targetX < 0 || targetX >= width || targetY < 0 || targetY >= height) return false;
                if (!IsCellAvailable(targetX, targetY)) return false;
                newStorage[GetIndex(targetX, targetY)] = potentialIndex;
            }
        }
        storage = new string(newStorage);
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
        AddItem(item.GetComponentInParent<Item>());
        return true;
    }

    public void RemoveItem(ItemRotation item, int x, int y)
    {
        string id = GetCell(x, y);
        char[] newStorage = storage.ToCharArray();
        // Loop over all fields the item can cover
        for (int i = 0; i < ItemRotation.SIZE; i++)
        {
            for (int j = 0; j < ItemRotation.SIZE; j++)
            {
                if (item.GetCell(i, j) == "0")
                    continue;
                
                // Item body covers this cell, remove
                int cellX = item.HasCornerLeft ? i : ItemRotation.SIZE - 1 - i;
                int cellY = item.HasCornerTop ? j : ItemRotation.SIZE - 1 - j;
                int targetX = x + (item.HasCornerLeft ? cellX : -cellX);
                int targetY = y + (item.HasCornerTop ? cellY : -cellY);
                
                if (targetX < 0 || targetX >= width || targetY < 0 || targetY >= height) 
                {
                    Debug.LogError("Tried removing item, but went out of bounds!");
                    return;
                }
                var cellValue = GetCell(targetX, targetY);
                if (cellValue == "0")
                    Debug.LogError("Tried removing item, but there is nothing stored at covered position (" + (x+i) + "/" + (y+j) + ")!");
                else if (cellValue != id)
                    Debug.LogError("Tried removing item, but found conflicting IDs!");
                
                newStorage[GetIndex(targetX, targetY)] = '0';
            }
        }
        storage = new string(newStorage);
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
        
        RemoveItem(id.ToCharArray()[0] - '1');
    }

    public void OnConfirmSorting()
    {
        StartCoroutine(TriggerAllItemEffects());
    }

    private IEnumerator TriggerAllItemEffects()
    {
        foreach (var item in items)
        {
            if (item?.HasEffect ?? false)
            {
                item.ActivateEffects();
                yield return new WaitForSeconds(.5f);
            }
        }
        OpenSellWindow();
    }

    void OpenSellWindow()
    {
        FindFirstObjectByType<SellItem>(FindObjectsInactive.Include).gameObject.SetActive(true);
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(InventoryManager))]
    public class InventoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var grid = (InventoryManager)target;

            EditorGUILayout.BeginVertical();

            GUILayout.Label( "Inventory Storage");

            for (int y = 0; y < grid.height; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < grid.width; x++)
                {
                    int n = grid.GetIndex(x, y);

                    var cell = grid.storage.Substring( n, 1);

                    // hard-coded some cheesy color map - improve it by all means!
                    GUI.color = Color.black;
                    if (grid.layout.GetCell(x, y) != "0")
                    {
                        GUI.color = Color.gray;
                        if (cell != "0") GUI.color = Color.white;
                    }

                    if (GUILayout.Button( "",  GUILayout.Width(20)))
                    {
                        //grid.ToggleCell(x, y);
                        //grid.TryInsertItem(grid.testItem.CurrentRotation, x, y);
                        Item item = grid.GetItemAtCell(x, y);
                        Debug.Log(item == null ? "No item in cell!" : "Cell contains " + item.name);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUI.color = Color.white;
            if (GUILayout.Button("Clear"))
            {
#if UNITY_EDITOR
                Undo.RecordObject( this, "Clear Format");
#endif
                grid.storage = new string('0', grid.width * grid.height);
#if UNITY_EDITOR
                EditorUtility.SetDirty(grid);
#endif
            }

            GUI.color = Color.white;

            EditorGUILayout.EndVertical();

            DrawDefaultInspector();
        }
    }
#endif
}
