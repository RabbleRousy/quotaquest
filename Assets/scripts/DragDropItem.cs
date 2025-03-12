using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropItem : MonoBehaviour, IPointerClickHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Item item;
    [SerializeField] private RectTransform upperLeft;
    public bool InInventory;
    public Vector2 inventorySlot = new Vector2(-1, -1);

    private static DragDropItem draggedItem = null;

    public bool IsDragging
    {
        get => draggedItem == this;
        set => draggedItem = value ? this : null;
    }

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        item = GetComponent<Item>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (IsDragging)
            {
                TryDrop();
            }
            else if (draggedItem == null)
                PickUp();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (IsDragging)
                item.Rotate();
        }
    }

    void TryDrop()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = item.CurrentRotation.Corner.position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Cell"))
            {
                Vector2 cellPos = result.gameObject.GetComponent<InventoryCell>().CellPos;
                bool inserted = FindFirstObjectByType<InventoryManager>().TryInsertItem(item.CurrentRotation, (int)cellPos.x, (int)cellPos.y);
                if (inserted)
                {
                    Vector3 cornerOffset = item.CurrentRotation.Corner.position - rectTransform.position;
                    rectTransform.position = result.gameObject.transform.position - cornerOffset;
                    InInventory = true;
                    IsDragging = false;
                    inventorySlot = cellPos;
                }
                else
                {
                    // TODO: Feedback, shake etc.
                    Debug.LogWarning("Failed to drop item at cell (" + cellPos.x + "/" + cellPos.y + ")!");
                }
            }
            else if (result.gameObject.CompareTag("DropArea"))
            {
                IsDragging = false;
            }
            else if (result.gameObject.CompareTag("SellArea"))
            {
                IsDragging = false;
                FindFirstObjectByType<SellItem>().Sell(item);
            }
        }
    }

    void PickUp()
    {
        IsDragging = true;
        transform.SetAsLastSibling();

        if (!InInventory) return;
        
        FindFirstObjectByType<InventoryManager>().RemoveItem(item.CurrentRotation, (int)inventorySlot.x, (int)inventorySlot.y);
        InInventory = false;
        inventorySlot = new Vector2(-1, -1);
    }

    private void Update()
    {
        if (IsDragging)
        {
            Vector2 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.worldCamera, out Vector2 localPoint);
            rectTransform.anchoredPosition = localPoint;
        }
    }
}

