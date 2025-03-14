using System;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public AudioClip pickup, drop, flip, sell, button;
    
    private AudioSource source;

    public static SoundEffectsManager SFX;

    private void Awake()
    {
        source = GetComponent<AudioSource>(); 
        SFX = this;
    }
    
    public void PlayPickupSound()  => source.PlayOneShot(pickup);
    public void PlayDropSound()  => source.PlayOneShot(drop);
    public void PlayFlipSound()  => source.PlayOneShot(flip);
    public void PlaySellSound()  => source.PlayOneShot(sell);
    
    public void PlayButtonSound()  => source.PlayOneShot(button);
}
