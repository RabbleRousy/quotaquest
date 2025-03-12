using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    public EventData[] events;
    public Button optionAButton;
    public Button optionBButton;
    public TMPro.TMP_Text optionButton1Text;
    public TMPro.TMP_Text optionButton2Text;

    [SerializeField] private GameObject inventoryWindow, dropArea;

    void OnEnable()
    {
        float totalChance = 0f;
        foreach (EventData eventData in events)
        {
            totalChance += eventData.chance;
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;
        EventData randomEvent1 = null;

        foreach (EventData eventData in events)
        {
            cumulativeChance += eventData.chance;
            if (randomValue <= cumulativeChance)
            {
                randomEvent1 = eventData;
                break;
            }
        }
        
        randomValue = Random.value * totalChance;
        cumulativeChance = 0f;
        foreach (var eventData in events)
        {
            cumulativeChance += eventData.chance;
            if (randomValue <= cumulativeChance && randomEvent1 != eventData)
            {
                DisplayOptions(randomEvent1, eventData);
                return;
            }
        }
    }

    private void DisplayOptions(EventData event1, EventData event2)
    {

        if (optionButton1Text != null && optionButton2Text != null)
        {
            optionButton1Text.text = event1.eventName;
            optionButton2Text.text = event2.eventName;

            optionAButton.onClick.RemoveAllListeners();
            optionAButton.onClick.AddListener(() => ExecuteEvent(event1));

            optionBButton.onClick.RemoveAllListeners();
            optionBButton.onClick.AddListener(() => ExecuteEvent(event2));
        }
        else
        {
            Debug.LogError("Text components are not assigned");
        }
    }

    private void ExecuteEvent(EventData e)
    {
        MessageWindow msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader(e.eventName + " Event!");
        msgWindow.SetDescription(e.eventName + " event description");
        
        inventoryWindow.SetActive(true);
        
        dropArea.SetActive(true);
        //FindFirstObjectByType<ItemSpawner>().SpawnRandomItems(5); // pass EventData
        
        this.gameObject.SetActive(false);
    }
}