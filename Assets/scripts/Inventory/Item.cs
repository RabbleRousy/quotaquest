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
        SetScale();
    }
    
    void SetScale()
    {
        Vector2 cellSize = FindFirstObjectByType<InventoryManager>().UICellSize;
        int maxSize = Int32.MinValue;
        foreach (var rotation in rotations)
        {
            int width, height;
            rotation.GetMaxDimensions(out width, out height);
            if (width > maxSize) maxSize = width;
            if (height > maxSize) maxSize = height;
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 padding = 0.1f * cellSize;
        Vector2 currentSize = rectTransform.sizeDelta;
        Vector2 targetSize = new Vector2(maxSize * cellSize.x - padding.x, maxSize * cellSize.y - padding.y);
        Vector2 scale = targetSize / currentSize;
        rectTransform.localScale = new Vector3(scale.x, scale.y, 1);
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
