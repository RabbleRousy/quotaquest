using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropItem : MonoBehaviour, IPointerClickHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private bool isDragging;
    private Item item;
    [SerializeField] private RectTransform upperLeft;

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
            if (isDragging)
            {
                TryDrop();
            }
            else
                isDragging = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            item.Rotate();
        }
    }

    void TryDrop()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = upperLeft.position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("cell"))
            {
                Vector2 cellPos = result.gameObject.GetComponent<InventoryCell>().CellPos;
                bool inserted = FindFirstObjectByType<InventoryManager>().TryInsertItem(item.CurrentRotation, (int)cellPos.x, (int)cellPos.y);
            }
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.worldCamera, out Vector2 localPoint);
            rectTransform.anchoredPosition = localPoint;
        }
    }
}

