using System;
using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int value;

    [SerializeField] private ItemRotation[] rotations;
    private int currentRotation;
    public ItemRotation CurrentRotation => rotations[currentRotation];

    private void Awake()
    {
        rotations = GetComponentsInChildren<ItemRotation>();
    }

    private void Start()
    {
        currentRotation = 0;
        UpdateRotations();
    }

    public void Rotate()
    {
        currentRotation = (currentRotation + 1) % rotations.Length;
        UpdateRotations();
    }

    void UpdateRotations()
    {
        for (int i = 0; i < rotations.Length; i++)
        {
            rotations[i].gameObject.SetActive(i == currentRotation);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Rotate"))
            {
                Item item = (Item)target;
                item.Rotate();
            }
        }
        base.OnInspectorGUI();
    }
}
#endif
