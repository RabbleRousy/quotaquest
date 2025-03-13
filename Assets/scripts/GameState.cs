using UnityEngine;

[CreateAssetMenu(fileName = "NewGameState", menuName = "Game/GameState")]
public class GameState : ScriptableObject
{
    public int currentMoney;
    public int startQuota = 500;
    public int nextQuota;
    public int quotaIncrease = 250;
    public int strikes;

    public int maxStrikes = 3;
    //public TMPro.TMP_Text strikesText;
    // Optional: passive Upgrades
    // public List<Upgrade> passiveUpgrades;
    
    public void AddMoney(int money) => currentMoney += money;
    public void NextQuota() => nextQuota += quotaIncrease;
    public bool GameOver => strikes >= maxStrikes;

    public void NewGame()
    {
        currentMoney = 0;
        nextQuota = startQuota;
        strikes = 0;
    }
}