using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicasDoMapa : MonoBehaviour
{
    //Managers
    GeneralManagerScript generalManager;

    //Enuns
    public enum Musica { Principal }

    //Variaveis
    [SerializeField] private AudioClip musicaPrincipal;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        StartCoroutine(IniciarMusicaPrincipal());
    }

    public void TocarMusica(Musica musica)
    {
        switch(musica)
        {
            case Musica.Principal:
                TrocarETocarMusica(musicaPrincipal);
                break;
        }
    }

    private void TrocarETocarMusica(AudioClip musica)
    {
        if(musica == null)
        {
            return;
        }

        generalManager.MusicManager.SetMusica(musica);
        generalManager.MusicManager.TocarMusica();
    }

    private IEnumerator IniciarMusicaPrincipal()
    {
        yield return null;

        TrocarETocarMusica(musicaPrincipal);
    }
}
