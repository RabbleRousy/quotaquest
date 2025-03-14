using UnityEngine;

public abstract class IEventEffect : ScriptableObject
{
    [TextArea] public string description;
    public abstract void Activate();

    public virtual string GetDescription() => description;
}
