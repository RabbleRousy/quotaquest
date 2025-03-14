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
        Item item = FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include).GetRandomUnenchantedItem();
        if (item is not null)
        {
            item.Enchant(randomEffect);
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
            return "There is no item in your inventory that could be enchanted. What a waste of money.";
        return pickupDescription + "\n \n" + lastItem + " was enchanted with \"" + lastEffect + "\".";
    }
}
