using UnityEngine;

[CreateAssetMenu(fileName = "EnchantUpgrade", menuName = "Upgrades/EnchantUpgrade")]
public class EnchantUpgrade : IUpgradeData
{
    public override bool CanActivate() => true;
    
    public override void Activate()
    {
        currentLevel++;
    }
}
