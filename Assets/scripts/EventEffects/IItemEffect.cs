using UnityEngine;

public abstract class IEventEffect : ScriptableObject
{
    public string description;
    public abstract void Activate();
}
