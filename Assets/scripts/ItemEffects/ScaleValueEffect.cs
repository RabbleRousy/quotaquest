using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScaleValueEffect", menuName = "ItemEffects/ScaleValueEffect")]
public class ScaleValueEffect : IItemEffect
{
    [FormerlySerializedAs("numTiles")] public int numItems = 5;
    public float scale = 1f;
    
    public Image cellHighlight;

    private bool increasesValue => scale > 1f;
    
    public override void Activate(Item attachedItem)
    {
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();
        int candidateCount = inventory.GetModifiableCount() - 1; // minus self
        
        for (int i = 0; i < numItems && i < candidateCount; i++)
        {
            Item item;
            do
            {
                item = inventory.GetRandomItem();
            } while (item == attachedItem);

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
