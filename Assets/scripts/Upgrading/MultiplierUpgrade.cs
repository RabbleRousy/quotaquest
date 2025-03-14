using UnityEngine;

[CreateAssetMenu(fileName = "MultiplierUpgrade", menuName = "Upgrades/MultiplierUpgrade")]
public class MultiplierUpgrade : IUpgradeData
{
    public float startMultiplier;
    [Tooltip("Added to Start Multiplier at each purchase")]
    public float multiplierIncrease;
    [SerializeField] private float currentMultiplier;
    public float Multiplier => currentMultiplier;
    [Range(1f, 5f)] public float priceIncrease = 2f;
    public override bool CanActivate() => true;

    public override void Activate()
    {
        currentMultiplier = startMultiplier + currentLevel * multiplierIncrease;
        currentLevel++;
        price = (int) (price * priceIncrease);
    }
    
    public override string GetLastActivationDescription()
    {
        return "Your value multiplier has increased!";
    }
}
