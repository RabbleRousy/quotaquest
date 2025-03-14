using UnityEngine;

[CreateAssetMenu(fileName = "InventoryUpgrade", menuName = "Upgrades/InventoryUpgrade")]
public class InventoryUpgrade : IUpgradeData
{
    public int priceIncrease = 1000;
    [SerializeField] private InventoryLayout[] layoutUpgrades;

    public override bool CanActivate() => currentLevel < layoutUpgrades.Length;
    
    public override void Activate()
    {
        FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include).SetLayout(layoutUpgrades[currentLevel++]);
        price += priceIncrease;
    }

    public override string GetLastActivationDescription()
    {
        return pickupDescription;
    }
}
