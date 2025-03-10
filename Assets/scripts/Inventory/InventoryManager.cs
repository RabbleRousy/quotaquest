using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryLayout layout;
    private int width, height;
    private string storage;

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
    }

    int GetIndex( int x, int y)
    {
        if (x < 0) return -1;
        if (y < 0) return -1;
        if (x >= width) return -1;
        if (y >= height) return -1;
        return x + y * width;
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

    public void AddItem()
    {
        
    }

    public void RemoveItem()
    {
        
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
                    int n = grid.GetIndex( x, y);

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
                        grid.ToggleCell(x, y);
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
