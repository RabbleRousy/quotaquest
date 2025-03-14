using UnityEngine;

[CreateAssetMenu(menuName = "EventEffects/GetMoney", fileName = "GetMoneyEffect")]
public class GetMoneyEffect : IEventEffect
{
    public GameState gameState;
    public int amount;
    public override void Activate()
    {
        gameState.AddMoney(amount);
        FindFirstObjectByType<SellItem>(FindObjectsInactive.Include).UpdateUI();
    }
    
    public override string GetDescription() => description + "\n \nYou found $" + amount + "!";
}
