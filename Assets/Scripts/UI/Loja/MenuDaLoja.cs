using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDaLoja : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private RectTransform telaInicial;
    [SerializeField] private LojaDeArmas lojaDeArmas;
    [SerializeField] private LojaDeMelhorias lojaDeMelhorias;
    [SerializeField] private LojaDeFerramentas lojaDeFerramentas;

    [SerializeField] private PainelDeEscolha opcoesMenuInicial;

    //Enums
    public enum Menu { Inicio, Armas, Melhorias, Ferramentas }

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    private int selecao;

    private InventarioLoja inventarioLoja;

    //Getters
    public InventarioLoja InventarioLoja => inventarioLoja;

    //Setters
    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch (this.menuAtual)
        {
            case Menu.Inicio:
                telaInicial.gameObject.SetActive(true);
                lojaDeArmas.gameObject.SetActive(false);
                lojaDeMelhorias.gameObject.SetActive(false);
                lojaDeFerramentas.gameObject.SetActive(false);
                break;

            case Menu.Armas:
                telaInicial.gameObject.SetActive(false);
                lojaDeArmas.gameObject.SetActive(true);
                lojaDeMelhorias.gameObject.SetActive(false);
                lojaDeFerramentas.gameObject.SetActive(false);
                break;

            case Menu.Melhorias:
                telaInicial.gameObject.SetActive(false);
                lojaDeArmas.gameObject.SetActive(false);
                lojaDeMelhorias.gameObject.SetActive(true);
                lojaDeFerramentas.gameObject.SetActive(false);
                break;

            case Menu.Ferramentas:
                telaInicial.gameObject.SetActive(false);
                lojaDeArmas.gameObject.SetActive(false);
                lojaDeMelhorias.gameObject.SetActive(false);
                lojaDeFerramentas.gameObject.SetActive(true);
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

        IniciarComponentes();

        FecharOsMenus();
    }

    private void IniciarComponentes()
    {
        lojaDeArmas.Iniciar();
        lojaDeMelhorias.Iniciar();
        lojaDeFerramentas.Iniciar();
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

            case Menu.Armas:
                MenuArmas();
                break;

            case Menu.Melhorias:
                MenuMelhorias();
                break;

            case Menu.Ferramentas:
                MenuFerramentas();
                break;
        }
    }

    public void AbrirOMenuDaLoja(InventarioLoja inventarioLoja)
    {
        //Seta o inventario da loja como o recebido
        this.inventarioLoja = inventarioLoja;

        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Loja);
        generalManager.PauseManager.Pausar(true);
        generalManager.PauseManager.SetPermitirInput(false);

        ativo = true;

        SetMenuAtual(Menu.Inicio);

        selecao = 0;
        AtualizarPainelDeEscolha(opcoesMenuInicial, selecao);
    }

    private void FecharOMenuDaLoja()
    {
        //Zera o inventario da loja
        this.inventarioLoja = null;

        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Nenhum);
        generalManager.PauseManager.Pausar(false);
        generalManager.PauseManager.SetPermitirInput(true);

        FecharOsMenus();

        ativo = false;
    }

    private void FecharOsMenus()
    {
        telaInicial.gameObject.SetActive(false);
        lojaDeArmas.gameObject.SetActive(false);
        lojaDeMelhorias.gameObject.SetActive(false);
        lojaDeFerramentas.gameObject.SetActive(false);
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
            FecharOMenuDaLoja();
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            switch (selecao)
            {
                case 0:
                    SetMenuAtual(Menu.Armas);

                    lojaDeArmas.IniciarScrool();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);

                    break;

                case 1:
                    SetMenuAtual(Menu.Melhorias);

                    //menuSalvar.IniciarScrool();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);

                    break;

                case 2:
                    SetMenuAtual(Menu.Ferramentas);

                    //menuOpcoes.IniciarScrool();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;

                case 3:
                    FecharOMenuDaLoja();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
                    break;
            }
        }
    }

    private void MenuArmas()
    {
        lojaDeArmas.MenuLojaDeArmas();
    }

    private void MenuMelhorias()
    {
        //menuOpcoes.SelecionandoOpcoes();
    }

    private void MenuFerramentas()
    {
        //Algo
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }
}
