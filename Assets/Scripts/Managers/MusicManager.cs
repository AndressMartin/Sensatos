using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //Componentes
    [SerializeField] private AudioClip musica;
    private AudioSource audioSource;

    //Variaveis
    private static int volume;

    //Getters
    public bool MusicaTocando => audioSource.isPlaying;
    public static int Volume => volume;

    private void Awake()
    {
        //Componentes
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //Variaveis
        volume = SaveConfiguracoes.configuracoes.volumeMusica;

        //Faz a musica ignorar as pausas ao AudionListener do Unity, para continuar tocando enquanto o jogo esta pausado.
        audioSource.ignoreListenerPause = true;

        if (audioSource != null)
        {
            SetMusica(musica);
        }

        SetVolume(volume);
        TocarMusica();
    }

    public void SetMusica(AudioClip musica)
    {
        this.audioSource.clip = musica;
    }

    public void TocarMusica()
    {
        audioSource.Play();
    }

    public void PararMusica()
    {
        audioSource.Stop();
    }

    public void SetVolume(int novoVolume)
    {
        volume = novoVolume;
        audioSource.volume = (float)volume / 100;
    }
}
