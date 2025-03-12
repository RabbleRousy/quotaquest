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
            OnQuotaReached();
        }
        else
        {
            quoteText.text = currentQuoteProgress + "/" + gameStateManager.gameState.nextQuote;
        }
        Destroy(item.gameObject);
    }

    private void OnQuotaReached()
    {
        gameStateManager.UpdateMoney(currentQuoteProgress - gameStateManager.gameState.nextQuote); 
        currentQuoteProgress = 0; // Setze den Fortschritt zurueck
        currentQuote += 250; // Erhoehe die Quote (Beispiel: Erhoehung um 250)
        UpdateQuote(); 
        UpdateMoneyText();

        MessageWindow msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader("Quota reached!");
        msgWindow.SetDescription("Congratulations! You reached your quota this time. Your next quota is $" + gameStateManager.gameState.nextQuote);
        msgWindow.SetConfirmButtonText("Next Turn");
        msgWindow.confirmButton.onClick.AddListener(TriggerNextTurnOnce);
    }

    private void TriggerNextTurnOnce()
    {
        gameObject.SetActive(false);
        FindFirstObjectByType<InventoryManager>().transform.parent.gameObject.SetActive(false);
        FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include).confirmButton.onClick.RemoveListener(TriggerNextTurnOnce);
        FindFirstObjectByType<EventManager>(FindObjectsInactive.Include).gameObject.SetActive(true);
    }
}