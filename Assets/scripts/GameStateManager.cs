using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public GameState gameState;
    public TMPro.TMP_Text strikesText; // UI-Element für Strikes
    public int maxStrikes = 3; // Maximale Anzahl an Strikes
    public MessageWindow messageWindow; // Referenz zum MessageWindow

    void Awake()
    {
        // Initialisierung
        gameState.currentMoney = 0; // Beispielwerte
        gameState.nextQuote = 500;
        gameState.strikes = 0;
        UpdateStrikesUI(); // Aktualisiere die UI
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
        UpdateStrikesUI(); // Aktualisiere die UI
        CheckGameOver(); // Überprüfe, ob Game Over erreicht ist
    }

    public void NextQuota()
    {
        gameState.nextQuote += 250;
    }

    private void UpdateStrikesUI()
    {
        strikesText.text = "Strikes: " + gameState.strikes + "/" + maxStrikes;
    }

    private void CheckGameOver()
    {
        if (gameState.strikes >= maxStrikes)
        {
            messageWindow.gameObject.SetActive(true);
            messageWindow.SetHeader("Game Over");
            messageWindow.SetDescription("Du hast die maximale Anzahl an Strikes erreicht.");
        }
    }
}