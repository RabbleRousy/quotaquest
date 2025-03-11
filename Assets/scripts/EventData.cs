using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Events/EventData")]
public class EventData : ScriptableObject
{
    public string eventName;
    public float chance;
    public string effect;
    public float effectChance;
}
