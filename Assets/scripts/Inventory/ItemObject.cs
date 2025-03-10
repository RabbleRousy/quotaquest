using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemObject : ScriptableObject
{
    public Sprite itemIcon;
    public int value;
    public string itemName;
    public int itemValue;

	private const int MAX_VALUE = 2;

	//[Header( "Actual saved payload. Use GetCell(x,y) to read!")]
	//[Header( "WARNING: changing this will nuke your data!")]
	private string data = "0000000000000000000000000";

	private const int SIZE = 5;

	// for you to get stuff out of the grid to use in your game
	public string GetCell( int x, int y)
	{
		int n = GetIndex( x, y);
		return data.Substring( n, 1);
	}


	int GetIndex( int x, int y)
	{
		if (x < 0) return -1;
		if (y < 0) return -1;
		if (x >= SIZE) return -1;
		if (y >= SIZE) return -1;
		return x + y * SIZE;
	}





    public void Initialize(string name, int value)
    {
        itemName = name;
        itemValue = value;
    }

void ToggleCell( int x, int y)
	{
		int n = GetIndex( x, y);
		if (n >= 0)
		{
			var cell = data.Substring( n, 1);

			int c = 0;
			int.TryParse( cell, out c);
			c = (c == 0) ? 1 : 0;

			cell = c.ToString();

#if UNITY_EDITOR
			Undo.RecordObject( this, "Toggle Cell");
#endif
			// reassemble
			data = data.Substring( 0, n) + cell + data.Substring( n + 1);
#if UNITY_EDITOR
			EditorUtility.SetDirty( this);
#endif
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ItemObject))]
	public class CheesyGridEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var grid = (ItemObject)target;

			EditorGUILayout.BeginVertical();

			GUILayout.Label( "Item Format");

			for (int y = 0; y < SIZE; y++)
			{
				GUILayout.BeginHorizontal();
				for (int x = 0; x < SIZE; x++)
				{
					int n = grid.GetIndex( x, y);

					var cell = grid.data.Substring( n, 1);

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

			if (GUILayout.Button("Clear"))
			{
#if UNITY_EDITOR
				Undo.RecordObject( this, "Clear Format");
#endif
				grid.data = "0000000000000000000000000";
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
