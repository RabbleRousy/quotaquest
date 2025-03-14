using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip track1;
    public AudioClip track2;
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(LoopTracks());
    }

    IEnumerator LoopTracks()
    {
        while (true)
        {
            // Play first track
            audioSource.clip = track1;
            audioSource.Play();
            yield return new WaitForSeconds(track1.length);

            // Play second track
            audioSource.clip = track2;
            audioSource.Play();
            yield return new WaitForSeconds(track2.length);
        }
    }
}