using UnityEngine;

[CreateAssetMenu(fileName = "MultiplierUpgrade", menuName = "Upgrades/MultiplierUpgrade")]
public class MultiplierUpgrade : IUpgradeData
{
    public override bool CanActivate() => true;

    public override void Activate()
    {
        currentLevel++;
    }
    
    public override string GetLastActivationDescription()
    {
        return "Your value multiplier has increased!";
    }
}
