using System;
using UnityEngine;

public abstract class IUpgradeData : ScriptableObject
{
    public Sprite upgradeCard;
    public int currentLevel = 0;
    public int maxLevel = 3;
    public string upgradeName;
    [TextArea] public string upgradeDescription, pickupDescription;
    public int price, startPrice;

    public abstract bool CanActivate();

    public abstract void Activate();

    public abstract string GetLastActivationDescription();

    public void ResetUpgrade()
    {
        currentLevel = 0;
        price = startPrice;
    }
}
