using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecaoMenu : SelecaoDoInventario
{
    //Componentes
    [SerializeField] private Animator animacao;
    [SerializeField] private Animator animacaoIcone;

    //Enums
    public enum Menu { ItensChave, Missoes, Conquistas }

    //Variaveis
    [SerializeField] private Menu menu;

    private bool iniciado = false;

    private void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        iniciado = true;
    }

    public override void Confirmar(MenuDoInventario menuDoInventario)
    {
        switch(menu)
        {
            case Menu.ItensChave:
                menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.ItensChave);
                menuDoInventario.MenuDosItensChave.IniciarScrool();
                break;

            case Menu.Missoes:
                menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Missoes);
                menuDoInventario.MenuDasMissoes.IniciarScrool();
                break;

            case Menu.Conquistas:
                //menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Conquistas);
                break;
        }
    }

    public override void Selecionado(bool selecionado)
    {
        if (selecionado == true)
        {
            animacao.SetBool("Selecionado", true);
            animacaoIcone.SetTrigger("Selecionado");
        }
        else
        {
            animacao.SetBool("Selecionado", false);
        }
    }
}
