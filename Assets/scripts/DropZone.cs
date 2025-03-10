using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform draggedItem = eventData.pointerDrag.GetComponent<RectTransform>();
            draggedItem.SetParent(transform); 
            draggedItem.anchoredPosition = Vector2.zero; 
        }
    }
}