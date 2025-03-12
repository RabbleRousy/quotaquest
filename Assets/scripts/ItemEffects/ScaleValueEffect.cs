using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ScaleValueEffect", menuName = "ItemEffects/ScaleValueEffect")]
public class ScaleValueEffect : IItemEffect
{
    public int numTiles = 5;
    public float scale = 1f;
    
    public GameObject cellHighlight;
    
    public override void Activate(Item attachedItem)
    {
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();
        Transform canvas = FindFirstObjectByType<Canvas>().transform;
        
        for (int i = 0; i < numTiles; i++)
        {
            // Random cell (not self)
            Item item;
            Vector2 cell;
            do
            {
                cell = inventory.GetRandomCell();
                item = inventory.GetItemAtCell((int)cell.x, (int)cell.y);
            } while (item == attachedItem);
            
            // Highlight cell
            Vector3 cellPos = inventory.GetWorldPos(new Vector2(cell.x, cell.y));
            inventory.StartCoroutine(ShowHighlightCell(cellPos, canvas));
            
            if (item == null) continue; // No item there

            int oldValue = item.value;
            item.value = (int) (item.value * scale);
            // TODO: Display / message that value was changed
        }
    }

    IEnumerator ShowHighlightCell(Vector3 position, Transform canvas)
    {
        GameObject obj = Instantiate(cellHighlight, position, Quaternion.identity);
        obj.transform.SetParent(canvas);
        
        // TODO: Animate, DoTween, whatever...
        yield return new WaitForSeconds(1f);
        
        Destroy(obj);
    }
}
