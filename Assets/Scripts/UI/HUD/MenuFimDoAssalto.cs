using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuFimDoAssalto : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform menu;
    [SerializeField] private PainelDeEscolha opcoesMenuInicial;

    [SerializeField] private TMP_Text dinheiroObjetivoPrincipal;
    [SerializeField] private TMP_Text dinheiroSaqueExtra;

    //Enums
    public enum Menu { Inicio, ConfirmacaoVoltarACidade }
    public enum Som { TerminouAssalto, MadeiraCaiuNoChao }

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    [SerializeField] private AudioClip somTerminouAssalto;
    [SerializeField] private AudioClip somMadeiraCaiuNoChao;

    //Setters
    public void AtivarMenu()
    {
        ativo = true;

        generalManager.PauseManager.SetPermitirInput(false);
    }

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        //Variaveis
        ativo = false;

        menu.gameObject.SetActive(false);
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
        }
    }

    private void MenuInicial()
    {
        //Confirmar
        if (InputManager.Confirmar())
        {
            IniciarVoltarParaACidade();

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }

    public void AtualizarDinheiro(int dinheiroObjetivoPrincipal, int dinheiroSaqueExtra)
    {
        this.dinheiroObjetivoPrincipal.text = dinheiroObjetivoPrincipal.ToString();
        this.dinheiroSaqueExtra.text = dinheiroSaqueExtra.ToString();
    }

    public void IniciarMenuFimDoAssalto()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.FimDoAssalto);

        generalManager.PauseManager.Pausar(true);
        generalManager.PauseManager.SetPermitirInput(false);

        menu.gameObject.SetActive(true);

        AtualizarPainelDeEscolha(opcoesMenuInicial, 0);

        animacao.Play("Iniciar");

        TocarSom(Som.TerminouAssalto);
    }

    private void IniciarVoltarParaACidade()
    {
        ativo = false;

        animacao.Play("VoltarParaACidade");
    }

    public void VoltarParaACidade()
    {
        GameManager.instance.VariaveisGlobais.CompletouUmAssalto = true;

        LevelLoaderScript.Instance.CarregarNivel(GameManager.instance.NomesDeCenas.Cidade);
    }

    public void TocarSom(Som som)
    {
        switch(som)
        {
            case Som.TerminouAssalto:
                generalManager.SoundManager.TocarSomIgnorandoPause(somTerminouAssalto);
                break;

            case Som.MadeiraCaiuNoChao:
                generalManager.SoundManager.TocarSomIgnorandoPause(somMadeiraCaiuNoChao);
                break;
        }
    }
}
