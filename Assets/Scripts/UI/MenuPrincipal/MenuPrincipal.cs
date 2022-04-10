using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private RectTransform telaDoLogo;
    [SerializeField] private RectTransform telaInicial;
    [SerializeField] private MenuNovoJogo menuNovoJogo;
    [SerializeField] private MenuCarregarJogo menuCarregarJogo;
    [SerializeField] private MenuOpcoes menuOpcoes;

    [SerializeField] private PainelDeEscolha opcoesMenuInicial;
    [SerializeField] private PainelDeEscolha painelDeConfirmacaoParaSairDoJogo;

    //Enums
    public enum Menu { Inicio, NovoJogo, CarregarJogo, Opcoes, ConfirmacaoParaSairDoJogo }

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    private int selecao;
    private int selecao2;

    //Getters
    public MenuNovoJogo GetMenuNovoJogo => menuNovoJogo;
    public MenuCarregarJogo GetMenuCarregarJogo => menuCarregarJogo;

    //Setters
    public void SetAtivo(bool ativo)
    {
        this.ativo = ativo;
    }

    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch (this.menuAtual)
        {
            case Menu.Inicio:
                telaInicial.gameObject.SetActive(true);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);
                break;

            case Menu.NovoJogo:
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(true);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);
                break;

            case Menu.CarregarJogo:
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(true);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);
                break;

            case Menu.Opcoes:
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(true);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);
                break;

            case Menu.ConfirmacaoParaSairDoJogo:
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(true);
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
        selecao = 0;
        selecao2 = 0;

        IniciarComponentes();

        telaDoLogo.gameObject.SetActive(true);

        telaInicial.gameObject.SetActive(true);
        menuNovoJogo.gameObject.SetActive(false);
        menuCarregarJogo.gameObject.SetActive(false);
        menuOpcoes.gameObject.SetActive(false);
        painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);
    }

    private void IniciarComponentes()
    {
        AtualizarPainelDeEscolha(opcoesMenuInicial, selecao);

        menuNovoJogo.Iniciar();
        menuCarregarJogo.Iniciar();
        menuOpcoes.Iniciar();
    }

    private void Update()
    {
        if(ativo == false)
        {
            return;
        }

        //Executa as funcoes do menu atual
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;

            case Menu.NovoJogo:
                MenuNovoJogo();
                break;

            case Menu.CarregarJogo:
                MenuCarregarJogo();
                break;

            case Menu.Opcoes:
                MenuOpcoes();
                break;

            case Menu.ConfirmacaoParaSairDoJogo:
                ConfirmacaoParaSairDoJogo();
                break;
        }
    }

    private void MenuInicial()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao > 0)
            {
                selecao--;

                AtualizarPainelDeEscolha(opcoesMenuInicial, selecao);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao < opcoesMenuInicial.Opcoes.Length - 1)
            {
                selecao++;

                AtualizarPainelDeEscolha(opcoesMenuInicial, selecao);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            switch (selecao)
            {
                case 0:
                    SetMenuAtual(Menu.NovoJogo);

                    menuNovoJogo.IniciarScrool();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 1:
                    SetMenuAtual(Menu.CarregarJogo);

                    menuCarregarJogo.IniciarScrool();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 2:
                    SetMenuAtual(Menu.Opcoes);

                    menuOpcoes.IniciarScrool();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 3:
                    //Tela de Creditos

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 4:
                    SetMenuAtual(Menu.ConfirmacaoParaSairDoJogo);

                    selecao2 = 0;
                    AtualizarPainelDeEscolha(painelDeConfirmacaoParaSairDoJogo, selecao2);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void MenuNovoJogo()
    {
        menuNovoJogo.EscolhendoSave();
    }

    private void MenuCarregarJogo()
    {
        menuCarregarJogo.EscolhendoSave();
    }

    private void MenuOpcoes()
    {
        menuOpcoes.SelecionandoOpcoes();

        //Voltar
        if (InputManager.Voltar())
        {
            //Salva as configuracoes do jogo
            SaveConfiguracoes.AtualizarConfiguracoes();
            SaveConfiguracoes.SalvarConfiguracoes();

            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }

    private void ConfirmacaoParaSairDoJogo()
    {
        //Mover para cima
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;

                AtualizarPainelDeEscolha(painelDeConfirmacaoParaSairDoJogo, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Direita())
        {
            if (selecao2 < painelDeConfirmacaoParaSairDoJogo.Opcoes.Length - 1)
            {
                selecao2++;

                AtualizarPainelDeEscolha(painelDeConfirmacaoParaSairDoJogo, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            switch (selecao2)
            {
                case 0:
                    SetMenuAtual(Menu.Inicio);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
                    break;

                case 1:
                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);

                    FecharOJogo();
                    break;
            }
        }
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }

    private void FecharOJogo()
    {
        Application.Quit();
    }

    public void IniciarNovoJogo()
    {
        GameManager.instance.IniciarNovoJogo();
    }

    public void IniciarJogo()
    {
        GameManager.instance.IniciarJogo();
    }
}
