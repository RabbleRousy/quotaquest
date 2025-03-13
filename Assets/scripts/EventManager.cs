using System;
using System.Collections;
using System.Collections.Generic;
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
    private GameObject panel;

    private void Awake()
    {
        panel = transform.GetChild(0).gameObject;
    }

    void OnEnable()
    {
        List<int> pool = new List<int>();
        for (int j = 0; j < events.Length; j++)
        {
            for (int i = 0; i < events[j].chance; i++)
                pool.Add(j);
        }
        int randomEvent1 = pool[Random.Range(0, pool.Count)];
        int randomEvent2 = randomEvent1;
        while (randomEvent1 == randomEvent2)
        {
            randomEvent2 = pool[Random.Range(0, pool.Count)];
        }
        DisplayOptions(events[randomEvent1], events[randomEvent2]);
    }

    private void DisplayOptions(EventData event1, EventData event2)
    {

        if (optionButton1Text != null && optionButton2Text != null)
        {
            optionButton1Text.text = event1.eventName;
            optionButton2Text.text = event2.eventName;

            optionAButton.GetComponent<Image>().sprite = event1.eventImage;
            optionBButton.GetComponent<Image>().sprite = event2.eventImage;

            optionAButton.onClick.RemoveAllListeners();
            optionAButton.onClick.AddListener(() => StartCoroutine(ExecuteEvent(event1)));

            optionBButton.onClick.RemoveAllListeners();
            optionBButton.onClick.AddListener(() => StartCoroutine(ExecuteEvent(event2)));
        }
        else
        {
            Debug.LogError("Text components are not assigned");
        }
    }

    private IEnumerator ExecuteEvent(EventData e)
    {
        // Hide panel first
        panel.SetActive(false);
        
        inventoryWindow.SetActive(true);
        
        dropArea.SetActive(true);

        bool effectTriggered = false;
        if (e.effect != null)
        {
            if (Random.value < e.effectChance)
            {
                yield return new WaitForSeconds(1.0f);
                e.effect.Activate();
                effectTriggered = true;
            }
        }
        
        yield return new WaitForSeconds(1.0f);
        FindFirstObjectByType<DropAreaManager>().SpawnItems(e);
        
        MessageWindow msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader(e.eventName + " Event!");
        string description = e.eventDescription;

        if (effectTriggered)
            description += e.effect.description;
        msgWindow.SetDescription(description);
        
        // Hide self, enable panel for next time showing
        panel.SetActive(true);
        gameObject.SetActive(false);
    }
}