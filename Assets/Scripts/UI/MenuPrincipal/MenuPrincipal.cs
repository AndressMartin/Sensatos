using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private RectTransform telaDoLogo;
    [SerializeField] private RectTransform telaAperteStart;
    [SerializeField] private RectTransform telaInicial;
    [SerializeField] private MenuNovoJogo menuNovoJogo;
    [SerializeField] private MenuCarregarJogo menuCarregarJogo;
    [SerializeField] private MenuOpcoes menuOpcoes;

    [SerializeField] private RectTransform menuSobreOJogo;
    [SerializeField] private RectTransform menuControles;
    [SerializeField] private TelaDeCreditos menuCreditos;

    [SerializeField] private PainelDeEscolha opcoesMenuInicial;
    [SerializeField] private PainelDeEscolha painelDeConfirmacaoParaSairDoJogo;

    [SerializeField] private Animator animacaoTitulo;

    //Enums
    public enum Menu { AperteStart, Inicio, NovoJogo, CarregarJogo, Opcoes, ConfirmacaoParaSairDoJogo, SobreOJogo, Controles, Creditos, BloquearComandos }

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    private int selecao;
    private int selecao2;

    [SerializeField] private AudioClip musicaMenuPrincipal;

    //Getters
    public MenuNovoJogo GetMenuNovoJogo => menuNovoJogo;
    public MenuCarregarJogo GetMenuCarregarJogo => menuCarregarJogo;

    //Setters
    public void SetAtivo(bool ativo)
    {
        this.ativo = ativo;
        SetMenuAtual(Menu.AperteStart);
    }

    public void MostrarTitulo()
    {
        animacaoTitulo.Play("Aparecendo");
    }

    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch (this.menuAtual)
        {
            case Menu.AperteStart:
                telaAperteStart.gameObject.SetActive(true);
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);

                menuSobreOJogo.gameObject.SetActive(false);
                menuControles.gameObject.SetActive(false);
                menuCreditos.gameObject.SetActive(false);
                break;

            case Menu.Inicio:
                telaAperteStart.gameObject.SetActive(false);
                telaInicial.gameObject.SetActive(true);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);

                menuSobreOJogo.gameObject.SetActive(false);
                menuControles.gameObject.SetActive(false);
                menuCreditos.gameObject.SetActive(false);

                AtualizarPainelDeEscolha(opcoesMenuInicial, selecao);
                break;

            case Menu.NovoJogo:
                telaAperteStart.gameObject.SetActive(false);
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(true);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);

                menuSobreOJogo.gameObject.SetActive(false);
                menuControles.gameObject.SetActive(false);
                menuCreditos.gameObject.SetActive(false);
                break;

            case Menu.CarregarJogo:
                telaAperteStart.gameObject.SetActive(false);
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(true);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);

                menuSobreOJogo.gameObject.SetActive(false);
                menuControles.gameObject.SetActive(false);
                menuCreditos.gameObject.SetActive(false);
                break;

            case Menu.Opcoes:
                telaAperteStart.gameObject.SetActive(false);
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(true);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);

                menuSobreOJogo.gameObject.SetActive(false);
                menuControles.gameObject.SetActive(false);
                menuCreditos.gameObject.SetActive(false);
                break;

            case Menu.ConfirmacaoParaSairDoJogo:
                telaAperteStart.gameObject.SetActive(false);
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(true);

                menuSobreOJogo.gameObject.SetActive(false);
                menuControles.gameObject.SetActive(false);
                menuCreditos.gameObject.SetActive(false);
                break;

            case Menu.SobreOJogo:
                telaAperteStart.gameObject.SetActive(false);
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);

                menuSobreOJogo.gameObject.SetActive(true);
                menuControles.gameObject.SetActive(false);
                menuCreditos.gameObject.SetActive(false);
                break;

            case Menu.Controles:
                telaAperteStart.gameObject.SetActive(false);
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);

                menuSobreOJogo.gameObject.SetActive(false);
                menuControles.gameObject.SetActive(true);
                menuCreditos.gameObject.SetActive(false);
                break;

            case Menu.Creditos:
                telaAperteStart.gameObject.SetActive(false);
                telaInicial.gameObject.SetActive(false);
                menuNovoJogo.gameObject.SetActive(false);
                menuCarregarJogo.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                painelDeConfirmacaoParaSairDoJogo.gameObject.SetActive(false);

                menuSobreOJogo.gameObject.SetActive(false);
                menuControles.gameObject.SetActive(false);
                menuCreditos.gameObject.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Variaveis
        ativo = false;
        selecao = 0;
        selecao2 = 0;

        IniciarComponentes();

        telaDoLogo.gameObject.SetActive(true);

        SetMenuAtual(Menu.AperteStart);

        telaAperteStart.gameObject.SetActive(false);
    }

    private void IniciarComponentes()
    {
        AtualizarPainelDeEscolha(opcoesMenuInicial, selecao);

        menuNovoJogo.Iniciar();
        menuCarregarJogo.Iniciar();
        menuOpcoes.Iniciar();
        menuCreditos.Iniciar();
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
            case Menu.AperteStart:
                MenuAperteStart();
                break;

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

            case Menu.SobreOJogo:
                MenuSobreOJogo();
                break;

            case Menu.Controles:
                MenuControles();
                break;

            case Menu.Creditos:
                MenuCreditos();
                break;

            case Menu.ConfirmacaoParaSairDoJogo:
                ConfirmacaoParaSairDoJogo();
                break;
        }
    }

    private void MenuAperteStart()
    {
        if(InputManager.QualquerBotao())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
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
                    SetMenuAtual(Menu.SobreOJogo);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 4:
                    SetMenuAtual(Menu.Controles);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 5:
                    SetMenuAtual(Menu.Creditos);

                    menuCreditos.IniciarAnimacao();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 6:
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

    private void MenuSobreOJogo()
    {
        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }

    private void MenuControles()
    {
        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }

    private void MenuCreditos()
    {
        //Voltar
        if (InputManager.Voltar())
        {
            menuCreditos.FicarInvisivel();

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

    public void TocarMusica()
    {
        generalManager.MusicManager.SetMusica(musicaMenuPrincipal);
        generalManager.MusicManager.TocarMusica();
    }

    private void FecharOJogo()
    {
        Application.Quit();
    }

    public void IniciarNovoJogo()
    {
        SetMenuAtual(Menu.BloquearComandos);

        GameManager.instance.IniciarNovoJogo();
    }

    public void IniciarJogo()
    {
        SetMenuAtual(Menu.BloquearComandos);

        GameManager.instance.IniciarJogo();
    }
}
