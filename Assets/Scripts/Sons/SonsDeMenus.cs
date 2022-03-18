using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsDeMenus : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private AudioClip pausar;
    [SerializeField] private AudioClip despausar;
    [SerializeField] private AudioClip abrirOInventario;
    [SerializeField] private AudioClip fecharOInventario;

    [SerializeField] private AudioClip movimento1;
    [SerializeField] private AudioClip movimento2;
    [SerializeField] private AudioClip confirmar;
    [SerializeField] private AudioClip voltar;
    [SerializeField] private AudioClip falha;
    [SerializeField] private AudioClip equiparArma;
    [SerializeField] private AudioClip equiparRoupa;

    //Enums
    public enum Som { Pausar, Despausar, AbrirOInventario, FecharOInventario, Movimento1, Movimento2, Confirmar, Voltar, Falha, EquiparArma, EquiparRoupa };

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void TocarSom(Som som)
    {
        switch (som)
        {
            case Som.Pausar:
                generalManager.SoundManager.TocarSomIgnorandoPause(pausar);
                break;

            case Som.Despausar:
                generalManager.SoundManager.TocarSomIgnorandoPause(despausar);
                break;

            case Som.AbrirOInventario:
                generalManager.SoundManager.TocarSomIgnorandoPause(abrirOInventario);
                break;

            case Som.FecharOInventario:
                generalManager.SoundManager.TocarSomIgnorandoPause(fecharOInventario);
                break;

            case Som.Movimento1:
                generalManager.SoundManager.TocarSomIgnorandoPause(movimento1);
                break;

            case Som.Movimento2:
                generalManager.SoundManager.TocarSomIgnorandoPause(movimento2);
                break;

            case Som.Confirmar:
                generalManager.SoundManager.TocarSomIgnorandoPause(confirmar);
                break;

            case Som.Voltar:
                generalManager.SoundManager.TocarSomIgnorandoPause(voltar);
                break;

            case Som.Falha:
                generalManager.SoundManager.TocarSomIgnorandoPause(falha);
                break;

            case Som.EquiparArma:
                generalManager.SoundManager.TocarSomIgnorandoPause(equiparArma);
                break;

            case Som.EquiparRoupa:
                generalManager.SoundManager.TocarSomIgnorandoPause(equiparRoupa);
                break;
        }
    }
}
