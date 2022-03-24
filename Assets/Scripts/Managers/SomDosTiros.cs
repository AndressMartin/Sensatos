using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomDosTiros : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TocarSom(AudioClip som)
    {
        SetSom(som);
        audioSource.Play();
    }

    public void PararSom()
    {
        audioSource.Stop();
    }

    private void SetSom(AudioClip som)
    {
        this.audioSource.clip = som;
    }

    public void SetVolume(int volume)
    {
        audioSource.volume = (float)volume / 100;
    }
}
