using System;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MessageWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header, description;
    public Button closeButton;
    private TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonText = closeButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetHeader(string text) => header.text = text;
    public void SetDescription(string text) => description.text = text;
    public void SetButtonText(string text) => buttonText.text = text;
}
