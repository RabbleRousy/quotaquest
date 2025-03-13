using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScaleValueEffect", menuName = "ItemEffects/ScaleValueEffect")]
public class ScaleValueEffect : IItemEffect
{
    public int numTiles = 5;
    public float scale = 1f;
    
    public Image cellHighlight;

    private bool increasesValue => scale > 1f;
    
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
            //inventory.StartCoroutine(ShowHighlightCell(cellPos, canvas));
            
            if (item == null) continue; // No item there

            int oldValue = item.value;
            item.value = (int) (item.value * scale);
            inventory.StartCoroutine(HighlightItem(item.CurrentRotation.GetComponent<Image>()));
        }
    }

    IEnumerator ShowHighlightCell(Vector3 position, Transform canvas)
    {
        Image sprite = Instantiate(cellHighlight, position, Quaternion.identity).GetComponent<Image>();
        sprite.color = increasesValue ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
        sprite.transform.SetParent(canvas);
        
        // TODO: Animate, DoTween, whatever...
        yield return new WaitForSeconds(1f);
        
        Destroy(sprite.gameObject);
    }

    IEnumerator HighlightItem(Image image)
    {
        // Be slower than highlight cell
        yield return new WaitForSeconds(0.5f);
        image.color = increasesValue ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(1f);
        image.color = Color.white;
    }
}
