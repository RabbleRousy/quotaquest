using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseHoverWindow : MonoBehaviour
{
    public static MouseHoverWindow Instance;
    private GameObject panel;
    private bool isShowing;
    public bool IsShowing => isShowing;
    
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI description;

    private void Awake()
    {
        Instance = this;
        panel = transform.GetChild(0).gameObject;
    }

    public void Show()
    {
        panel.SetActive(true);
        isShowing = true;
        transform.SetAsLastSibling(); // bring to front
    }

    public void Hide()
    {
        panel.SetActive(false);
        isShowing = false;
    }

    private void Update()
    {
        if (!isShowing) return;
        
        transform.position = Input.mousePosition;
    }
    
    public void SetName(string s) => header.text = s;
    public void SetDescription(string s) => description.text = s;
}
