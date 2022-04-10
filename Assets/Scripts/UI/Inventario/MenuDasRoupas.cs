using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuDasRoupas : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private SelecaoRoupa[] roupas;

    [SerializeField] private TMP_Text nomeDaRoupa;
    [SerializeField] private TMP_Text descricaoDaRoupa;

    [SerializeField] private SetaDeScrool[] setasDeScrool;

    //Variaveis
    private int selecao;
    private int scrool;

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

        //Variaveis
        selecao = 0;
        scrool = 0;

        foreach (SelecaoRoupa selecaoRoupa in roupas)
        {
            selecaoRoupa.Iniciar();
        }

        foreach (SetaDeScrool setaDeScrool in setasDeScrool)
        {
            setaDeScrool.Ativa(false);
        }

        iniciado = true;
    }

    private void AtualizarInformacoesDaRoupa()
    {
        nomeDaRoupa.text = generalManager.Player.Inventario.RoupasDeCamuflagem[selecao].Nome;
        descricaoDaRoupa.text = generalManager.Player.Inventario.RoupasDeCamuflagem[selecao].Descricao;
    }

    private void AtualizarScroolDasRoupas()
    {
        for (int i = 0; i < roupas.Length; i++)
        {
            if (scrool + i >= generalManager.Player.Inventario.RoupasDeCamuflagem.Count || scrool + i < 0)
            {
                roupas[i].ZerarInformacoes();
            }
            else
            {
                roupas[i].AtualizarInformacoes(generalManager.Player.Inventario.RoupasDeCamuflagem[scrool + i]);
            }
        }

        foreach (SelecaoRoupa roupa in roupas)
        {
            roupa.Selecionado(false);
        }

        roupas[selecao - scrool].Selecionado(true);

        //Setas de Scrool
        if (scrool > 0)
        {
            setasDeScrool[0].Ativa(true);
        }
        else
        {
            setasDeScrool[0].Ativa(false);
        }

        if (scrool + roupas.Length < generalManager.Player.Inventario.RoupasDeCamuflagem.Count)
        {
            setasDeScrool[1].Ativa(true);
        }
        else
        {
            setasDeScrool[1].Ativa(false);
        }
    }

    public void IniciarScrool()
    {
        for (int i = 0; i < generalManager.Player.Inventario.RoupasDeCamuflagem.Count; i++)
        {
            if (generalManager.Player.Inventario.RoupasDeCamuflagem[i] == generalManager.Player.Inventario.RoupaAtual)
            {
                selecao = i;
                scrool = i - 1;
                break;
            }
        }

        AtualizarScroolDasRoupas();
        AtualizarInformacoesDaRoupa();

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    public void MenuRoupa()
    {
        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecao > 0)
            {
                selecao--;

                if (selecao < scrool)
                {
                    scrool = selecao;
                }

                AtualizarScroolDasRoupas();
                AtualizarInformacoesDaRoupa();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecao < generalManager.Player.Inventario.RoupasDeCamuflagem.Count - 1)
            {
                selecao++;

                if (selecao - scrool > roupas.Length - 1)
                {
                    scrool = selecao - (roupas.Length - 1);
                }

                AtualizarScroolDasRoupas();
                AtualizarInformacoesDaRoupa();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            ConfirmarRoupa();

            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.EquiparRoupa);
        }
    }

    public void ConfirmarRoupa()
    {
        generalManager.Player.Inventario.SetRoupaAtual(generalManager.Player.Inventario.RoupasDeCamuflagem[selecao]);
    }
}
