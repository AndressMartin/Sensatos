using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsDoLogoBergamota : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    private MenuPrincipal menuPrincipal;

    //Componentes
    [SerializeField] private AudioClip somDoLogo;

    //Enums
    public enum Som { SomDoLogo };

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        menuPrincipal = FindObjectOfType<MenuPrincipal>();
    }

    public void TocarSom(Som som)
    {
        switch (som)
        {
            case Som.SomDoLogo:
                generalManager.SoundManager.TocarSomIgnorandoPause(somDoLogo);
                break;
        }
    }

    public void TocarSomDoLogo()
    {
        TocarSom(Som.SomDoLogo);
    }

    public void MostrarTitulo()
    {
        menuPrincipal.MostrarTitulo();
    }

    public void LiberarMenu()
    {
        menuPrincipal.SetAtivo(true);
    }

    public void TocarMusicaDoMenu()
    {
        menuPrincipal.TocarMusica();
    }
}
