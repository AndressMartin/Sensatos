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
    [SerializeField] private PainelDeEscolha painelDeEscolhaDosItens;
    [SerializeField] private PainelDeEscolha painelDeEscolhaDosAtalhos;
    [SerializeField] private PainelDeEscolha painelDeConfirmacaoParaJogarUmItemFora;
    [SerializeField] private RectTransform painelDeExplicacaoDosItens;
    [SerializeField] private RectTransform selecaoDoItem;

    [SerializeField] private RectTransform telaEscura;

    [SerializeField] private TMP_Text nomeDoItem;
    [SerializeField] private TMP_Text descricaoDoItem;

    //Enums
    public enum MenuItemEnum { Inicio, AdicionandoNosAtalhos, TrocandoPosicao, ConfirmandoJogarFora }
    public enum MenuAtalhoEnum { Inicio, TrocandoPosicao }

    //Variaveis
    private int selecao;
    private int selecao2;
    private int selecaoPainelDeEscolha;

    private MenuItemEnum menuItemAtual;
    private MenuAtalhoEnum menuAtalhoAtual;

    private bool iniciado = false;

    //Variaveis Fixas
    private readonly int numeroDeColunasDosItens = 3;
    private readonly int numeroDeColunasDosAtalhos = 2;

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
        selecao2 = 0;
        selecaoPainelDeEscolha = 0;

        menuItemAtual = MenuItemEnum.Inicio;
        menuAtalhoAtual = MenuAtalhoEnum.Inicio;

        iniciado = true;
    }

    public void AtualizarPosicaoDoPainelDeExplicacaoDosItens(float posX)
    {
        painelDeExplicacaoDosItens.transform.position = new Vector2(posX + (Colisao.GetWorldRect(painelDeExplicacaoDosItens).size.x / 2), painelDeExplicacaoDosItens.transform.position.y);
    }

    private void AtualizarInformacoesDoItem()
    {
        nomeDoItem.text = generalManager.Player.Inventario.Itens[selecao].Nome;
        descricaoDoItem.text = generalManager.Player.Inventario.Itens[selecao].Descricao;
    }

    public void AtualizarPosicaoDaSelecaoDoItem(SelecaoItem item)
    {
        selecaoDoItem.transform.position = item.transform.position;
    }

    private void IniciarPainelDeEscolha(PainelDeEscolha painelDeEscolha, SelecaoItem selecaoItem)
    {
        Rect retanguloGlobal = Colisao.GetWorldRect(selecaoItem.GetComponent<RectTransform>());
        painelDeEscolha.transform.position = new Vector2(selecaoItem.transform.position.x, selecaoItem.transform.position.y - (retanguloGlobal.size.y / 2) - (retanguloGlobal.size.y / 21));

        //Se o painel estiver na ultima coluna do grid de itens, ajustar a posicao dele para ele nao ultrapassar o limite direito da tela
        if(painelDeEscolha.transform.position.x > generalManager.Hud.MenuDoInventario.ItemSlots[1].transform.position.x)
        {
            painelDeEscolha.transform.position = new Vector2(selecaoItem.transform.position.x + (retanguloGlobal.size.x / 2) - (Colisao.GetWorldRect(painelDeEscolha.GetComponent<RectTransform>()).size.x / 2), painelDeEscolha.transform.position.y);
        }
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }

    #region Menu dos Itens

    private void SetMenuItemAtual(MenuItemEnum menuItemAtual)
    {
        this.menuItemAtual = menuItemAtual;

        switch (this.menuItemAtual)
        {
            case MenuItemEnum.Inicio:
                painelDeEscolhaDosItens.gameObject.SetActive(true);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeConfirmacaoParaJogarUmItemFora.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(true);
                selecaoDoItem.gameObject.SetActive(false);
                telaEscura.gameObject.SetActive(true);

                AtualizarPainelDeEscolha(painelDeEscolhaDosItens, selecaoPainelDeEscolha);
                break;

            case MenuItemEnum.AdicionandoNosAtalhos:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeConfirmacaoParaJogarUmItemFora.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(false);
                selecaoDoItem.gameObject.SetActive(true);
                telaEscura.gameObject.SetActive(false);
                break;

            case MenuItemEnum.TrocandoPosicao:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeConfirmacaoParaJogarUmItemFora.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(false);
                selecaoDoItem.gameObject.SetActive(true);
                telaEscura.gameObject.SetActive(false);
                break;

            case MenuItemEnum.ConfirmandoJogarFora:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeConfirmacaoParaJogarUmItemFora.gameObject.SetActive(true);
                painelDeExplicacaoDosItens.gameObject.SetActive(false);
                selecaoDoItem.gameObject.SetActive(false);
                telaEscura.gameObject.SetActive(true);
                break;
        }
    }

    public void IniciarSelecaoItem(SelecaoItem selecaoItem)
    {
        for (int i = 0; i < generalManager.Hud.MenuDoInventario.ItemSlots.Length; i++)
        {
            if (generalManager.Hud.MenuDoInventario.ItemSlots[i] == selecaoItem)
            {
                selecao = i;
                break;
            }
        }

        //Nao iniciar o menu caso a selecao seja de um item vazio
        if (generalManager.Player.Inventario.Itens[selecao].ID == Listas.instance.ListaDeItens.GetID["ItemVazio"])
        {
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);
            return;
        }

        selecaoPainelDeEscolha = 0;

        SetMenuItemAtual(MenuItemEnum.Inicio);

        AtualizarInformacoesDoItem();
        IniciarPainelDeEscolha(painelDeEscolhaDosItens, selecaoItem);
        AtualizarPainelDeEscolha(painelDeEscolhaDosItens, selecaoPainelDeEscolha);

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    public void MenuItem()
    {
        switch (menuItemAtual)
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

                AtualizarPainelDeEscolha(painelDeEscolhaDosItens, selecaoPainelDeEscolha);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecaoPainelDeEscolha < painelDeEscolhaDosItens.Opcoes.Length - 1)
            {
                selecaoPainelDeEscolha++;

                AtualizarPainelDeEscolha(painelDeEscolhaDosItens, selecaoPainelDeEscolha);

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
            switch (selecaoPainelDeEscolha)
            {
                case 0:
                    UsarItemNoInventario();
                    break;

                case 1:
                    SetMenuItemAtual(MenuItemEnum.AdicionandoNosAtalhos);

                    selecao2 = 0;
                    AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 2:
                    SetMenuItemAtual(MenuItemEnum.TrocandoPosicao);

                    selecao2 = selecao;
                    AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.ItemSlots[selecao]);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 3:
                    SetMenuItemAtual(MenuItemEnum.ConfirmandoJogarFora);

                    selecao2 = 0;
                    IniciarPainelDeEscolha(painelDeConfirmacaoParaJogarUmItemFora, generalManager.Hud.MenuDoInventario.ItemSlots[selecao]);
                    AtualizarPainelDeEscolha(painelDeConfirmacaoParaJogarUmItemFora, selecao2);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void AdicionandoItemNosAtalhos()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao2 - numeroDeColunasDosAtalhos >= 0)
            {
                selecao2 -= numeroDeColunasDosAtalhos;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao2 + numeroDeColunasDosAtalhos < generalManager.Hud.MenuDoInventario.AtalhoSlots.Length)
            {
                selecao2 += numeroDeColunasDosAtalhos;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecao2 < generalManager.Hud.MenuDoInventario.AtalhoSlots.Length - 1)
            {
                selecao2++;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuItemAtual(MenuItemEnum.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            generalManager.Player.Inventario.AdicionarAtalho(selecao2, generalManager.Player.Inventario.Itens[selecao]);

            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
    }

    private void TrocandoPosicaoItem()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if(selecao2 - numeroDeColunasDosItens >= 0)
            {
                selecao2 -= numeroDeColunasDosItens;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.ItemSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao2 + numeroDeColunasDosItens < generalManager.Hud.MenuDoInventario.ItemSlots.Length)
            {
                selecao2 += numeroDeColunasDosItens;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.ItemSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.ItemSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecao2 < generalManager.Hud.MenuDoInventario.ItemSlots.Length - 1)
            {
                selecao2++;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.ItemSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuItemAtual(MenuItemEnum.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            generalManager.Player.Inventario.MoverItem(selecao, selecao2);

            generalManager.Hud.MenuDoInventario.SetSelecaoAtual(generalManager.Hud.MenuDoInventario.ItemSlots[selecao2]);
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
    }

    private void ConfirmandoJogarItemFora()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao2 > 0)
            {
                selecao2--;

                AtualizarPainelDeEscolha(painelDeConfirmacaoParaJogarUmItemFora, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao2 < painelDeConfirmacaoParaJogarUmItemFora.Opcoes.Length - 1)
            {
                selecao2++;

                AtualizarPainelDeEscolha(painelDeConfirmacaoParaJogarUmItemFora, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuItemAtual(MenuItemEnum.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            switch (selecao2)
            {
                case 0:
                    SetMenuItemAtual(MenuItemEnum.Inicio);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
                    break;

                case 1:
                    generalManager.Player.Inventario.Itens[selecao].JogarFora(generalManager.Player);

                    generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void UsarItemNoInventario()
    {
        if(generalManager.Player.Inventario.Itens[selecao].UsarNoInventario(generalManager.Player) == true)
        {
            generalManager.Hud.MenuDoInventario.FecharOInventario();

            generalManager.Player.UsarItem(generalManager.Player.Inventario.Itens[selecao]);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
        else
        {
            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
        }
    }

    #endregion

    #region Menu dos Atalhos

    private void SetMenuAtalhoAtual(MenuAtalhoEnum menuAtalhoAtual)
    {
        this.menuAtalhoAtual = menuAtalhoAtual;

        switch (this.menuAtalhoAtual)
        {
            case MenuAtalhoEnum.Inicio:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(true);
                painelDeConfirmacaoParaJogarUmItemFora.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(true);
                selecaoDoItem.gameObject.SetActive(false);
                telaEscura.gameObject.SetActive(true);

                AtualizarPainelDeEscolha(painelDeEscolhaDosAtalhos, selecaoPainelDeEscolha);
                break;

            case MenuAtalhoEnum.TrocandoPosicao:
                painelDeEscolhaDosItens.gameObject.SetActive(false);
                painelDeEscolhaDosAtalhos.gameObject.SetActive(false);
                painelDeConfirmacaoParaJogarUmItemFora.gameObject.SetActive(false);
                painelDeExplicacaoDosItens.gameObject.SetActive(false);
                selecaoDoItem.gameObject.SetActive(true);
                telaEscura.gameObject.SetActive(false);
                break;
        }
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
        if (generalManager.Player.Inventario.AtalhosDeItens[selecao].ID == Listas.instance.ListaDeItens.GetID["ItemVazio"])
        {
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);
            return;
        }

        selecaoPainelDeEscolha = 0;

        SetMenuAtalhoAtual(MenuAtalhoEnum.Inicio);

        AtualizarInformacoesDoItem();
        IniciarPainelDeEscolha(painelDeEscolhaDosAtalhos, selecaoAtalho);
        AtualizarPainelDeEscolha(painelDeEscolhaDosAtalhos, selecaoPainelDeEscolha);

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    public void MenuAtalho()
    {
        switch (menuAtalhoAtual)
        {
            case MenuAtalhoEnum.Inicio:
                EscolhendoOpcaoAtalho();
                break;

            case MenuAtalhoEnum.TrocandoPosicao:
                TrocandoPosicaoAtalho();
                break;
        }
    }

    private void EscolhendoOpcaoAtalho()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecaoPainelDeEscolha > 0)
            {
                selecaoPainelDeEscolha--;

                AtualizarPainelDeEscolha(painelDeEscolhaDosAtalhos, selecaoPainelDeEscolha);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecaoPainelDeEscolha < painelDeEscolhaDosAtalhos.Opcoes.Length - 1)
            {
                selecaoPainelDeEscolha++;

                AtualizarPainelDeEscolha(painelDeEscolhaDosAtalhos, selecaoPainelDeEscolha);

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
            switch (selecaoPainelDeEscolha)
            {
                case 0:
                    generalManager.Player.Inventario.RemoverAtalho(selecao);

                    generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 1:
                    SetMenuAtalhoAtual(MenuAtalhoEnum.TrocandoPosicao);

                    selecao2 = selecao;
                    AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao]);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void TrocandoPosicaoAtalho()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao2 - numeroDeColunasDosAtalhos >= 0)
            {
                selecao2 -= numeroDeColunasDosAtalhos;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao2 + numeroDeColunasDosAtalhos < generalManager.Hud.MenuDoInventario.AtalhoSlots.Length)
            {
                selecao2 += numeroDeColunasDosAtalhos;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecao2 < generalManager.Hud.MenuDoInventario.AtalhoSlots.Length - 1)
            {
                selecao2++;
                AtualizarPosicaoDaSelecaoDoItem(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuAtalhoAtual(MenuAtalhoEnum.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            generalManager.Player.Inventario.MoverAtalho(selecao, selecao2);

            generalManager.Hud.MenuDoInventario.SetSelecaoAtual(generalManager.Hud.MenuDoInventario.AtalhoSlots[selecao2]);
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
    }

    #endregion
}
