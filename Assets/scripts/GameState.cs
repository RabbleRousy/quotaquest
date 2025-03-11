using UnityEngine;

[CreateAssetMenu(fileName = "NewGameState", menuName = "Game/GameState")]
public class GameState : ScriptableObject
{
    public int currentMoney;
    public int nextQuote;
    public int strikes;
    // Optional: passive Upgrades
    // public List<Upgrade> passiveUpgrades;
}