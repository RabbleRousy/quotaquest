using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Events/EventData")]
public class EventData : ScriptableObject
{
    public string eventName;
    public float chance; // Wahrscheinlichkeit, wie oft das Event vorkommt
    public string optionA;
    public string optionB;
    public string optionC;
    public string effect;
    public float effectChance; // Wahrscheinlichkeit für den Effekt
}

