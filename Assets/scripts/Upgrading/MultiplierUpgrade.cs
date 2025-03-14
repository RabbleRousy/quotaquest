using UnityEngine;

[CreateAssetMenu(fileName = "MultiplierUpgrade", menuName = "Upgrades/MultiplierUpgrade")]
public class MultiplierUpgrade : IUpgradeData
{
    [Range(1f, 5f)] public float priceIncrease = 2f;
    public override bool CanActivate() => true;

    public override void Activate()
    {
        currentLevel++;
        price = (int) (price * priceIncrease);
    }
    
    public override string GetLastActivationDescription()
    {
        return "Your value multiplier has increased!";
    }
}
