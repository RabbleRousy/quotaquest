using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button soundButton;
    public Button creditsButton;
    public Button exitButton;


    void Start()
    {
        // Start Button Position und Größe
        RectTransform startRect = startButton.GetComponent<RectTransform>();
        startRect.anchoredPosition = new Vector2(0, 200);
        startRect.sizeDelta = new Vector2(600, 150);

        // Sound Button Position und Größe
        RectTransform soundRect = soundButton.GetComponent<RectTransform>();
        soundRect.anchoredPosition = new Vector2(0, 0);
        soundRect.sizeDelta = new Vector2(600, 150);

        // Credits Button Position und Größe
        RectTransform creditsRect = creditsButton.GetComponent<RectTransform>();
        creditsRect.anchoredPosition = new Vector2(0, -200);
        creditsRect.sizeDelta = new Vector2(600, 150);

        // Exit Button Position und Größe
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
        // Hier die Szene laden, die das Spiel startet
        SceneManager.LoadScene("MainScene");
    }

    public void ToggleSound()
    {
        // Hier den Sound ein- oder ausschalten
        AudioListener.pause = !AudioListener.pause;
    }
    public void ShowCredits()
    {
        // Hier die Szene laden, die die Credits anzeigt
        SceneManager.LoadScene("CreditsScene");
    }
    public void ExitGame()
    {
        // Spiel beenden
        Debug.Log("Exit button pressed!");
        Application.Quit();
    }

}

