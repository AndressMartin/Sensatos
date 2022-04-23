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

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    //Setters
    public void AtivarMenu()
    {
        ativo = true;

        generalManager.PauseManager.Pausar(true);
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
        generalManager.PauseManager.SetPermitirInput(false);

        AtualizarPainelDeEscolha(opcoesMenuInicial, 0);

        animacao.Play("Iniciar");

        menu.gameObject.SetActive(true);
    }

    private void IniciarVoltarParaACidade()
    {
        ativo = false;

        animacao.Play("VoltarParaACidade");
    }

    public void VoltarParaACidade()
    {
        LevelLoaderScript.Instance.CarregarNivel("Mapa_Teste_2");
    }
}
