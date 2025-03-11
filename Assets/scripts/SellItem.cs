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
        int itemQuote = Random.Range(25, 100); // Beispiel f�r unterschiedliche Quotes pro Item
        Debug.Log("Item verkauft: " + item.name + " f�r " + itemQuote + " M�nzen");
        currentQuoteProgress += itemQuote;
        gameStateManager.UpdateMoney(itemQuote);
        UpdateMoneyText(); // Aktualisiere die Anzeige des aktuellen Geldes

        // �berpr�fe, ob die Quote erf�llt ist
        if (currentQuoteProgress >= gameStateManager.gameState.nextQuote)
        {
            currentQuoteProgress = 0; // Setze den Fortschritt zur�ck
            currentQuote += 250; // Erh�he die Quote (Beispiel: Erh�hung um 250)
            UpdateQuote(); // Aktualisiere die Quote nach dem Erf�llen
        }
        else
        {
            quoteText.text = currentQuoteProgress + "/" + gameStateManager.gameState.nextQuote;
        }
    }
}
