using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDosItensChave : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    //[SerializeField] private SelecaoRoupa[] roupas;

    [SerializeField] private TMP_Text nomeDoItem;
    [SerializeField] private Image imagemDoItem;
    [SerializeField] private TMP_Text descricaoDoItem;

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

        /*
        foreach (SelecaoRoupa selecaoRoupa in roupas)
        {
            selecaoRoupa.Iniciar();
        }
        */

        iniciado = true;
    }

    private void AtualizarInformacoesDoItem()
    {
        nomeDoItem.text = generalManager.Player.InventarioMissao.Itens[selecao].Nome;
        imagemDoItem.sprite = generalManager.Player.InventarioMissao.Itens[selecao].ImagemInventario;
        descricaoDoItem.text = generalManager.Player.InventarioMissao.Itens[selecao].Descricao;
    }

    private void AtualizarScroolDosItens()
    {
        /*
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
        */
    }

    public void IniciarScrool()
    {
        selecao = 0;
        scrool = 0;

        AtualizarScroolDosItens();
        AtualizarInformacoesDoItem();

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    public void MenuItensChave()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao > 0)
            {
                selecao--;

                if (selecao < scrool)
                {
                    scrool = selecao;
                }

                AtualizarScroolDosItens();
                AtualizarInformacoesDoItem();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao < generalManager.Player.InventarioMissao.Itens.Count - 1)
            {
                selecao++;

                /*
                if (selecao - scrool > roupas.Length - 1)
                {
                    scrool = selecao - (roupas.Length - 1);
                }
                */

                AtualizarScroolDosItens();
                AtualizarInformacoesDoItem();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }
}
