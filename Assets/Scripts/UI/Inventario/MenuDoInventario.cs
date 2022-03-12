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
    [SerializeField] private TMP_Text dinheiroTexto;
    [SerializeField] private RectTransform telaInicial;
    [SerializeField] private RectTransform telaEscuraDaTelaInicial;
    [SerializeField] private MenuDasArmas menuDasArmas;
    [SerializeField] private RectTransform menuDosItens;
    [SerializeField] private RectTransform menuDasRoupas;

    //Enums
    public enum Menu { Inicio, Arma, Item, Roupa, ItensChave, Missoes, Conquistas}

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    [SerializeField] private SelecaoDoInventario selecaoInicial;
    private SelecaoDoInventario selecaoAtual;

    [SerializeField] private SelecaoArma[] armaSlots;

    //Getters
    public MenuDasArmas MenuDasArmas => menuDasArmas;

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
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        AbrirOInventario();
                    }
                }
            }

            return;
        }

        //Fechar o inventario
        if (Input.GetKeyDown(KeyCode.I))
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
    }

    private void MenuInicial()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            selecaoAtual.Confirmar(this);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FecharOInventario();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(selecaoAtual.Selecao.Cima != null)
            {
                selecaoAtual.Selecionado(false);

                selecaoAtual = selecaoAtual.Selecao.Cima;
                selecaoAtual.Selecionado(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selecaoAtual.Selecao.Baixo != null)
            {
                selecaoAtual.Selecionado(false);

                selecaoAtual = selecaoAtual.Selecao.Baixo;
                selecaoAtual.Selecionado(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selecaoAtual.Selecao.Esquerda != null)
            {
                selecaoAtual.Selecionado(false);

                selecaoAtual = selecaoAtual.Selecao.Esquerda;
                selecaoAtual.Selecionado(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selecaoAtual.Selecao.Direita != null)
            {
                selecaoAtual.Selecionado(false);

                selecaoAtual = selecaoAtual.Selecao.Direita;
                selecaoAtual.Selecionado(true);
            }
        }
    }

    private void MenuArma()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            menuDasArmas.Subir();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            menuDasArmas.Descer();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            menuDasArmas.ConfirmarArma();
            SetMenuAtual(Menu.Inicio);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetMenuAtual(Menu.Inicio);
        }
    }

    private void AtualizarInformacoes()
    {
        AtualizarInformacoesJogador();
        AtualizarInformacoesArmas();
    }

    private void AtualizarInformacoesJogador()
    {
        barraDeVida.fillAmount = generalManager.Player.Vida / generalManager.Player.VidaMax;
        dinheiroTexto.text = "R$ " + generalManager.Player.Inventario.Dinheiro;
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
}
