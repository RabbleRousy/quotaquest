using UnityEngine;

[CreateAssetMenu(fileName = "EnchantUpgrade", menuName = "Upgrades/EnchantUpgrade")]
public class EnchantUpgrade : IUpgradeData
{
    public IItemEffect[] possibleEffects;
    public override bool CanActivate() => true;
    private string lastItem, lastEffect;
    
    public override void Activate()
    {
        IItemEffect randomEffect = possibleEffects[Random.Range(0, possibleEffects.Length)];
        Item item = FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include).GetRandomItem();
        if (item is not null)
        {
            item.effect = randomEffect;
            lastItem = item.itemName;
            lastEffect = randomEffect.effectName;
        }
        else
        {
            lastItem = lastEffect = "";
        }
            
    }
    
    public override string GetLastActivationDescription()
    {
        if (lastItem == "")
            return "No item was enchanted because your inventory is empty.";
        return lastItem + " was enchanted with " + lastEffect;
    }
}
