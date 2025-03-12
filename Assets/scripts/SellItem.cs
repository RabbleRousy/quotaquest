using UnityEngine;
using UnityEngine.UI;
using TMPro; // Erg�nzung f�r TMPro

public class SellItem : MonoBehaviour
{
    public TMP_Text quoteText; // Erg�nzung f�r TMPro
    public TMP_Text moneyText; // Erg�nzung f�r die Anzeige des aktuellen Geldes
    public Button sellButton;
    public GameObject itemTestPrefab;
    public GameStateManager gameStateManager;

    private int currentQuote = 500; // Startquote
    private int currentQuoteProgress = 0;

    void Start()
    {
        UpdateQuote();
        UpdateMoneyText();
    }

    void UpdateQuote()
    {
        gameStateManager.UpdateQuote(currentQuote);
        quoteText.text = currentQuoteProgress + "/" + gameStateManager.gameState.nextQuote;
    }

    void UpdateMoneyText()
    {
        moneyText.text = "aktuelles Geld: " + gameStateManager.gameState.currentMoney;
    }

    public void Sell(Item item)
    {
        int itemQuote = item.value;
        Debug.Log("Item verkauft: " + item.name + " fuer " + itemQuote + " Muenzen");
        currentQuoteProgress += itemQuote;

        // Ueberpruefe, ob die Quote erfuellt ist
        if (currentQuoteProgress >= gameStateManager.gameState.nextQuote)
        {
            gameStateManager.UpdateMoney(currentQuoteProgress - gameStateManager.gameState.nextQuote); 
            currentQuoteProgress = 0; // Setze den Fortschritt zur�ck
            currentQuote += 250; // Erh�he die Quote (Beispiel: Erh�hung um 250)
            UpdateQuote(); 
            UpdateMoneyText(); 
        }
        else
        {
            quoteText.text = currentQuoteProgress + "/" + gameStateManager.gameState.nextQuote;
        }
        Destroy(item.gameObject);
    }
}