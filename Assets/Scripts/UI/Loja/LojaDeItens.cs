using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LojaDeItens : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private ItemLojaSlot[] itens;

    [SerializeField] private TMP_Text nomeDoItem;
    [SerializeField] private TMP_Text descricaoDoItem;

    [SerializeField] private TMP_Text dinheiroJogador;

    [SerializeField] private RectTransform barraDeRolagem;

    [SerializeField] private RectTransform painelSemEspacoInventario;

    //Enums
    public enum Menu { Inicio, SemEspacoInventario }

    //Variaveis
    private int selecao;
    private int scrool;

    private Menu menuAtual;

    private float barraDeRolagemAlturaInicial;

    private string nomeSemItens = "";
    private string descricaoSemItens = "";

    [SerializeField] private string nomeSemItensPortugues;
    [SerializeField] private string descricaoSemItensPortugues;

    [SerializeField] private string nomeSemItensIngles;
    [SerializeField] private string descricaoSemItensIngles;


    private bool iniciado = false;

    private List<InventarioLoja.ItemLoja> listaDeItens = new List<InventarioLoja.ItemLoja>();

    //Variaveis Fixas
    private readonly int numeroDeColunas = 3;

    //Setters
    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch (this.menuAtual)
        {
            case Menu.Inicio:
                painelSemEspacoInventario.gameObject.SetActive(false);
                break;

            case Menu.SemEspacoInventario:
                painelSemEspacoInventario.gameObject.SetActive(true);
                break;
        }
    }

    void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Variaveis
        selecao = 0;
        scrool = 0;

        barraDeRolagemAlturaInicial = barraDeRolagem.sizeDelta.y;

        foreach (ItemLojaSlot itemSlot in itens)
        {
            itemSlot.Iniciar();
            itemSlot.ZerarInformacoes();
        }

        //Trocar o idioma uma vez para iniciar o objeto com o idioma correto
        TrocarIdioma();

        iniciado = true;
    }

    private void AtualizarInformacoesSemItens()
    {
        nomeDoItem.text = nomeSemItens;
        descricaoDoItem.text = descricaoSemItens;
    }

    private void AtualizarInformacoesDoItem()
    {
        nomeDoItem.text = listaDeItens[selecao].Item.Nome;
        descricaoDoItem.text = listaDeItens[selecao].Item.Descricao;

        dinheiroJogador.text = generalManager.Player.Inventario.Dinheiro.ToString();
    }

    private void AtualizarScroolDasArmas()
    {
        for (int i = 0; i < itens.Length; i++)
        {
            if (scrool + i >= listaDeItens.Count || scrool + i < 0)
            {
                itens[i].gameObject.SetActive(false);
            }
            else
            {
                itens[i].AtualizarInformacoes(listaDeItens[scrool + i].Item.ImagemInventario, listaDeItens[scrool + i].Preco);

                itens[i].gameObject.SetActive(true);
            }
        }

        foreach (ItemLojaSlot itemSlot in itens)
        {
            itemSlot.Selecionado(false);
        }

        itens[selecao - scrool].Selecionado(true);

        //Posicao da Barra de Rolagem
        if (itens.Length < listaDeItens.Count)
        {
            barraDeRolagem.anchoredPosition = new Vector2(0, (barraDeRolagemAlturaInicial / Mathf.Ceil((float)listaDeItens.Count / numeroDeColunas)) * (scrool / numeroDeColunas) * -1);
        }
        else
        {
            barraDeRolagem.anchoredPosition = Vector2.zero;
        }
    }

    public void IniciarScrool()
    {
        selecao = 0;
        scrool = 0;

        SetMenuAtual(Menu.Inicio);

        AtualizarListaDeItens();

        //Tamanho da Barra de Rolagem
        if (itens.Length < listaDeItens.Count)
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial * (Mathf.Ceil((float)itens.Length / numeroDeColunas) / Mathf.Ceil((float)listaDeItens.Count / numeroDeColunas)));
        }
        else
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial);
        }

        AtualizarScroolDasArmas();

        if (listaDeItens.Count > 0)
        {
            AtualizarInformacoesDoItem();
        }
        else
        {
            AtualizarInformacoesSemItens();
        }

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    private void AtualizarListaDeItens()
    {
        //Cria a lista com as armas e municoes disponiveis para comprar
        listaDeItens.Clear();
        listaDeItens = new List<InventarioLoja.ItemLoja>();

        if (generalManager.Hud.MenuDaLoja.InventarioLoja != null)
        {
            if (generalManager.Hud.MenuDaLoja.InventarioLoja.ListaDeItens.Count > 0)
            {
                foreach (InventarioLoja.ItemLoja item in generalManager.Hud.MenuDaLoja.InventarioLoja.ListaDeItens)
                {
                    if (Flags.GetFlag(item.Flag) == true)
                    {
                        listaDeItens.Add(item);
                    }
                }
            }
        }
    }

    private void ComprarItem()
    {
        if(generalManager.Player.Inventario.AdicionarItem(listaDeItens[selecao].Item) == true)
        {
            generalManager.Player.Inventario.SetDinheiro(generalManager.Player.Inventario.Dinheiro - (listaDeItens[selecao].Preco));

            AtualizarScroolDasArmas();
            AtualizarInformacoesDoItem();

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Comprar);
        }
        else
        {
            SetMenuAtual(Menu.SemEspacoInventario);

            IniciarPainelSemEspacoInventario();

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
        }
    }

    private void IniciarPainelSemEspacoInventario()
    {
        painelSemEspacoInventario.transform.position = itens[selecao - scrool].transform.position;
    }

    public void MenuLojaDeItens()
    {
        //Executa as funcoes do menu atual
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;

            case Menu.SemEspacoInventario:
                PainelSemEspacoInventario();
                break;
        }
    }

    private void MenuInicial()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao - numeroDeColunas >= 0)
            {
                selecao -= numeroDeColunas;

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDoItem();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
            else if (selecao > 0)
            {
                selecao = 0;

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDoItem();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao + numeroDeColunas < listaDeItens.Count)
            {
                selecao += numeroDeColunas;

                if (selecao - scrool > itens.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDoItem();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
            else if (selecao < listaDeItens.Count - 1)
            {
                selecao = listaDeItens.Count - 1;

                if (selecao - scrool > itens.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDoItem();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecao > 0)
            {
                selecao--;

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDoItem();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecao < listaDeItens.Count - 1)
            {
                selecao++;


                if (selecao - scrool > itens.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDoItem();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            generalManager.Hud.MenuDaLoja.SetMenuAtual(MenuDaLoja.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            if (generalManager.Player.Inventario.Dinheiro >= listaDeItens[selecao].Preco)
            {
                ComprarItem();
            }
            else
            {
                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
            }
        }
    }

    private void PainelSemEspacoInventario()
    {
        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }

    private void TrocarIdioma()
    {
        switch (IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                nomeSemItens = nomeSemItensPortugues;
                descricaoSemItens = descricaoSemItensPortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                nomeSemItens = nomeSemItensIngles;
                descricaoSemItens = descricaoSemItensIngles;
                break;
        }
    }
}
