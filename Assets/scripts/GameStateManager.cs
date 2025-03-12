using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameState gameState;

    void Awake()
    {
        // Initialisierung
        gameState.currentMoney = 0; // Beispielwerte
        gameState.nextQuote = 500; 
        gameState.strikes = 0; 
    }

    public void UpdateMoney(int amount)
    {
        gameState.currentMoney += amount;
    }

    public void UpdateQuote(int newQuote)
    {
        gameState.nextQuote = newQuote;
    }

    public void AddStrike()
    {
        gameState.strikes++;
    }

    public void NextQuota()
    {
        gameState.nextQuote += 250;
    }
}