using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button creditsButton;
    public Button exitButton;

    public GameObject creditsPanel, gameStatePanel;

    void Start()
    {
        // Button-Events zuweisen
        startButton.onClick.AddListener(StartGame);
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

