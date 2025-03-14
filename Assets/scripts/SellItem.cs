using UnityEngine;
using TMPro; 

public class SellItem : MonoBehaviour
{
    public TMP_Text quoteText; // Ergaenzung fuer TMPro
    public TMP_Text moneyText; // Ergaenzung fuer die Anzeige des aktuellen Geldes
    public TMP_Text strikeText;
    public GameState gameState;

    private int currentQuote = 500; // Startquote
    private int currentQuoteProgress;
    public void Reset()
    { 
        currentQuote = gameState.nextQuota;
        currentQuoteProgress = 0;
    }

    private bool quotaReached;

    private void OnEnable()
    {
        currentQuoteProgress = 0;
        currentQuote = gameState.nextQuota;
        quotaReached = false;
        UpdateUI();
    }

    void Awake()
    {
        InitGameState();
        UpdateUI();
        gameObject.SetActive(false);
    }

    void InitGameState()
    {
        gameState.currentMoney = 0; // Beispielwerte
        gameState.nextQuota = gameState.startQuota;
        gameState.strikes = 0;
        Reset();
    }

    public void UpdateUI()
    {
        UpdateQuoteText();
        UpdateMoneyText();
        UpdateStrikesText();
    }

    void UpdateQuoteText()
    {
        quoteText.text = "Quota: " + currentQuoteProgress + "/" + currentQuote + " $";
    }

    void UpdateMoneyText()
    {
        moneyText.text = "Surplus: " + gameState.currentMoney + " $";
    }

    void UpdateStrikesText()
    {
        strikeText.text = "Strikes: " + gameState.strikes + "/" + gameState.maxStrikes;
    }

    public void Sell(Item item)
    {
        int itemQuote = item.value;

        if (currentQuoteProgress < currentQuote)
        {
            currentQuoteProgress += itemQuote;

            // Ueberpruefe, ob die Quote erfuellt ist
            if (currentQuoteProgress >= currentQuote)
            {
                OnQuotaReached();
            }
            else
            {
                quoteText.text = "Quota: " + currentQuoteProgress + "/" + gameState.nextQuota + " $";
            }
        }
        else
        {
            gameState.AddMoney(itemQuote);
        }
        UpdateUI();
        Destroy(item.gameObject);
    }

    private void OnQuotaReached()
    {
        quotaReached = true;
        
        gameState.AddMoney(currentQuoteProgress - gameState.nextQuota); 
        currentQuoteProgress = gameState.nextQuota;
        UpdateUI();
        
        gameState.NextQuota();

        MessageWindow msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader("Quota reached!");
        msgWindow.SetDescription("Congratulations! You reached your quota this time. Your next quota is $" + gameState.nextQuota + 
                                 "\nContinue selling for surplus or keep items for the next turn.");
    }

    public void OnCloseButton()
    {
        if (quotaReached)
        {
            ToUpgradeScreen();
            return;
        }
        MessageWindow msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
        msgWindow.gameObject.SetActive(true);
        msgWindow.SetHeader("Finish turn?");
        msgWindow.SetDescription("You have not reached your quota for this turn. Continuing will yield a STRIKE.");
        msgWindow.SetConfirmButtonText("Confirm");
        msgWindow.confirmButton.onClick.AddListener(ToUpgradeScreen);
        msgWindow.cancelButton.gameObject.SetActive(true);
        msgWindow.cancelButton.onClick.AddListener(() => msgWindow.confirmButton.onClick.RemoveListener(ToUpgradeScreen));
    }

    private void ToUpgradeScreen()
    {
        SoundEffectsManager.SFX.PlayFlipSound();
        
        // If quota not reached, strike
        if (!quotaReached)
            Strike();
        
        currentQuote = gameState.nextQuota;
        currentQuoteProgress = 0;
        UpdateUI();
        gameObject.SetActive(false);

        if (gameState.GameOver) return;
        
        FindFirstObjectByType<InventoryManager>().transform.parent.parent.gameObject.SetActive(false);
        FindFirstObjectByType<UpgradeManager>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include).confirmButton.onClick.RemoveListener(ToUpgradeScreen);
    }

    private void Strike()
    {
        gameState.strikes++;
        UpdateStrikesText();

        if (gameState.GameOver)
        {
            MessageWindow msgWindow = FindFirstObjectByType<MessageWindow>(FindObjectsInactive.Include);
            msgWindow.ShowGameOverMessage();
            FindFirstObjectByType<EventManager>(FindObjectsInactive.Include).optionAButton.gameObject.SetActive(false);
            FindFirstObjectByType<EventManager>(FindObjectsInactive.Include).optionBButton.gameObject.SetActive(false);
        }
    }
}