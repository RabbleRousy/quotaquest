using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseHoverWindow : MonoBehaviour
{
    public static MouseHoverWindow Instance;
    private RectTransform panel;
    private bool isShowing;
    public bool IsShowing => isShowing;
    
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI description;

    private Vector2 defaultSize;

    private void Awake()
    {
        Instance = this;
        panel = transform.GetChild(0).GetComponent<RectTransform>();
        defaultSize = panel.sizeDelta;
    }

    public void Show(bool big = false)
    {
        panel.gameObject.SetActive(true);
        isShowing = true;
        transform.SetAsLastSibling(); // bring to front
        panel.sizeDelta = big ? 1.5f * defaultSize : defaultSize;
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
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
