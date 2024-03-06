using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip run;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip dash;
    public AudioClip attack;
    public AudioClip hit;
    public AudioClip money;
    [Space]

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioRun()
    {
        audioSource.clip = run;
        audioSource.Play();
    }

    public void AudioJump()
    {
        audioSource.clip = jump;
        audioSource.Play();
    }

    public void AudioLand()
    {
        audioSource.clip = land;
        audioSource.Play();
    }

    public void AudioDash()
    {
        audioSource.clip = dash;
        audioSource.Play();
    }

    public void AudioAttack()
    {
        audioSource.clip = attack;
        audioSource.Play();
    }

    public void AudioHit()
    {
        audioSource.clip = hit;
        audioSource.Play();
    }


    public void AudioMoney()
    {
        audioSource.clip = money;
        audioSource.Play();
    }
}
