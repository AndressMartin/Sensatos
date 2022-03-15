using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDoInventario : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private Image barraDeVida;
    [SerializeField] private TMP_Text vidaTexto;
    [SerializeField] private TMP_Text dinheiroTexto;
    [SerializeField] private RectTransform telaInicial;
    [SerializeField] private RectTransform telaEscuraDaTelaInicial;
    [SerializeField] private MenuDasArmas menuDasArmas;
    [SerializeField] private MenuDosItens menuDosItens;
    [SerializeField] private RectTransform menuDasRoupas;

    //Getters
    public float PosicaoXBarraDeExplicacaoItens => armaSlots[0].transform.position.x - (Colisao.GetWorldRect(armaSlots[0].GetComponent<RectTransform>()).size.x / 2);
    public float PosicaoXBarraDeExplicacaoAtalhos => itemSlots[0].transform.position.x - (Colisao.GetWorldRect(itemSlots[0].GetComponent<RectTransform>()).size.x / 2);

    //Enums
    public enum Menu { Inicio, Arma, Item, Atalho, Roupa, ItensChave, Missoes, Conquistas}

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    [SerializeField] private SelecaoDoInventario selecaoInicial;
    private SelecaoDoInventario selecaoAtual;

    [SerializeField] private SelecaoArma[] armaSlots;
    [SerializeField] private SelecaoItem[] itemSlots;
    [SerializeField] private SelecaoAtalho[] atalhoSlots;

    //Getters
    public MenuDasArmas MenuDasArmas => menuDasArmas;
    public MenuDosItens MenuDosItens => menuDosItens;
    public SelecaoItem[] ItemSlots => itemSlots;
    public SelecaoAtalho[] AtalhoSlots => atalhoSlots;

    //Setters
    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch(this.menuAtual)
        {
            case Menu.Inicio:
                menuDasArmas.gameObject.SetActive(false);
                menuDosItens.gameObject.SetActive(false);
                menuDasRoupas.gameObject.SetActive(false);
                telaEscuraDaTelaInicial.gameObject.SetActive(false);
                AtualizarInformacoes();
                break;

            case Menu.Arma:
                menuDasArmas.gameObject.SetActive(true);
                telaEscuraDaTelaInicial.gameObject.SetActive(true);
                break;

            case Menu.Item:
                menuDosItens.gameObject.SetActive(true);
                telaEscuraDaTelaInicial.gameObject.SetActive(true);
                break;

            case Menu.Atalho:
                menuDosItens.gameObject.SetActive(true);
                telaEscuraDaTelaInicial.gameObject.SetActive(true);
                break;

            case Menu.Roupa:
                menuDasRoupas.gameObject.SetActive(true);
                telaEscuraDaTelaInicial.gameObject.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Variaveis
        ativo = false;
        menuAtual = Menu.Inicio;

        selecaoAtual = selecaoInicial;

        menuDasArmas.Iniciar();

        telaInicial.gameObject.SetActive(false);
        menuDasArmas.gameObject.SetActive(false);
        menuDosItens.gameObject.SetActive(false);
        menuDasRoupas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (ativo == false)
        {
            if (generalManager.PauseManager.PermitirInput == true)
            {
                if (generalManager.PauseManager.JogoPausado == false)
                {
                    //Abrir o inventario
                    if (InputManager.AbrirOInventario())
                    {
                        AbrirOInventario();
                    }
                }
            }

            return;
        }

        //Fechar o inventario
        if (InputManager.AbrirOInventario())
        {
            FecharOInventario();
        }

        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;

            case Menu.Arma:
                MenuArma();
                break;

            case Menu.Item:
                MenuItem();
                break;
        }
    }

    public void AbrirOInventario()
    {
        generalManager.PauseManager.Pausar(true);
        generalManager.PauseManager.SetPermitirInput(false);

        ativo = true;

        telaInicial.gameObject.SetActive(true);
        SetMenuAtual(Menu.Inicio);

        selecaoAtual.Selecionado(false);
        selecaoAtual = selecaoInicial;
        selecaoAtual.Selecionado(true);

        AtualizarInformacoes();

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.AbrirOInventario);
    }

    public void FecharOInventario()
    {
        generalManager.PauseManager.Pausar(false);
        generalManager.PauseManager.SetPermitirInput(true);

        telaInicial.gameObject.SetActive(false);
        menuDasArmas.gameObject.SetActive(false);
        menuDosItens.gameObject.SetActive(false);
        menuDasRoupas.gameObject.SetActive(false);

        if(generalManager.Player.Animacao.ArmaEquipadaVisual != "")
        {
            generalManager.Player.AtualizarArma();
        }

        ativo = false;

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.FecharOInventario);
    }

    private void MenuInicial()
    {
        //Confirmar
        if (InputManager.Confirmar())
        {
            selecaoAtual.Confirmar(this);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }

        //Voltar
        if (InputManager.Voltar())
        {
            FecharOInventario();
        }

        //Mover para cima
        if (InputManager.Cima())
        {
            if(selecaoAtual.Selecao.Cima != null)
            {
                selecaoAtual.Selecionado(false);

                selecaoAtual = selecaoAtual.Selecao.Cima;
                selecaoAtual.Selecionado(true);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecaoAtual.Selecao.Baixo != null)
            {
                selecaoAtual.Selecionado(false);

                selecaoAtual = selecaoAtual.Selecao.Baixo;
                selecaoAtual.Selecionado(true);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecaoAtual.Selecao.Esquerda != null)
            {
                selecaoAtual.Selecionado(false);

                selecaoAtual = selecaoAtual.Selecao.Esquerda;
                selecaoAtual.Selecionado(true);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecaoAtual.Selecao.Direita != null)
            {
                selecaoAtual.Selecionado(false);

                selecaoAtual = selecaoAtual.Selecao.Direita;
                selecaoAtual.Selecionado(true);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }
    }

    private void MenuArma()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            menuDasArmas.Subir();
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            menuDasArmas.Descer();
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            menuDasArmas.ConfirmarArma();
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.EquiparArma);
        }

        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }

    private void MenuItem()
    {
        menuDosItens.MenuItem();
    }

    private void AtualizarInformacoes()
    {
        AtualizarInformacoesJogador();
        AtualizarInformacoesArmas();
        AtualizarInformacoesItens();
    }

    private void AtualizarInformacoesJogador()
    {
        barraDeVida.fillAmount = (float)generalManager.Player.Vida / (float)generalManager.Player.VidaMax;
        vidaTexto.text = generalManager.Player.Vida.ToString() + "/" + generalManager.Player.VidaMax.ToString();
        dinheiroTexto.text = generalManager.Player.Inventario.Dinheiro.ToString();
    }

    private void AtualizarInformacoesArmas()
    {
        for (int i = 0; i < armaSlots.Length; i++)
        {
            if (generalManager.Player.Inventario.ArmaSlot[i] != null)
            {
                armaSlots[i].AtualizarInformacoes(generalManager.Player.Inventario.ArmaSlot[i]);
            }
            else
            {
                armaSlots[i].ZerarInformacoes();
            }
        }
    }

    private void AtualizarInformacoesItens()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (generalManager.Player.Inventario.Itens[i].ID != 0)
            {
                itemSlots[i].AtualizarInformacoes(generalManager.Player.Inventario.Itens[i]);
            }
            else
            {
                itemSlots[i].ZerarInformacoes();
            }
        }
    }
}
