using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDosItens : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private PainelDeEscolhaDosItens painelDeEscolhaDosItens;
    [SerializeField] private PainelDeEscolhaDosItens painelDeEscolhaDosAtalhos;
    [SerializeField] private RectTransform painelDeExplicacaoDosItens;

    [SerializeField] private TMP_Text nomeDoItem;
    [SerializeField] private TMP_Text descricaoDoItem;

    //Enums
    public enum MenuItemEnum { Inicio, AdicionandoNosAtalhos, TrocandoPosicao, ConfirmandoJogarFora }
    public enum MenuAtalhoEnum { Inicio, TrocandoPosicao }

    //Variaveis
    private int selecao;
    private int selecaoPainelDeEscolha;

    private MenuItemEnum menuItemAtual;
    private MenuAtalhoEnum menuAtalhoAtual;

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
        selecaoPainelDeEscolha = 0;

        menuItemAtual = MenuItemEnum.Inicio;
        menuAtalhoAtual = MenuAtalhoEnum.Inicio;

        iniciado = true;
    }

    private void SetMenuItemAtual(MenuItemEnum menuItemAtual)
    {
        this.menuItemAtual = menuItemAtual;

        switch(this.menuItemAtual)
        {
            case MenuItemEnum.Inicio:
                painelDeEscolhaDosItens.gameObject.SetActive(true);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(true);
                break;

            case MenuItemEnum.AdicionandoNosAtalhos:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(false);
                break;

            case MenuItemEnum.TrocandoPosicao:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(false);
                break;

            case MenuItemEnum.ConfirmandoJogarFora:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(false);
                break;
        }
    }

    private void SetMenuAtalhoAtual(MenuAtalhoEnum menuAtalhoAtual)
    {
        this.menuAtalhoAtual = menuAtalhoAtual;

        switch (this.menuAtalhoAtual)
        {
            case MenuAtalhoEnum.Inicio:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(true);
                painelDeExplicacaoDosItens.gameObject.SetActive(true);
                break;

            case MenuAtalhoEnum.TrocandoPosicao:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(false);
                break;
        }
    }

    public void MenuItem()
    {
        switch(menuItemAtual)
        {
            case MenuItemEnum.Inicio:
                EscolhendoOpcaoItem();
                break;

            case MenuItemEnum.AdicionandoNosAtalhos:
                AdicionandoItemNosAtalhos();
                break;

            case MenuItemEnum.TrocandoPosicao:
                TrocandoPosicaoItem();
                break;

            case MenuItemEnum.ConfirmandoJogarFora:
                ConfirmandoJogarItemFora();
                break;
        }
    }

    private void EscolhendoOpcaoItem()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecaoPainelDeEscolha > 0)
            {
                selecaoPainelDeEscolha--;

                AtualizarPainelDeEscolha(painelDeEscolhaDosItens);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecaoPainelDeEscolha < painelDeEscolhaDosItens.Opcoes.Length - 1)
            {
                selecaoPainelDeEscolha++;

                AtualizarPainelDeEscolha(painelDeEscolhaDosItens);

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
            switch(selecaoPainelDeEscolha)
            {
                case 0:
                    //UsarItem
                    break;

                case 1:
                    SetMenuItemAtual(MenuItemEnum.AdicionandoNosAtalhos);
                    break;

                case 2:
                    SetMenuItemAtual(MenuItemEnum.TrocandoPosicao);
                    break;

                case 3:
                    SetMenuItemAtual(MenuItemEnum.ConfirmandoJogarFora);
                    break;
            }

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
    }

    private void AdicionandoItemNosAtalhos()
    {

    }

    private void TrocandoPosicaoItem()
    {

    }

    private void ConfirmandoJogarItemFora()
    {

    }

    private void AtualizarInformacoesDoItem()
    {
        nomeDoItem.text = generalManager.Player.Inventario.Itens[selecao].Nome;
        descricaoDoItem.text = generalManager.Player.Inventario.Itens[selecao].Descricao;
    }

    public void AtualizarPosicaoDoPainelDeExplicacaoDosItens(float posX)
    {
        painelDeExplicacaoDosItens.transform.position = new Vector2(posX + (Colisao.GetWorldRect(painelDeExplicacaoDosItens).size.x / 2), painelDeExplicacaoDosItens.transform.position.y);
    }

    public void IniciarSelecaoItem(SelecaoItem selecaoItem)
    {
        for(int i = 0; i < generalManager.Hud.MenuDoInventario.ItemSlots.Length; i++)
        {
            if(generalManager.Hud.MenuDoInventario.ItemSlots[i] == selecaoItem)
            {
                selecao = i;
                break;
            }
        }

        //Nao iniciar o menu caso a selecao seja de um item vazio
        if(generalManager.Player.Inventario.Itens[selecao].ID == 0)
        {
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);
            return;
        }

        selecaoPainelDeEscolha = 0;

        SetMenuItemAtual(MenuItemEnum.Inicio);

        AtualizarInformacoesDoItem();
        IniciarPainelDeEscolha(painelDeEscolhaDosItens, selecaoItem);
        AtualizarPainelDeEscolha(painelDeEscolhaDosItens);
    }

    private void IniciarPainelDeEscolha(PainelDeEscolhaDosItens painelDeEscolha, SelecaoItem selecaoItem)
    {
        Rect retanguloGlobal = Colisao.GetWorldRect(selecaoItem.GetComponent<RectTransform>());
        painelDeEscolha.transform.position = new Vector2(selecaoItem.transform.position.x, selecaoItem.transform.position.y - (retanguloGlobal.size.y / 2) - (retanguloGlobal.size.y / 21));

        //Se o painel estiver na ultima coluna do grid de itens, ajustar a posicao dele para ele nao ultrapassar o limite direito da tela
        if(painelDeEscolha.transform.position.x > generalManager.Hud.MenuDoInventario.ItemSlots[1].transform.position.x)
        {
            painelDeEscolha.transform.position = new Vector2(selecaoItem.transform.position.x + (retanguloGlobal.size.x / 2) - (Colisao.GetWorldRect(painelDeEscolha.GetComponent<RectTransform>()).size.x / 2), painelDeEscolha.transform.position.y);
        }
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolhaDosItens painelDeEscolha)
    {
        painelDeEscolha.Selecionar(selecaoPainelDeEscolha);
    }

    public void IniciarSelecaoAtalho(SelecaoAtalho selecaoAtalho)
    {
        for (int i = 0; i < generalManager.Hud.MenuDoInventario.AtalhoSlots.Length; i++)
        {
            if (generalManager.Hud.MenuDoInventario.AtalhoSlots[i] == selecaoAtalho)
            {
                selecao = i;
                break;
            }
        }

        //Nao iniciar o menu caso a selecao seja de um item vazio
        if (generalManager.Player.Inventario.AtalhosDeItens[selecao].ID == 0)
        {
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);
            return;
        }

        selecaoPainelDeEscolha = 0;

        SetMenuAtalhoAtual(MenuAtalhoEnum.Inicio);

        AtualizarInformacoesDoItem();
        IniciarPainelDeEscolha(painelDeEscolhaDosAtalhos, selecaoAtalho);
        AtualizarPainelDeEscolha(painelDeEscolhaDosAtalhos);
    }
}
