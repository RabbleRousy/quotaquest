using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks; // Array für die Musikstücke
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayNextTrack();
    }

    void Update()
    {
        
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
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