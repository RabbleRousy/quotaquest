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

    private EventData optionA, optionB;

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
        optionA = events[randomEvent1];
        optionB = events[randomEvent2];
        DisplayOptions();
    }

    private void DisplayOptions()
    {
        optionAButton.GetComponent<Image>().sprite = optionA.eventImage;
        optionBButton.GetComponent<Image>().sprite = optionB.eventImage;

        optionAButton.onClick.RemoveAllListeners();
        optionAButton.onClick.AddListener(() => StartCoroutine(ExecuteEvent(optionA)));

        optionBButton.onClick.RemoveAllListeners();
        optionBButton.onClick.AddListener(() => StartCoroutine(ExecuteEvent(optionB)));
    }

    private IEnumerator ExecuteEvent(EventData e)
    {
        // Hide panel first
        panel.SetActive(false);
        MouseHoverWindow.Instance.Hide();
        
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
        string description = e.eventMessage;

        if (effectTriggered)
            description += e.effect.description;
        msgWindow.SetDescription(description);
        
        // Hide self, enable panel for next time showing
        panel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnPointerEnterButtonA()
    {
        MouseHoverWindow.Instance.Show();
        MouseHoverWindow.Instance.SetName(optionA.eventName);
        MouseHoverWindow.Instance.SetDescription(optionA.eventDescription);
    }
    
    public void OnPointerEnterButtonB()
    {
        MouseHoverWindow.Instance.Show();
        MouseHoverWindow.Instance.SetName(optionB.eventName);
        MouseHoverWindow.Instance.SetDescription(optionB.eventDescription);
    }

    public void OnPointerExitButton() => MouseHoverWindow.Instance.Hide();
}