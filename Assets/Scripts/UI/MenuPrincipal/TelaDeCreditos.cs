using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaDeCreditos : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    private MenuPrincipal menuPrincipal;

    //Componentes
    private Animator animacao;

    //Variaveis
    private bool iniciado = false;

    void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        menuPrincipal = FindObjectOfType<MenuPrincipal>();

        //Componentes
        animacao = GetComponent<Animator>();

        iniciado = true;
    }

    public void IniciarAnimacao()
    {
        animacao.Play("Subindo");
    }

    public void FicarInvisivel()
    {
        animacao.Play("Invisivel");
    }

    public void VoltarAoMenuPrincipal()
    {
        FicarInvisivel();
        menuPrincipal.SetMenuAtual(MenuPrincipal.Menu.Inicio);

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
    }
}
