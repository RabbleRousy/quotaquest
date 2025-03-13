using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Inventory/Layout", fileName = "New Inventory Layout")]
public class InventoryLayout : ScriptableObject
{
    private const int SIZE = 50;
    [SerializeField]
    public string storage = new string('0', SIZE * SIZE);

    public InventoryLayout(InventoryLayout originalLayout)
    {
        storage = originalLayout.storage;
    }

    public void GetMaxDimensions(out int width, out int height, out int xMin, out int yMin)
    {
        xMin = int.MaxValue;
        int xMax = int.MinValue;
        yMin = int.MaxValue;
        int yMax = int.MinValue;

        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                if (GetCell(x, y) == "0") continue;
				
                if (y > yMax) yMax = y;
                if (x > xMax) xMax = x;
                if (x < xMin) xMin = x;
                if (y < yMin) yMin = y;
            }
        }
        width = xMax - xMin + 1;
        height = yMax - yMin + 1;
    }

    public void GetMaxDimensions(out int width, out int height) => GetMaxDimensions(out width, out height, out _, out _);
    
    public string GetCell(int x, int y)
    {
        int n = GetIndex(x, y);
        return storage.Substring( n, 1);
    }
    
    int GetIndex(int x, int y)
    {
        if (x < 0) return -1;
        if (y < 0) return -1;
        if (x >= SIZE) return -1;
        if (y >= SIZE) return -1;
        return x + y * SIZE;
    }
    
    public void ToggleCell(int x, int y)
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
    
    public Vector2 GetRandomCell()
    {
        int minX, minY, width, height;
        GetMaxDimensions(out width, out height, out minX, out minY);
        int x, y;
        do
        {
            x = Random.Range(minX, minX + width);
            y = Random.Range(minY, minY + height);
        } while (GetCell(x, y) == "0");
        return new Vector2(x, y);
    }

    public void DisableRandomCells(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // Get a random active cell and toggle it off
            Vector2 cell = GetRandomCell();
            ToggleCell((int)cell.x, (int)cell.y);
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(InventoryLayout))]
    public class InventoryLayoutEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var grid = (InventoryLayout)target;

            EditorGUILayout.BeginVertical();

            GUILayout.Label( "Inventory Layout");

            for (int y = 0; y < SIZE; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < SIZE; x++)
                {
                    int n = grid.GetIndex( x, y);

                    var cell = grid.storage.Substring( n, 1);

                    // hard-coded some cheesy color map - improve it by all means!
                    GUI.color = Color.gray;
                    if (cell == "1") GUI.color = Color.white;

                    if (GUILayout.Button( "",  GUILayout.Width(20)))
                    {
                        grid.ToggleCell(x, y);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUI.color = Color.white;
            if (GUILayout.Button("Clear"))
            {
#if UNITY_EDITOR
                Undo.RecordObject( this, "Clear Layout");
#endif
                grid.storage = new string('0', SIZE * SIZE);
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
