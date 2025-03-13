using UnityEngine;
using TMPro; 

public class SellItem : MonoBehaviour
{
    public TMP_Text quoteText; // Ergaenzung fuer TMPro
    public TMP_Text moneyText; // Ergaenzung fuer die Anzeige des aktuellen Geldes
    public GameStateManager gameStateManager;

    private int currentQuote = 500; // Startquote
    private int currentQuoteProgress;

    private bool quotaReached;

    private void OnEnable()
    {
        currentQuoteProgress = 0;
        quotaReached = false;
    }

    void Start()
    {
        UpdateQuote();
        UpdateMoneyText();
    }

    void UpdateQuote()
    {
        gameStateManager.UpdateQuote(currentQuote);
        quoteText.text = "Quota: " + currentQuoteProgress + "/" + gameStateManager.gameState.nextQuote + " $";
    }

    void UpdateMoneyText()
    {
        moneyText.text = "Surplus: " + gameStateManager.gameState.currentMoney + " $";
    }

    public void Sell(Item item)
    {
        int itemQuote = item.value;

        if (currentQuoteProgress < gameStateManager.gameState.nextQuote)
        {
            currentQuoteProgress += itemQuote;

            // Ueberpruefe, ob die Quote erfuellt ist
            if (currentQuoteProgress >= gameStateManager.gameState.nextQuote)
            {
                OnQuotaReached();
            }
            else
            {
                quoteText.text = "Quota: " + currentQuoteProgress + "/" + gameStateManager.gameState.nextQuote + " $";
            }
        }
        else
        {
            gameStateManager.UpdateMoney(itemQuote);
            UpdateMoneyText();
        }
        
        Destroy(item.gameObject);
    }

    private void OnQuotaReached()
    {
        quotaReached = true;
        
        gameStateManager.UpdateMoney(currentQuoteProgress - gameStateManager.gameState.nextQuote); 
        UpdateMoneyText();
        
        currentQuoteProgress = gameStateManager.gameState.nextQuote;
        UpdateQuote();
        
        gameStateManager.NextQuota();

        MessageWindow msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader("Quota reached!");
        msgWindow.SetDescription("Congratulations! You reached your quota this time. Your next quota is $" + gameStateManager.gameState.nextQuote + 
                                 "\nContinue selling for surplus or keep items for the next turn.");
    }

    public void OnCloseButton()
    {
        if (quotaReached)
        {
            ToEventScreen();
            return;
        }
        MessageWindow msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader("Finish turn?");
        msgWindow.SetDescription("You have not reached your quota for this turn. Continuing will yield a STRIKE.");
        msgWindow.SetConfirmButtonText("Confirm");
        msgWindow.confirmButton.onClick.AddListener(ToEventScreen);
        msgWindow.cancelButton.gameObject.SetActive(true);
        msgWindow.cancelButton.onClick.AddListener(() => msgWindow.confirmButton.onClick.RemoveListener(ToEventScreen));
    }

    private void ToEventScreen()
    {
        // If quota not reached, strike
        if (!quotaReached)
            gameStateManager.gameState.strikes++;
        
        FindFirstObjectByType<InventoryManager>().transform.parent.parent.gameObject.SetActive(false);
        FindFirstObjectByType<EventManager>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include).confirmButton.onClick.RemoveListener(ToEventScreen);
        gameObject.SetActive(false);
    }
}