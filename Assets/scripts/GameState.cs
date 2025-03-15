using UnityEngine;

[CreateAssetMenu(fileName = "NewGameState", menuName = "Game/GameState")]
public class GameState : ScriptableObject
{
    public int currentMoney;
    public int startQuota = 500;
    public int nextQuota;
    public float quotaIncrease = 1.3f;
    public int strikes;

    public int maxStrikes = 3;

    public InventoryLayout startLayout;
    //public TMPro.TMP_Text strikesText;
    // Optional: passive Upgrades
    // public List<Upgrade> passiveUpgrades;
    
    public void AddMoney(int money) => currentMoney += money;
    public void NextQuota() => nextQuota = (int)(nextQuota * quotaIncrease);
    public bool GameOver => strikes >= maxStrikes;

    public void NewGame()
    {
        currentMoney = 0;
        nextQuota = startQuota;
        strikes = 0;
    }
}