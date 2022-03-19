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
    [SerializeField] private ListaSlot[] itens;

    [SerializeField] private TMP_Text nomeDoItem;
    [SerializeField] private Image imagemDoItem;
    [SerializeField] private TMP_Text descricaoDoItem;

    [SerializeField] private RectTransform barraDeRolagem;

    //Variaveis
    private int selecao;
    private int scrool;

    private float barraDeRolagemAlturaInicial;

    [SerializeField] private string nomeSemItens;
    [SerializeField] private Sprite imagemSemItens;
    [SerializeField] private string descricaoSemItens;

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

        barraDeRolagemAlturaInicial = barraDeRolagem.sizeDelta.y;

        foreach (ListaSlot listaSlot in itens)
        {
            listaSlot.Iniciar();
            listaSlot.ZerarInformacoes();
        }

        iniciado = true;
    }

    private void AtualizarInformacoesSemItem()
    {
        nomeDoItem.text = nomeSemItens;
        imagemDoItem.sprite = imagemSemItens;
        descricaoDoItem.text = descricaoSemItens;
    }

    private void AtualizarInformacoesDoItem()
    {
        nomeDoItem.text = generalManager.Player.InventarioMissao.Itens[selecao].Nome;
        imagemDoItem.sprite = generalManager.Player.InventarioMissao.Itens[selecao].ImagemInventario;
        descricaoDoItem.text = generalManager.Player.InventarioMissao.Itens[selecao].Descricao;
    }

    private void AtualizarScroolDosItens()
    {
        for (int i = 0; i < itens.Length; i++)
        {
            if (scrool + i >= generalManager.Player.InventarioMissao.Itens.Count || scrool + i < 0)
            {
                itens[i].gameObject.SetActive(false);
            }
            else
            {
                itens[i].AtualizarNome(generalManager.Player.InventarioMissao.Itens[scrool + i].Nome);
                itens[i].gameObject.SetActive(true);
            }
        }

        foreach (ListaSlot listaSlot in itens)
        {
            listaSlot.Selecionado(false);
        }

        itens[selecao - scrool].Selecionado(true);

        //Posicao da Barra de Rolagem
        if (itens.Length < generalManager.Player.InventarioMissao.Itens.Count)
        {
            barraDeRolagem.anchoredPosition = new Vector2(0, (barraDeRolagemAlturaInicial / generalManager.Player.InventarioMissao.Itens.Count) * scrool * -1);
        }
        else
        {
            barraDeRolagem.anchoredPosition = Vector2.zero;
        }
    }

    public void IniciarScrool()
    {
        selecao = 0;
        scrool = 0;

        //Tamanho da Barra de Rolagem
        if (itens.Length < generalManager.Player.InventarioMissao.Itens.Count)
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial * ((float)itens.Length / generalManager.Player.InventarioMissao.Itens.Count));
        }
        else
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial);
        }

        AtualizarScroolDosItens();

        if(generalManager.Player.InventarioMissao.Itens.Count > 0)
        {
            AtualizarInformacoesDoItem();
        }
        else
        {
            AtualizarInformacoesSemItem();
        }

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

                if (selecao - scrool > itens.Length - 1)
                {
                    scrool = selecao - (itens.Length - 1);
                }

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
