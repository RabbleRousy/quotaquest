using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour
{
    public string itemName;
    public int value;
    public IItemEffect effect;
    [Range(0f,1f)] public float effectChance;
    private bool hasEffect;
    public bool HasEffect => effect != null && hasEffect;

    [SerializeField] private ItemRotation[] rotations;
    private int currentRotation;
    public ItemRotation CurrentRotation => rotations[currentRotation];

    private void Awake()
    {
        rotations = GetComponentsInChildren<ItemRotation>();
        foreach (var r in rotations)
        {
            r.item = this;
        }
    }

    private void Start()
    {
        RollEffect();
        currentRotation = 0;
        UpdateRotations();
        SetScale();
    }

    // Determine whether we get an effect
    void RollEffect()
    {
        hasEffect = Random.value < effectChance;
        if (!HasEffect) return;
        
        // TODO: Modify visually
    }
    
    public void SetScale()
    {
        Vector2 cellSize = FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include).UICellSize;
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

    public void ActivateEffects()
    {
        if (!HasEffect) return;
        
        effect.Activate(this);
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
}