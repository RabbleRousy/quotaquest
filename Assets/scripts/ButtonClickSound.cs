using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public Button startButton;
    public Button soundButton;
    public Button creditsButton;
    public Button exitButton; 
    public Button Button;
    public AudioClip sceneChangeClip;
    public AudioSource audioSource; // Referenz zu deiner AudioSource

    void Start()
    {
        AudioSettings.Reset(AudioSettings.GetConfiguration());
        SceneManager.sceneLoaded += OnSceneLoaded;
        startButton.onClick.AddListener(PlaySound);
        soundButton.onClick.AddListener(PlaySound);
        creditsButton.onClick.AddListener(PlaySound);
        exitButton.onClick.AddListener(PlaySound);  
    }

    void PlaySound()
    {  
        audioSource.clip = sceneChangeClip;
        audioSource.Play();
        Debug.Log("Button clicked!");
        
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySound();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
