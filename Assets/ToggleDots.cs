using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDots : MonoBehaviour
{
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            img.enabled = !img.enabled;
        } 
    }
}
