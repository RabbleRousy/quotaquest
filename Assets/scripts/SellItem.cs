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
        Debug.Log("Item verkauft: " + itemName + " f�r " + itemPrice + " M�nzen");
        // Hier kannst du weitere Aktionen basierend auf dem Verkauf hinzuf�gen, z.B. M�nzen dem Spieler gut schreiben
    }
}
