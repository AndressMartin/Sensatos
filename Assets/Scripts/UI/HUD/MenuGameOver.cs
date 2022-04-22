using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameOver : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform menu;
    [SerializeField] private PainelDeEscolha opcoesMenuInicial;
    [SerializeField] private PainelDeEscolha opcoesConfirmacaoParaVoltarACidade;

    //Enums
    public enum Menu { Inicio, ConfirmacaoVoltarACidade }

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    private int selecao;
    private int selecao2;

    //Setters
    public void AtivarMenu()
    {
        ativo = true;

        generalManager.PauseManager.Pausar(true);
        generalManager.PauseManager.SetPermitirInput(false);
    }

    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch (this.menuAtual)
        {
            case Menu.Inicio:
                opcoesConfirmacaoParaVoltarACidade.gameObject.SetActive(false);
                break;

            case Menu.ConfirmacaoVoltarACidade:
                opcoesConfirmacaoParaVoltarACidade.gameObject.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        //Variaveis
        ativo = false;
        menuAtual = Menu.Inicio;
        selecao = 0;
        selecao2 = 0;

        menu.gameObject.SetActive(false);
        opcoesConfirmacaoParaVoltarACidade.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (ativo == false)
        {

            return;
        }

        //Executa as funcoes do menu atual
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;

            case Menu.ConfirmacaoVoltarACidade:
                ConfirmacaoVoltarACidade();
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
                    IniciarRespawnar();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 1:
                    SetMenuAtual(Menu.ConfirmacaoVoltarACidade);

                    selecao2 = 0;
                    AtualizarPainelDeEscolha(opcoesConfirmacaoParaVoltarACidade, selecao2);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);

                    break;
            }
        }
    }

    private void ConfirmacaoVoltarACidade()
    {
        //Mover para cima
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;

                AtualizarPainelDeEscolha(opcoesConfirmacaoParaVoltarACidade, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Direita())
        {
            if (selecao2 < opcoesConfirmacaoParaVoltarACidade.Opcoes.Length - 1)
            {
                selecao2++;

                AtualizarPainelDeEscolha(opcoesConfirmacaoParaVoltarACidade, selecao2);

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
                    IniciarVoltarParaACidade();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }

    public void IniciarMenuGameOver()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.GameOver);
        generalManager.PauseManager.SetPermitirInput(false);

        selecao = 0;
        selecao2 = 0;

        AtualizarPainelDeEscolha(opcoesMenuInicial, 0);

        animacao.Play("Iniciar");

        menu.gameObject.SetActive(true);
    }

    public void DesativarMenuGameOver()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Nenhum);

        ativo = false;
        animacao.Play("Vazio");
        menu.gameObject.SetActive(false);
    }

    private void IniciarRespawnar()
    {
        ativo = false;

        animacao.Play("Respawn");
    }

    public void Respawnar()
    {
        generalManager.RespawnManager.Respawn();

        generalManager.PauseManager.Pausar(false);
        generalManager.PauseManager.SetPermitirInput(true);
    }

    private void IniciarVoltarParaACidade()
    {
        ativo = false;

        animacao.Play("VoltarParaACidade");
    }

    public void VoltarParaACidade()
    {
        LevelLoaderScript.Instance.CarregarNivel("MenuPrincipal");
    }
}
