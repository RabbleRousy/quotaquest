using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Layout", fileName = "New Inventory Layout")]
public class InventoryLayout : ScriptableObject
{
    private const int SIZE = 50;
    [SerializeField, HideInInspector]
    private string storage = new string('0', SIZE * SIZE);

    public void GetMaxDimensions(out int width, out int height)
    {
        // Find width
        int minY = Int32.MaxValue, maxY = Int32.MinValue;
        // Find first and last y in each column
        for (int x = 0; x < SIZE; x++)
        {
            int thisColMinY = -1, thisColMaxY = -1;
            for (int y = 0; y < SIZE; y++)
            {
                if (GetCell(x, y) == "0") continue;
                
                if (thisColMinY == -1) thisColMinY = y;
                else thisColMaxY = y;
            }
            if (thisColMinY < minY) minY = thisColMinY;
            if (thisColMaxY > maxY) maxY = thisColMaxY;
        }
        width = maxY - minY;
        // Find height
        int minX = Int32.MaxValue, maxX = Int32.MinValue;
        // Find first and last x in each row
        for (int y = 0; y < SIZE; y++)
        {
            int thisRowMinX = -1, thisRowMaxX = -1;
            for (int x = 0; x < SIZE; x++)
            {
                if (GetCell(x, y) == "0") continue;
                
                if (thisRowMinX == -1) thisRowMinX = x;
                else thisRowMaxX = x;
            }
            if (thisRowMinX < minX) minX = thisRowMinX;
            if (thisRowMaxX > maxX) maxX = thisRowMaxX;
        }
        height = maxX - minX;
    }
    
    public string GetCell( int x, int y)
    {
        int n = GetIndex( x, y);
        return storage.Substring( n, 1);
    }
    
    int GetIndex( int x, int y)
    {
        if (x < 0) return -1;
        if (y < 0) return -1;
        if (x >= SIZE) return -1;
        if (y >= SIZE) return -1;
        return x + y * SIZE;
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
    
#if UNITY_EDITOR
    [CustomEditor(typeof(InventoryLayout))]
    public class InventoryFormatEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var grid = (InventoryLayout)target;

            EditorGUILayout.BeginVertical();

            GUILayout.Label( "Inventory Layout");

            for (int x = 0; x < SIZE; x++)
            {
                GUILayout.BeginHorizontal();
                for (int y = 0; y < SIZE; y++)
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
