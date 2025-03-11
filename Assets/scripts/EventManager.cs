using UnityEngine;

public class EventManager : MonoBehaviour
{
    public EventData[] events;

    public void SelectEvent(int index)
    {
        if (index < 0 || index >= events.Length)
        {
            Debug.LogError("Invalid event index");
            return;
        }

        EventData selectedEvent = events[index];
        Debug.Log("Selected Event: " + selectedEvent.eventName);
        Debug.Log("Chance: " + selectedEvent.chance);
        Debug.Log("Effect: " + selectedEvent.effect);
        Debug.Log("Effect Chance: " + selectedEvent.effectChance);

       
    }
}