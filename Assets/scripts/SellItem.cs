using UnityEngine;
using UnityEngine.UI;

public class SellItem : MonoBehaviour
{
    public Button sellButton;
    public Text itemNameText;
    public Text itemPriceText;

    void Start()
    {
        sellButton.onClick.AddListener(OnSell);
    }

    void OnSell()
    {
        string itemName = itemNameText.text;
        int itemPrice = int.Parse(itemPriceText.text);
        Debug.Log("Item verkauft: " + itemName + " für " + itemPrice + " Münzen");
        // Hier kannst du weitere Aktionen basierend auf dem Verkauf hinzufügen, z.B. Münzen dem Spieler gut schreiben
    }
}
