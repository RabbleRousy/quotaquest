using UnityEngine;
using UnityEngine.UI;

public class EventSelector : MonoBehaviour
{
    public EventManager eventManager;
    public Button eventButton1;
    public Button eventButton2;
    public Button eventButton3;

    void Start()
    {
        eventButton1.onClick.AddListener(() => eventManager.SelectEvent(0));
        eventButton2.onClick.AddListener(() => eventManager.SelectEvent(1));
        eventButton3.onClick.AddListener(() => eventManager.SelectEvent(2));
    }
}