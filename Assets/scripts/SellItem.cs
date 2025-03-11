using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ergänzung für TMPro

public class SellItem : MonoBehaviour
{
    public TMP_Text quoteText; // Ergänzung für TMPro
    public TMP_Text moneyText; // Ergänzung für die Anzeige des aktuellen Geldes
    public Button sellButton;
    public GameObject itemTestPrefab;
    public GameStateManager gameStateManager;

    private int currentQuote = 500; // Startquote
    private int currentQuoteProgress = 0;

    void Start()
    {
        UpdateQuote();
        UpdateMoneyText();
        sellButton.onClick.AddListener(OnSell);
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

    void OnSell()
    {
        GameObject item = Instantiate(itemTestPrefab, transform);
        int itemQuote = Random.Range(25, 100); // Beispiel für unterschiedliche Quotes pro Item
        Debug.Log("Item verkauft: " + item.name + " für " + itemQuote + " Münzen");
        currentQuoteProgress += itemQuote;
        gameStateManager.UpdateMoney(itemQuote);
        UpdateMoneyText(); // Aktualisiere die Anzeige des aktuellen Geldes

        // Überprüfe, ob die Quote erfüllt ist
        if (currentQuoteProgress >= gameStateManager.gameState.nextQuote)
        {
            currentQuoteProgress = 0; // Setze den Fortschritt zurück
            currentQuote += 250; // Erhöhe die Quote (Beispiel: Erhöhung um 250)
            UpdateQuote(); // Aktualisiere die Quote nach dem Erfüllen
        }
        else
        {
            quoteText.text = currentQuoteProgress + "/" + gameStateManager.gameState.nextQuote;
        }
    }
}
