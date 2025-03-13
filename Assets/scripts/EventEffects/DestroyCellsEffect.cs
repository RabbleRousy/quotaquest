using UnityEngine;

[CreateAssetMenu(menuName = "EventEffects/DestroyCells", fileName = "DestroyCellsEffect")]
public class DestroyCellsEffect : IEventEffect
{
    private InventoryLayout originalLayout;
    public int amountToDestroy;
    
    public override void Activate()
    {
        Debug.Log("Activating Destroy Cell Effect!!");
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();
        originalLayout = inventory.layout;
        InventoryLayout newLayout = CreateInstance<InventoryLayout>();
        newLayout.storage = originalLayout.storage;
        
        for (int i = 0; i < amountToDestroy; i++)
        {
            // Find random unoccupied cell
            Vector2 cell;
            do
            {
                cell = inventory.GetRandomCell();
            } while (inventory.GetCell((int)cell.x, (int)cell.y) != "0");
            // Toggle that cell off in the layout
            newLayout.ToggleCell((int)cell.x, (int)cell.y);
        }
        inventory.layout = newLayout;
        inventory.UpdateCellsUI();

        // TODO: Register reset
    }
}
