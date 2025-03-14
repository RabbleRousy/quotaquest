using UnityEngine;

public abstract class IItemEffect : ScriptableObject
{
    public string effectName;
    [TextArea] public string description;
    public abstract void Activate(Item attachedItem);
}
