using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StickyEffect", menuName = "ItemEffects/StickyEffect")]
public class StickyEffect : IItemEffect
{
    public int turns;
    
    public override void Activate(Item attachedItem)
    {
        // Starts counter
        if (attachedItem.stickyCounter == -1)
            attachedItem.stickyCounter = turns;
        
        attachedItem.stickyCounter--;
        if (attachedItem.stickyCounter <= 0)
        {
            attachedItem.GetComponent<DragDropItem>().CanPickUp = true;
        }
    }
}
