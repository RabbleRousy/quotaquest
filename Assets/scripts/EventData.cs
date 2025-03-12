using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Events/EventData")]
public class EventData : ScriptableObject
{
    public string eventName;
    public float chance; // Wahrscheinlichkeit, wie oft das Event vorkommt
    public string effect;
    public float effectChance; // Wahrscheinlichkeit f�r den Effekt
}

