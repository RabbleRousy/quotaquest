using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public EventData[] events;
    public Button optionAButton;
    public Button optionBButton;
    public TMPro.TMP_Text optionButton1Text;
    public TMPro.TMP_Text optionButton2Text;

    void Start()
    {

        float totalChance = 0f;
        foreach (EventData eventData in events)
        {
            totalChance += eventData.chance;
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (EventData eventData in events)
        {
            cumulativeChance += eventData.chance;
            if (randomValue <= cumulativeChance)
            {
                DisplayOptions(eventData);
                return;
            }
        }

    }
    public void SelectRandomEvent()
    {
    }

    private void DisplayOptions(EventData selectedEvent)
    {
        Debug.Log("Selected Event: " + selectedEvent.eventName);
        Debug.Log("Chance: " + selectedEvent.chance);
        Debug.Log("Effect: " + selectedEvent.effect);
        Debug.Log("Effect Chance: " + selectedEvent.effectChance);

        // Wähle zwei zufällige Optionen aus den drei verfügbaren
        string[] options = { selectedEvent.optionA, selectedEvent.optionB, selectedEvent.optionC };

        int firstOptionIndex = Random.Range(0, options.Length);
        int secondOptionIndex;
        do
        {
            secondOptionIndex = Random.Range(0, options.Length);
        } while (secondOptionIndex == firstOptionIndex);

      
        if (optionButton1Text != null && optionButton2Text != null)
        {
            optionButton1Text.text = options[firstOptionIndex];
            optionButton2Text.text = options[secondOptionIndex];

       
            optionAButton.onClick.RemoveAllListeners();
            optionAButton.onClick.AddListener(() => ExecuteOption(options[firstOptionIndex]));

            optionBButton.onClick.RemoveAllListeners();
            optionBButton.onClick.AddListener(() => ExecuteOption(options[secondOptionIndex]));
        }
        else
        {
            Debug.LogError("Text components are not assigned");
        }
    }

    private void ExecuteOption(string option)
    {
        float randomValue = Random.value;
        Debug.Log("Executed Option: " + option);
    }
}