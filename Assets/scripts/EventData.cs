using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Events/EventData")]
public class EventData : ScriptableObject
{
    public string eventName, eventDescription;
    [FormerlySerializedAs("eventDescription")] public string eventMessage;
    public int chance; // Wahrscheinlichkeit, wie oft das Event vorkommt
    public IEventEffect effect;
    [Range(0f,1f)] public float effectChance; // Wahrscheinlichkeit fuer den Effekt
    public int itemAmount;
    public Item[] possibleItems;
    public Sprite eventImage;
 
}

