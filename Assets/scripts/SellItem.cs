using UnityEngine;
using UnityEngine.UI;

public class SellItem : MonoBehaviour
{
    public TMPro.TMP_Text quoteText;
    public Button sellButton;
    public GameObject ItemTest;
    public GameStateManager gameStateManager;

    void Start()
    {
        quoteText.text = "Quote: " + gameStateManager.gameState.nextQuote.ToString();
        sellButton.onClick.AddListener(OnSell);
    }

    void OnSell()
    {
        
        GameObject item = Instantiate(ItemTest, transform);
        Debug.Log("Item verkauft: " + item.name + " für " + gameStateManager.gameState.nextQuote + " Münzen");
        gameStateManager.UpdateMoney(gameStateManager.gameState.nextQuote);
        // Füge weitere Aktionen basierend auf dem Verkauf hinzu
    }
}

