using UnityEngine;

[CreateAssetMenu(menuName = "EventEffects/SpawnItems", fileName = "SpawnItemsEffect")]
public class SpawnItemsEffect : IEventEffect
{
    public Item itemToSpawn;
    public int amount;
    public override void Activate()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include);
        int emptyCells = inventory.GetNumEmptyCells();

        for (int i = 0; i < amount && i < emptyCells; i++)
        {
            // Find random unoccupied cell
            Vector2 cell;
            do
            {
                cell = inventory.GetRandomCell();
            } while (inventory.GetCell((int)cell.x, (int)cell.y) != "0");
            Item item = Instantiate(itemToSpawn, canvas.transform);
            inventory.TryInsertItem(item.CurrentRotation, (int)cell.x, (int)cell.y);
            item.transform.position = inventory.GetWorldPos(cell);
        }
    }
}
