using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LojaDeArmas : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private ArmaLojaSlot[] armas;

    [SerializeField] private TMP_Text nomeDaArma;
    [SerializeField] private TMP_Text descricaoDaArma;

    [SerializeField] private TMP_Text dinheiroJogador;

    [SerializeField] private RectTransform barraDeRolagem;

    [SerializeField] private RectTransform painelEscolhaQuantidadeMunicao;
    [SerializeField] private TMP_Text quantidadeMunicaoTexto;
    [SerializeField] private TMP_Text precoMunicaoTexto;

    //Enums
    public enum Menu { Inicio, ConfirmarComprarArma, EscolhendoQuantidadeMunicao }

    //Variaveis
    private int selecao;
    private int scrool;

    private Menu menuAtual;

    private float barraDeRolagemAlturaInicial;

    private bool iniciado = false;

    private List<InventarioLoja.ArmaLoja> listaDeArmas = new List<InventarioLoja.ArmaLoja>();

    //Variaveis Fixas
    private readonly int numeroDeColunas = 2;

    //Setters
    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch (this.menuAtual)
        {
            case Menu.Inicio:
                painelEscolhaQuantidadeMunicao.gameObject.SetActive(false);
                break;

            case Menu.EscolhendoQuantidadeMunicao:
                painelEscolhaQuantidadeMunicao.gameObject.SetActive(true);
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

        //Variaveis
        selecao = 0;
        scrool = 0;

        barraDeRolagemAlturaInicial = barraDeRolagem.sizeDelta.y;

        foreach (ArmaLojaSlot armaSlot in armas)
        {
            armaSlot.Iniciar();
            armaSlot.ZerarInformacoes();
        }

        iniciado = true;
    }

    private void AtualizarInformacoesDaArma()
    {
        nomeDaArma.text = listaDeArmas[selecao].Arma.Nome;
        descricaoDaArma.text = listaDeArmas[selecao].Arma.Descricao;

        dinheiroJogador.text = generalManager.Player.Inventario.Dinheiro.ToString();
    }

    private void AtualizarScroolDasArmas()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            if (scrool + i >= listaDeArmas.Count || scrool + i < 0)
            {
                armas[i].gameObject.SetActive(false);
            }
            else
            {
                if (listaDeArmas[scrool + i].Tipo == InventarioLoja.ArmaLoja.TipoCompra.Arma)
                {
                    armas[i].AtualizarInformacoes(listaDeArmas[scrool + i].Arma, listaDeArmas[scrool + i].PrecoArma);
                }
                else
                {
                    armas[i].AtualizarInformacoes(listaDeArmas[scrool + i].Arma, listaDeArmas[scrool + i].PrecoMunicao);
                }

                armas[i].ImagemMunicaoAtiva(listaDeArmas[scrool + i].Tipo == InventarioLoja.ArmaLoja.TipoCompra.Municao);

                armas[i].gameObject.SetActive(true);
            }
        }

        foreach (ArmaLojaSlot armaSlot in armas)
        {
            armaSlot.Selecionado(false);
        }

        armas[selecao - scrool].Selecionado(true);

        //Posicao da Barra de Rolagem
        if ((armas.Length / numeroDeColunas) < (listaDeArmas.Count / numeroDeColunas))
        {
            barraDeRolagem.anchoredPosition = new Vector2(0, (barraDeRolagemAlturaInicial / (listaDeArmas.Count / numeroDeColunas)) * scrool * -1);
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

        AtualizarListaDeArmas();

        //Tamanho da Barra de Rolagem
        if ((armas.Length / numeroDeColunas) < (listaDeArmas.Count / numeroDeColunas))
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial * ((float)(armas.Length / numeroDeColunas) / (listaDeArmas.Count / numeroDeColunas)));
        }
        else
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial);
        }

        AtualizarScroolDasArmas();

        AtualizarInformacoesDaArma();

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    private void AtualizarListaDeArmas()
    {
        //Cria a lista com as armas e municoes disponiveis para comprar
        listaDeArmas.Clear();
        listaDeArmas = new List<InventarioLoja.ArmaLoja>();

        if (generalManager.Hud.MenuDaLoja.InventarioLoja != null)
        {
            if (generalManager.Hud.MenuDaLoja.InventarioLoja.ListaDeArmas.Count > 0)
            {
                foreach (InventarioLoja.ArmaLoja arma in generalManager.Hud.MenuDaLoja.InventarioLoja.ListaDeArmas)
                {
                    if (generalManager.Player.Inventario.PossuiArma(arma.Arma) == true)
                    {
                        arma.SetTipo(InventarioLoja.ArmaLoja.TipoCompra.Municao);
                        listaDeArmas.Add(arma);
                    }
                    else
                    {
                        if (Flags.GetFlag(arma.Flag) == true)
                        {
                            arma.SetTipo(InventarioLoja.ArmaLoja.TipoCompra.Arma);
                            listaDeArmas.Add(arma);
                        }
                    }
                }
            }
        }
    }

    public void MenuLojaDeArmas()
    {
        //Executa as funcoes do menu atual
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;

            case Menu.EscolhendoQuantidadeMunicao:
                EscolhendoQuantidadeMunicao();
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

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao + numeroDeColunas < listaDeArmas.Count)
            {
                selecao += numeroDeColunas;

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

                if (selecao - scrool > armas.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecao > 0)
            {
                selecao--;

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecao < listaDeArmas.Count - 1)
            {
                selecao++;

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

                if (selecao - scrool > armas.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            generalManager.Hud.MenuDaLoja.SetMenuAtual(MenuDaLoja.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }

    private void EscolhendoQuantidadeMunicao()
    {
        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }
}
