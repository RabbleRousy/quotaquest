using UnityEngine;

public abstract class IItemEffect : ScriptableObject
{
    public abstract void Activate(Item attachedItem);
}
