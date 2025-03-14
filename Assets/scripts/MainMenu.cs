using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button soundButton;
    public Button creditsButton;
    public Button exitButton;

    public GameObject creditsPanel, gameStatePanel;

    void Start()
    {
        // Start Button Position und Gr��e
        RectTransform startRect = startButton.GetComponent<RectTransform>();
        startRect.anchoredPosition = new Vector2(0, 200);
        startRect.sizeDelta = new Vector2(600, 150);

        // Sound Button Position und Gr��e
        RectTransform soundRect = soundButton.GetComponent<RectTransform>();
        soundRect.anchoredPosition = new Vector2(0, 0);
        soundRect.sizeDelta = new Vector2(600, 150);

        // Credits Button Position und Gr��e
        RectTransform creditsRect = creditsButton.GetComponent<RectTransform>();
        creditsRect.anchoredPosition = new Vector2(0, -200);
        creditsRect.sizeDelta = new Vector2(600, 150);

        // Exit Button Position und Gr��e
        RectTransform exitRect = exitButton.GetComponent<RectTransform>();
        exitRect.anchoredPosition = new Vector2(0, -400);
        exitRect.sizeDelta = new Vector2(600, 150);


        // Button-Events zuweisen
        startButton.onClick.AddListener(StartGame);
        soundButton.onClick.AddListener(ToggleSound);
        creditsButton.onClick.AddListener(ShowCredits);
        exitButton.onClick.AddListener(ExitGame);

    }

    public void StartGame()
    {
        SoundEffectsManager.SFX.PlayButtonSound();
        FindFirstObjectByType<EventManager>(FindObjectsInactive.Include).gameObject.SetActive(true);
        gameStatePanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ToggleSound()
    {
        // Hier den Sound ein- oder ausschalten
        AudioListener.pause = !AudioListener.pause;
    }
    public void ShowCredits()
    {
        SoundEffectsManager.SFX.PlayButtonSound();
        creditsPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ExitGame()
    {
        // Spiel beenden
        Debug.Log("Exit button pressed!");
        Application.Quit();
    }

}

