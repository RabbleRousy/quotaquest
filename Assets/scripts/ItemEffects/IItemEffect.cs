using UnityEngine;

public abstract class IItemEffect : ScriptableObject
{
    public string description;
    public abstract void Activate(Item attachedItem);
}
