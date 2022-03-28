using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private AudioSource audioSource;
    private AudioSource audioSourceIgnorandoPause;

    //Variaveis
    private static int volume;

    //Getters
    public static int Volume => volume;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        audioSource = GetComponent<AudioSource>();

        audioSourceIgnorandoPause = gameObject.AddComponent<AudioSource>() as AudioSource;
        audioSourceIgnorandoPause.ignoreListenerPause = true;

        CopiarAudioSource(audioSourceIgnorandoPause, audioSource);

        //Variaveis
        volume = SaveConfiguracoes.configuracoes.volumeEfeitosSonoros;

        StartCoroutine(SetVolumeInicial(volume));
    }

    public void TocarSom(AudioClip som)
    {
        audioSource.PlayOneShot(som);
    }

    public void TocarSomIgnorandoPause(AudioClip audio)
    {
        audioSourceIgnorandoPause.PlayOneShot(audio);
    }

    private void CopiarAudioSource(AudioSource audioSource, AudioSource audioSource2)
    {
        audioSource.outputAudioMixerGroup = audioSource2.outputAudioMixerGroup;
        audioSource.mute = audioSource2.mute;
        audioSource.bypassEffects = audioSource2.bypassEffects;
        audioSource.bypassListenerEffects = audioSource2.bypassListenerEffects;
        audioSource.bypassReverbZones = audioSource2.bypassReverbZones;
        audioSource.playOnAwake = audioSource2.playOnAwake;
        audioSource.loop = audioSource2.loop;

        audioSource.priority = audioSource2.priority;
        audioSource.volume = audioSource2.volume;
        audioSource.pitch = audioSource2.pitch;
        audioSource.panStereo = audioSource2.panStereo;
        audioSource.spatialBlend = audioSource2.spatialBlend;
        audioSource.reverbZoneMix = audioSource2.reverbZoneMix;

        audioSource.dopplerLevel = audioSource2.dopplerLevel;
        audioSource.spread = audioSource2.spread;
        audioSource.rolloffMode = audioSource2.rolloffMode;
        audioSource.minDistance = audioSource2.minDistance;
        audioSource.maxDistance = audioSource2.maxDistance;
    }

    public void SetVolume(int novoVolume)
    {
        volume = novoVolume;
        audioSource.volume = (float)volume / 100;
        audioSourceIgnorandoPause.volume = (float)volume / 100;

        generalManager.Player.SomDosTiros.SetVolume(volume);

        foreach(Enemy inimigo in generalManager.ObjectManager.ListaInimigos)
        {
            inimigo.SomDosTiros.SetVolume(volume);
        }
    }

    private IEnumerator SetVolumeInicial(int novoVolume)
    {
        yield return null;
        SetVolume(novoVolume);
    }
}
