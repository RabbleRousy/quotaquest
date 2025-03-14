using UnityEngine;

public abstract class IUpgradeData : ScriptableObject
{
    public int currentLevel = 0;
    public int maxLevel = 3;
    public string upgradeName;
    [TextArea] public string upgradeDescription;
    public int price, startPrice;

    public abstract bool CanActivate();

    public abstract void Activate();

    public abstract string GetLastActivationDescription();
}
