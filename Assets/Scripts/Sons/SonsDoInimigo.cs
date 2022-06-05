using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsDoInimigo : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private AudioClip[] tomandoDano;

    [SerializeField] private AudioClip[] morte;

    [SerializeField] private AudioClip alerta;

    //Enums
    public enum Som { TomandoDano, Morte, Alerta };

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void TocarSom(Som som)
    {
        switch (som)
        {
            case Som.TomandoDano:
                generalManager.SoundManager.TocarSom(tomandoDano[Random.Range(0, tomandoDano.Length)]);
                break;

            case Som.Morte:
                generalManager.SoundManager.TocarSom(morte[Random.Range(0, morte.Length)]);
                break;

            case Som.Alerta:
                generalManager.SoundManager.TocarSom(alerta);
                break;
        }
    }
}
