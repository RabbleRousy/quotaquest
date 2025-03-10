using UnityEngine;
using UnityEngine.UI;

public class EventSelector : MonoBehaviour
{
    public TMPro.TMP_Dropdown eventDropdown;
    public Button confirmButton;

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirm);
    }

    void OnConfirm()
    {
        int selectedIndex = eventDropdown.value;
        string selectedEvent = eventDropdown.options[selectedIndex].text;
        Debug.Log("Selected Event: " + selectedEvent);
        // Hier kannst du weitere Aktionen basierend auf der Auswahl hinzufügen
    }
}
  
