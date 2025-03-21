using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ItemRotation : MonoBehaviour
{
	//[Header( "Actual saved payload. Use GetCell(x,y) to read!")]
	//[Header( "WARNING: changing this will nuke your data!")]
	[SerializeField]
	private string format = "0000000000000000000000000";

	public const int SIZE = 5;
	public Transform Corner;

	public bool HasCornerLeft;
	public bool HasCornerTop;

	public Item item;

	private void Awake()
	{
		Corner = transform.Find("Corner");
		HasCornerLeft = Corner.CompareTag("CornerTopLeft") || Corner.CompareTag("CornerBottomLeft");
		HasCornerTop = Corner.CompareTag("CornerTopLeft") || Corner.CompareTag("CornerTopRight");
	}

	// for you to get stuff out of the grid to use in your game
	public string GetCell( int x, int y)
	{
		int n = GetIndex( x, y);
		return format.Substring( n, 1);
	}

	int GetIndex( int x, int y)
	{
		if (x < 0) return -1;
		if (y < 0) return -1;
		if (x >= SIZE) return -1;
		if (y >= SIZE) return -1;
		return x + y * SIZE;
	}
	
	public void GetMaxDimensions(out int width, out int height)
	{
		int xMin = int.MaxValue, xMax = int.MinValue;
		int yMin = int.MaxValue, yMax = int.MinValue;

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

	void ToggleCell( int x, int y)
	{
		int n = GetIndex( x, y);
		if (n >= 0)
		{
			var cell = format.Substring( n, 1);

			int c = 0;
			int.TryParse( cell, out c);
			c = (c == 0) ? 1 : 0;

			cell = c.ToString();

#if UNITY_EDITOR
			Undo.RecordObject( this, "Toggle Cell");
#endif
			// reassemble
			format = format.Substring( 0, n) + cell + format.Substring( n + 1);
#if UNITY_EDITOR
			EditorUtility.SetDirty( this);
#endif
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ItemRotation))]
	public class CheesyGridEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var grid = (ItemRotation)target;

			EditorGUILayout.BeginVertical();

			GUILayout.Label( "Item Format");

			for (int y = 0; y < SIZE; y++)
			{
				GUILayout.BeginHorizontal();
				for (int x = 0; x < SIZE; x++)
				{
					int n = grid.GetIndex( x, y);

					var cell = grid.format.Substring( n, 1);

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
				Undo.RecordObject( this, "Clear Format");
#endif
				grid.format = "0000000000000000000000000";
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
