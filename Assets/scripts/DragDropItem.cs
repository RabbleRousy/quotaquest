using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Item item;
    [SerializeField] private RectTransform upperLeft;
    public bool InInventory, InDropArea;
    public Vector2 inventorySlot = new Vector2(-1, -1);
    public bool CanPickUp = true;

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
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsDragging) return;
        
        MouseHoverWindow.Instance.Show();
        MouseHoverWindow.Instance.SetName(item.itemName + (item.HasEffect ? " (" + item.effect.effectName + ")" : ""));
        string description = "Value: " + item.value;
        description += "\nEffect: " + (item.HasEffect ? item.effect.description : "-");
        MouseHoverWindow.Instance.SetDescription(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseHoverWindow.Instance.Hide();
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
            {
                if (item.effect is StickyEffect && item.stickyCounter == 0)
                {
                    if (InInventory)
                        FindFirstObjectByType<InventoryManager>().RemoveItem(item.CurrentRotation, (int)inventorySlot.x, (int)inventorySlot.y);
                    Destroy(gameObject);
                    MouseHoverWindow.Instance.Hide();
                } else
                    PickUp();
            }
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
                    SoundEffectsManager.SFX.PlayDropSound();
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
                InDropArea = true;
                FindFirstObjectByType<DropAreaManager>().AddItem(item);
                SoundEffectsManager.SFX.PlayDropSound();
            }
            else if (result.gameObject.CompareTag("SellArea"))
            {
                IsDragging = false;
                FindFirstObjectByType<SellItem>().Sell(item);
                SoundEffectsManager.SFX.PlaySellSound();
            }
        }
    }

    void PickUp()
    {
        if (!CanPickUp) return;
        
        SoundEffectsManager.SFX.PlayPickupSound();
        IsDragging = true;
        transform.SetAsLastSibling();
        
        MouseHoverWindow.Instance.Hide();

        if (InDropArea)
        {
            FindFirstObjectByType<DropAreaManager>().RemoveItem(item);
            InDropArea = false;
        }

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

