using UnityEngine;

[CreateAssetMenu(fileName = "ScaleValueEffect", menuName = "ItemEffects/ScaleValueEffect")]
public class ScaleValueEffect : IItemEffect
{
    public int numTiles = 5;
    public float scale = 1f;
    public override void Activate(Item attachedItem)
    {
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();
        for (int i = 0; i < numTiles; i++)
        {
            // Random cell (not self)
            Vector2 cell = inventory.GetRandomCell();
        }
    }
}
