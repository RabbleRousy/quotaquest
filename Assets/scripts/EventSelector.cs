using UnityEngine;
using UnityEngine.UI;

public class EventSelector : MonoBehaviour
{
    public TMPro.TMP_Dropdown eventDropdown;
    public Button confirmButton;
    public Button eventButton1;
    public Button eventButton2;
    public Button eventButton3;

    void Start()
    {
        eventButton1.onClick.AddListener(() => SelectEvent("Bergpass"));
        eventButton2.onClick.AddListener(() => SelectEvent("Höhle"));
        eventButton3.onClick.AddListener(() => SelectEvent("Waldweg"));
    }

    void SelectEvent(string eventName)
    {
        Debug.Log("Selected Event: " + eventName);
        
    }

void OnConfirm()
    {
        int selectedIndex = eventDropdown.value;
        string selectedEvent = eventDropdown.options[selectedIndex].text;
        Debug.Log("Selected Event: " + selectedEvent);
        // Hier kannst du weitere Aktionen basierend auf der Auswahl hinzufügen
    }
}
  
