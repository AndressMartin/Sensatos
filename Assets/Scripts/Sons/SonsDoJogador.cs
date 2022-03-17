using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsDoJogador : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private AudioClip ataqueFisico;
    [SerializeField] private AudioClip acertoAtaqueFisico;
    [SerializeField] private AudioClip[] dano;
    [SerializeField] private AudioClip morte;

    [SerializeField] private AudioClip trocarDeArma;

    [SerializeField] private AudioClip usarOAlicate;

    //Enums
    public enum Som { AtaqueFisico, AcertoAtaqueFisico, Dano, Morte, TrocarDeArma, UsarOAlicate };

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void TocarSom(Som som)
    {
        switch(som)
        {
            case Som.AtaqueFisico:
                generalManager.SoundManager.TocarSom(ataqueFisico);
                break;

            case Som.AcertoAtaqueFisico:
                generalManager.SoundManager.TocarSom(acertoAtaqueFisico);
                break;

            case Som.Dano:
                generalManager.SoundManager.TocarSom(dano[Random.Range(0, dano.Length)]);
                break;

            case Som.Morte:
                generalManager.SoundManager.TocarSom(morte);
                break;

            case Som.TrocarDeArma:
                generalManager.SoundManager.TocarSom(trocarDeArma);
                break;

            case Som.UsarOAlicate:
                generalManager.SoundManager.TocarSom(usarOAlicate);
                break;
        }
    }
}
