using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //Componentes
    [SerializeField] private AudioClip musica;

    //Variaveis
    private AudioSource audioSource;

    //Getters
    public bool MusicaTocando => audioSource.isPlaying;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Faz a musica ignorar as pausas ao AudionListener do Unity, para continuar tocando enquanto o jogo esta pausado.
        audioSource.ignoreListenerPause = true;

        if (audioSource != null)
        {
            SetMusic(musica);
        }

        PlayMusic();
    }

    public void SetMusic(AudioClip musica)
    {
        this.audioSource.clip = musica;
    }

    public void PlayMusic()
    {
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
