using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryLayout layout;
    private int width, height;
    [SerializeField] private string storage;
    
    [SerializeField] private Item testItem;
    [SerializeField] private InventoryCell cellPrefab;

    public Vector2 UICellSize => GetComponent<GridLayoutGroup>().cellSize;

    void SetDimensions()
    {
        layout.GetMaxDimensions(out width, out height);
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
    }

    private void Awake()
    {
        SetDimensions();
        storage = new string('0', width * height);
        PopulateUI();
    }

    void PopulateUI()
    {
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
                cell.GetComponent<Image>().color = layout.GetCell(x, y) == "1" ? Color.gray : Color.black;
                cell.CellPos = new Vector2(x, y);
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
    
    void ToggleCell( int x, int y)
    {
        int n = GetIndex( x, y);
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
        bool isInLayout = layout.GetCell(x, y) == "1";
        bool isOccupied = GetCell(x, y) == "1";
        return isInLayout && !isOccupied;
    }

    public bool TryInsertItem(ItemRotation item, int x, int y)
    {
        char[] newStorage = storage.ToCharArray();
        // Loop over all fields the item can cover
        for (int i = 0; i < ItemRotation.SIZE; i++)
        {
            for (int j = 0; j < ItemRotation.SIZE; j++)
            {
                if (item.GetCell(i, j) == "0")
                    continue;
                
                // Item body covers this cell
                if (!IsCellAvailable(x + i, y + j)) return false;
                newStorage[GetIndex(x + i, y + j)] = '1';
            }
        }
        storage = new string(newStorage);
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
        return true;
    }

    public void RemoveItem(ItemRotation item, int x, int y)
    {
        char[] newStorage = storage.ToCharArray();
        // Loop over all fields the item can cover
        for (int i = 0; i < ItemRotation.SIZE; i++)
        {
            for (int j = 0; j < ItemRotation.SIZE; j++)
            {
                if (item.GetCell(i, j) == "0")
                    continue;
                
                // Item body covers this cell, remove
                if (GetCell(x + i, y + j) == "0")
                    Debug.LogError("Tried removing item, but there is nothing stored at covered position (" + (x+i) + "/" + (y+j) + ")!");
                newStorage[GetIndex(x + i, y + j)] = '0';
            }
        }
        storage = new string(newStorage);
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
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
                    if (grid.layout.GetCell(x, y) == "1")
                    {
                        GUI.color = Color.gray;
                        if (cell == "1") GUI.color = Color.white;
                    }

                    if (GUILayout.Button( "",  GUILayout.Width(20)))
                    {
                        //grid.ToggleCell(x, y);
                        grid.TryInsertItem(grid.testItem.CurrentRotation, x, y);
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
