using UnityEngine;
using UnityEngine.UI;

public class EventSelector : MonoBehaviour
{
    public EventManager eventManager;
    public Button selectEventButton;

    void Start()
    {
        if (selectEventButton != null && eventManager != null)
        {
            selectEventButton.onClick.AddListener(() => eventManager.SelectRandomEvent());
        }
        else
        {
            Debug.LogError("SelectEventButton or EventManager is not assigned");
        }
    }
}