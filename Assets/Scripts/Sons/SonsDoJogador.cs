using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsDoJogador : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private AudioClip morte;

    //Enums
    public enum Som { Morte };

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void TocarSom(Som som)
    {
        switch(som)
        {
            case Som.Morte:
                generalManager.SoundManager.TocarSom(morte);
                break;
        }
    }
}
