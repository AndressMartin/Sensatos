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
    [SerializeField] private LojaDeItens lojaDeItens;

    [SerializeField] private PainelDeEscolha opcoesMenuInicial;

    private MudarIdiomaItensDoInventario mudarIdiomaItensDoInventario;

    //Enums
    public enum Menu { Inicio, Armas, Melhorias, Itens }

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
                lojaDeItens.gameObject.SetActive(false);
                break;

            case Menu.Armas:
                telaInicial.gameObject.SetActive(false);
                lojaDeArmas.gameObject.SetActive(true);
                lojaDeMelhorias.gameObject.SetActive(false);
                lojaDeItens.gameObject.SetActive(false);
                break;

            case Menu.Melhorias:
                telaInicial.gameObject.SetActive(false);
                lojaDeArmas.gameObject.SetActive(false);
                lojaDeMelhorias.gameObject.SetActive(true);
                lojaDeItens.gameObject.SetActive(false);
                break;

            case Menu.Itens:
                telaInicial.gameObject.SetActive(false);
                lojaDeArmas.gameObject.SetActive(false);
                lojaDeMelhorias.gameObject.SetActive(false);
                lojaDeItens.gameObject.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Variaveis
        ativo = false;
        menuAtual = Menu.Inicio;
        selecao = 0;

        //Componentes
        mudarIdiomaItensDoInventario = GetComponent<MudarIdiomaItensDoInventario>();

        IniciarComponentes();

        FecharOsMenus();

        //Trocar o idioma uma vez para iniciar os objetos com o idioma correto
        TrocarIdioma();
    }

    private void IniciarComponentes()
    {
        lojaDeArmas.Iniciar();
        lojaDeMelhorias.Iniciar();
        lojaDeItens.Iniciar();
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

            case Menu.Itens:
                MenuItens();
                break;
        }
    }

    public void AbrirOMenuDaLoja(InventarioLoja inventarioLoja)
    {
        //Seta o inventario da loja como o recebido
        this.inventarioLoja = inventarioLoja;
        
        //Atualiza o idioma dos itens da loja
        TrocarIdioma();

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
        lojaDeItens.gameObject.SetActive(false);
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
                    SetMenuAtual(Menu.Itens);

                    lojaDeItens.IniciarScrool();

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

    private void MenuItens()
    {
        lojaDeItens.MenuLojaDeItens();
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }

    private void TrocarIdioma()
    {
        if(inventarioLoja == null)
        {
            return;
        }

        foreach (InventarioLoja.ArmaLoja armaLoja in inventarioLoja.ListaDeArmas)
        {
            mudarIdiomaItensDoInventario.TrocarIdioma(armaLoja.Arma);
        }

        foreach (InventarioLoja.ItemLoja itemLoja in inventarioLoja.ListaDeItens)
        {
            mudarIdiomaItensDoInventario.TrocarIdioma(itemLoja.Item);
        }
    }
}
