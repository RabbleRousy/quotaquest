using System;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MessageWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header, description;
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
}
