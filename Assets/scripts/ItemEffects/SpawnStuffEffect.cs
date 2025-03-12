using UnityEngine;

[CreateAssetMenu(fileName = "SpawnStuffEffect", menuName = "ItemEffects/SpawnStuffEffect")]
public class SpawnStuffEffect : IItemEffect
{
    public Item prefab;
    
    public override void Activate(Item attachedItem)
    {
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();

        Transform canvas = FindFirstObjectByType<Canvas>().transform;
        for (int x = 0; x < inventory.Width; x+=2)
        {
            for (int y = 0; y < inventory.Height; y+=2)
            {
                Vector3 cellPos = inventory.GetWorldPos(new Vector2(x, y));
                Item item = Instantiate(prefab, cellPos, Quaternion.identity);
                item.transform.SetParent(canvas);
                inventory.TryInsertItem(item.CurrentRotation, x, y);
            }
        }
    }
}
