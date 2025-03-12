using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Events/EventData")]
public class EventData : ScriptableObject
{
    public string eventName;
    public int chance; // Wahrscheinlichkeit, wie oft das Event vorkommt
    public string effect;
    public float effectChance; // Wahrscheinlichkeit fuer den Effekt
    public int itemAmount;
    public Item[] possibleItems;
}

