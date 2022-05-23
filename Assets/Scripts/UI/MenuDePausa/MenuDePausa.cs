using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDePausa : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private RectTransform telaEscura;
    [SerializeField] private RectTransform telaInicial;
    [SerializeField] private MenuSalvar menuSalvar;
    [SerializeField] private MenuOpcoes menuOpcoes;

    [SerializeField] private PainelDeEscolha opcoesMenuInicial;
    [SerializeField] private PainelDeEscolha opcoesConfirmacaoParaVoltarAoMenuInicial;

    //Enums
    public enum Menu { Inicio, Salvar, Opcoes, ConfirmacaoMenuPrincipal }

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    private int selecao;
    private int selecao2;

    //Getters
    public MenuSalvar GetMenuSalvar => menuSalvar;

    //Setters
    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch (this.menuAtual)
        {
            case Menu.Inicio:
                telaInicial.gameObject.SetActive(true);
                menuSalvar.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                opcoesConfirmacaoParaVoltarAoMenuInicial.gameObject.SetActive(false);

                AtualizarPainelDeEscolha(opcoesMenuInicial, selecao);
                break;

            case Menu.Salvar:
                telaInicial.gameObject.SetActive(false);
                menuSalvar.gameObject.SetActive(true);
                menuOpcoes.gameObject.SetActive(false);
                opcoesConfirmacaoParaVoltarAoMenuInicial.gameObject.SetActive(false);
                break;

            case Menu.Opcoes:
                telaInicial.gameObject.SetActive(false);
                menuSalvar.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(true);
                opcoesConfirmacaoParaVoltarAoMenuInicial.gameObject.SetActive(false);
                break;

            case Menu.ConfirmacaoMenuPrincipal:
                telaInicial.gameObject.SetActive(false);
                menuSalvar.gameObject.SetActive(false);
                menuOpcoes.gameObject.SetActive(false);
                opcoesConfirmacaoParaVoltarAoMenuInicial.gameObject.SetActive(true);
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

        telaEscura.gameObject.SetActive(false);
        telaInicial.gameObject.SetActive(false);
        menuSalvar.gameObject.SetActive(false);
        menuOpcoes.gameObject.SetActive(false);
        opcoesConfirmacaoParaVoltarAoMenuInicial.gameObject.SetActive(false);
    }

    private void IniciarComponentes()
    {
        menuOpcoes.Iniciar();
    }

    private void Update()
    {
        if (ativo == false)
        {
            if (generalManager.PauseManager.PermitirInput == true)
            {
                if (generalManager.Hud.MenuAberto == HUDScript.Menu.Nenhum && generalManager.Player.GetEstado != Player.Estado.Morto)
                {
                    //Abrir o menu de pausa
                    if (InputManager.Pausar())
                    {
                        AbrirOMenuDePausa();
                    }
                }
            }

            return;
        }

        //Fechar o menu de pausa
        if (InputManager.Pausar())
        {
            FecharOMenuDePausa();
        }

        //Executa as funcoes do menu atual
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;

            case Menu.Salvar:
                MenuSalvar();
                break;

            case Menu.Opcoes:
                MenuOpcoes();
                break;

            case Menu.ConfirmacaoMenuPrincipal:
                ConfirmacaoMenuPrincipal();
                break;
        }
    }

    private void AbrirOMenuDePausa()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Pausa);
        generalManager.PauseManager.Pausar(true);
        generalManager.PauseManager.SetPermitirInput(false);

        ativo = true;

        telaEscura.gameObject.SetActive(true);
        SetMenuAtual(Menu.Inicio);

        selecao = 0;
        AtualizarPainelDeEscolha(opcoesMenuInicial, selecao);

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Pausar);
    }

    private void FecharOMenuDePausa()
    {
        //Salva as configuracoes do jogo caso o jogador feche o menu de pausa enquanto esta no menu de opcoes
        if (menuAtual == Menu.Opcoes)
        {
            SaveConfiguracoes.AtualizarConfiguracoes();
            SaveConfiguracoes.SalvarConfiguracoes();
        }

        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Nenhum);
        generalManager.PauseManager.Pausar(false);
        generalManager.PauseManager.SetPermitirInput(true);

        telaEscura.gameObject.SetActive(false);
        telaInicial.gameObject.SetActive(false);
        menuSalvar.gameObject.SetActive(false);
        menuOpcoes.gameObject.SetActive(false);
        opcoesConfirmacaoParaVoltarAoMenuInicial.gameObject.SetActive(false);

        ativo = false;

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Despausar);
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

        //Voltar
        if (InputManager.Voltar())
        {
            FecharOMenuDePausa();
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            switch (selecao)
            {
                case 0:
                    FecharOMenuDePausa();
                    break;

                case 1:
                    if(GameManager.instance.ModoDeJogo == GameManager.Modo.Cidade && generalManager.ModoDeJogoManager.SaveLiberado == true)
                    {
                        SetMenuAtual(Menu.Salvar);

                        menuSalvar.IniciarScrool();

                        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    }
                    else
                    {
                        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
                    }
                    
                    break;

                case 2:
                    SetMenuAtual(Menu.Opcoes);

                    menuOpcoes.IniciarScrool();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 3:
                    SetMenuAtual(Menu.ConfirmacaoMenuPrincipal);

                    selecao2 = 0;
                    AtualizarPainelDeEscolha(opcoesConfirmacaoParaVoltarAoMenuInicial, selecao2);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void MenuSalvar()
    {
        menuSalvar.EscolhendoSave();
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

    private void ConfirmacaoMenuPrincipal()
    {
        //Mover para cima
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;

                AtualizarPainelDeEscolha(opcoesConfirmacaoParaVoltarAoMenuInicial, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Direita())
        {
            if (selecao2 < opcoesConfirmacaoParaVoltarAoMenuInicial.Opcoes.Length - 1)
            {
                selecao2++;

                AtualizarPainelDeEscolha(opcoesConfirmacaoParaVoltarAoMenuInicial, selecao2);

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
                    LevelLoaderScript.Instance.CarregarNivel("MenuPrincipal");

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }
}
