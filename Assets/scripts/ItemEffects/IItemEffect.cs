using UnityEngine;

public abstract class IItemEffect : ScriptableObject
{
    [Tooltip("This multiplier is applied to the items value at the moment it becomes enchanted")]
    public float valueMultiplier = 1f;
    public string effectName;
    [TextArea] public string description;
    public abstract void Activate(Item attachedItem);
}
