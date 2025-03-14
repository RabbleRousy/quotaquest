using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks; // Array für die Musikstücke
    private AudioSource audioSource;
    private int currentTrackIndex = 0;
    public float initialPause = 10.0f; 


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayNextTrackWithPause());
    }


    void Update()
    {
        
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    IEnumerator PlayNextTrackWithPause()
    {
        yield return new WaitForSeconds(initialPause); 
        PlayNextTrack();
    }

    void PlayNextTrack()
    {
        if (musicTracks.Length == 0)
            return;

        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.Play();

        
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
    }
}