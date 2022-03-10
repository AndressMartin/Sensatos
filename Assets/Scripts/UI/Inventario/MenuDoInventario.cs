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
    [SerializeField] private RectTransform menuDasArmas;
    [SerializeField] private RectTransform menuDosItens;
    [SerializeField] private RectTransform menuDasRoupas;


    //Enums
    public enum Menu { Inicio, Arma, Item, Roupa, ItensChave, Missoes, Conquistas}

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    [SerializeField] private SelecaoDoInventario selecaoInicial;
    private SelecaoDoInventario selecaoAtual;

    //Setters
    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;
    }

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Variaveis
        ativo = false;
        menuAtual = Menu.Inicio;

        selecaoAtual = selecaoInicial;

        telaInicial.gameObject.SetActive(false);
        menuDasArmas.gameObject.SetActive(false);
        menuDosItens.gameObject.SetActive(false);
        menuDasRoupas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (ativo == false)
        {
            return;
        }

        switch (menuAtual)
        {
            case Menu.Inicio:
            {
                MenuInicial();
                break;
            }
        }
    }

    public void AbrirOInventario()
    {
        generalManager.PauseManager.Pausar(true);
        generalManager.PauseManager.SetPermitirInput(false);

        ativo = true;
        menuAtual = Menu.Inicio;

        telaInicial.gameObject.SetActive(true);
        menuDasArmas.gameObject.SetActive(false);
        menuDosItens.gameObject.SetActive(false);
        menuDasRoupas.gameObject.SetActive(false);

        selecaoAtual.Selecionado(false);
        selecaoAtual = selecaoInicial;
        selecaoAtual.Selecionado(true);

        AtualizarInformacoesJogador();
    }

    public void FecharOInventario()
    {
        generalManager.PauseManager.Pausar(false);
        generalManager.PauseManager.SetPermitirInput(true);

        telaInicial.gameObject.SetActive(false);
        menuDasArmas.gameObject.SetActive(false);
        menuDosItens.gameObject.SetActive(false);
        menuDasRoupas.gameObject.SetActive(false);

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

    private void AtualizarInformacoesJogador()
    {
        barraDeVida.fillAmount = generalManager.Player.Vida / generalManager.Player.VidaMax;
        dinheiroTexto.text = "R$ " + generalManager.Player.Inventario.Dinheiro;
    }
}
