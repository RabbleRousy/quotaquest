using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class MessageWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header, description;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameState gameState;
    public Button confirmButton, cancelButton;
    private TextMeshProUGUI confirmButtonText, cancelButtonText;

    private void Awake()
    {
        confirmButtonText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();
        cancelButtonText = cancelButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetHeader(string text) => header.text = text;
    public void SetDescription(string text) => description.text = text;
    public void SetConfirmButtonText(string text) => confirmButtonText.text = text;
    public void SetCancelButtonText(string text) => cancelButtonText.text = text;

    public void ShowConfirmMessage()
    {
        gameObject.SetActive(true);
        SetHeader("Finish Inventory Sorting");
        SetDescription("Itemeffects will trigger and you'll move on to the Sell Stage. All unlooted items will be lost.");
        SetConfirmButtonText("Confirm");
        confirmButton.onClick.AddListener(CloseDropAreaAndTriggerEffectsOnce);
        cancelButton.gameObject.SetActive(true);
        cancelButton.onClick.AddListener(() => confirmButton.onClick.RemoveListener(CloseDropAreaAndTriggerEffectsOnce));
    }

    private void CloseDropAreaAndTriggerEffectsOnce()
    {
        GameObject.Find("DropArea").SetActive(false);
        FindFirstObjectByType<InventoryManager>().OnConfirmSorting();
        // Unregister self so this only triggers once on button press
        confirmButton.onClick.RemoveListener(CloseDropAreaAndTriggerEffectsOnce);
    }

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }

    private void OnDisable()
    {
        SetConfirmButtonText("OK");
        SetCancelButtonText("Cancel");
        cancelButton.gameObject.SetActive(false);
    }

    public void ShowGameOverMessage()
    {
        gameObject.SetActive(true);
        gameOverPanel.SetActive(true);
        GetComponent<Image>().enabled = false;
        confirmButton.onClick.AddListener(OnGameOverButtonOnce);
        confirmButtonText.text = "New Game";
        cancelButton.gameObject.SetActive(true); // Exit-Button aktivieren
        cancelButton.onClick.AddListener(ExitGame); // Exit-Button Listener hinzuf�gen
        cancelButtonText.text = "Exit Game"; // Exit-Button Text setzen
    }

    private void OnGameOverButtonOnce()
    {
        gameState.NewGame();
        SellItem sellHandler = FindFirstObjectByType<SellItem>(FindObjectsInactive.Include);
        sellHandler.Reset();
        sellHandler.UpdateUI();
        FindFirstObjectByType<InventoryManager>(FindObjectsInactive.Include).ResetInventory(gameState.startLayout);
        var eventManager = FindFirstObjectByType<EventManager>(FindObjectsInactive.Include);
        eventManager.gameObject.SetActive(true);
        eventManager.optionAButton.gameObject.SetActive(true);
        eventManager.optionBButton.gameObject.SetActive(true);
        FindFirstObjectByType<UpgradeManager>().ResetUpgrades();
        GetComponent<Image>().enabled = true;
        gameOverPanel.SetActive(false);
        confirmButton.onClick.RemoveListener(OnGameOverButtonOnce);
        cancelButton.gameObject.SetActive(false); // Exit-Button deaktivieren
        cancelButton.onClick.RemoveListener(ExitGame); // Exit-Button Listener entfernen
    }

    private void ExitGame()
    {
        Application.Quit(); // Spiel beenden
    }
}
