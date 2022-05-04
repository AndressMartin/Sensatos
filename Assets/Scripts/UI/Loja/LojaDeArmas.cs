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
    [SerializeField] private InformacoesDaMelhoria[] melhorias;

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

    private int quantidadeMunicao;

    private Menu menuAtual;

    private float barraDeRolagemAlturaInicial;

    private bool iniciado = false;

    private List<InventarioLoja.ArmaLoja> listaDeArmas = new List<InventarioLoja.ArmaLoja>();

    //Variaveis Fixas
    private readonly int numeroDeColunas = 2;

    //Variaveis de controle
    private float intervaloInput = 0;
    private float intervaloInputMax = 0.05f;

    private bool apertouOsBotoes = false;

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
        quantidadeMunicao = 0;

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

        foreach (InformacoesDaMelhoria melhoria in melhorias)
        {
            melhoria.gameObject.SetActive(false);
        }

        for (int i = 0; i < listaDeArmas[selecao].Arma.Melhorias.Count; i++)
        {
            melhorias[i].gameObject.SetActive(true);
            melhorias[i].AtualizarInformacoes(listaDeArmas[selecao].Arma.Melhorias[i]);

            melhorias[i].MelhoriaAdquirida(true);
        }

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
        if (armas.Length < listaDeArmas.Count)
        {
            barraDeRolagem.anchoredPosition = new Vector2(0, (barraDeRolagemAlturaInicial / Mathf.Ceil((float)listaDeArmas.Count / numeroDeColunas)) * (scrool / numeroDeColunas) * -1);
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
        if (armas.Length < listaDeArmas.Count)
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial * (Mathf.Ceil((float)armas.Length / numeroDeColunas) / Mathf.Ceil((float)listaDeArmas.Count / numeroDeColunas)));
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

    private void IniciarQuantidadeMunicao()
    {
        Rect retanguloGlobal = Colisao.GetWorldRect(armas[selecao - scrool].GetComponent<RectTransform>());

        quantidadeMunicao = MaximoDeMunicaoAdicionavel();
        painelEscolhaQuantidadeMunicao.transform.position = new Vector2(armas[selecao - scrool].transform.position.x, armas[selecao - scrool].transform.position.y - retanguloGlobal.size.y / 2);
    }

    private void AtualizarPainelQuantidadeMunicao()
    {
        quantidadeMunicaoTexto.text = quantidadeMunicao.ToString();
        precoMunicaoTexto.text = (listaDeArmas[selecao].PrecoMunicao * quantidadeMunicao).ToString();
    } 

    private int MaximoDeMunicaoAdicionavel()
    {
        int valor = 0;

        valor += generalManager.Player.Inventario.GetArma(listaDeArmas[selecao].Arma).GetStatus.MunicaoMax - generalManager.Player.Inventario.GetArma(listaDeArmas[selecao].Arma).Municao;
        valor += generalManager.Player.Inventario.GetArma(listaDeArmas[selecao].Arma).GetStatus.MunicaoMaxCartucho - generalManager.Player.Inventario.GetArma(listaDeArmas[selecao].Arma).MunicaoCartucho;

        return valor;
    }

    private void ComprarArma()
    {
        generalManager.Player.Inventario.AdicionarArma(listaDeArmas[selecao].Arma);
        generalManager.Player.Inventario.SetDinheiro(generalManager.Player.Inventario.Dinheiro - listaDeArmas[selecao].PrecoArma);

        AtualizarListaDeArmas();

        if(selecao > listaDeArmas.Count - 1)
        {
            selecao = listaDeArmas.Count - 1;

            if (selecao - scrool > armas.Length - 1)
            {
                scrool += numeroDeColunas;
            }
        }

        AtualizarScroolDasArmas();
        AtualizarInformacoesDaArma();
    }

    private void ComprarMunicao()
    {
        generalManager.Player.Inventario.GetArma(listaDeArmas[selecao].Arma).AdicionarMunicao(quantidadeMunicao);
        generalManager.Player.Inventario.SetDinheiro(generalManager.Player.Inventario.Dinheiro - (listaDeArmas[selecao].PrecoMunicao * quantidadeMunicao));

        AtualizarScroolDasArmas();
        AtualizarInformacoesDaArma();
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

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

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
                AtualizarInformacoesDaArma();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao + numeroDeColunas < listaDeArmas.Count)
            {
                selecao += numeroDeColunas;

                if (selecao - scrool > armas.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
            else if (selecao < listaDeArmas.Count - 1)
            {
                selecao = listaDeArmas.Count - 1;

                if (selecao - scrool > armas.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

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
                AtualizarInformacoesDaArma();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecao < listaDeArmas.Count - 1)
            {
                selecao++;


                if (selecao - scrool > armas.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

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
            switch (listaDeArmas[selecao].Tipo)
            {
                case InventarioLoja.ArmaLoja.TipoCompra.Arma:
                    if(generalManager.Player.Inventario.Dinheiro >= listaDeArmas[selecao].PrecoArma)
                    {
                        ComprarArma();

                        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Comprar);
                    }
                    else
                    {
                        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
                    }
                    break;

                case InventarioLoja.ArmaLoja.TipoCompra.Municao:
                    if(MaximoDeMunicaoAdicionavel() > 0)
                    {
                        SetMenuAtual(Menu.EscolhendoQuantidadeMunicao);

                        IniciarQuantidadeMunicao();
                        AtualizarPainelQuantidadeMunicao();

                        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    }
                    else
                    {
                        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
                    }
                    break;
            }
        }
    }

    private void EscolhendoQuantidadeMunicao()
    {
        if (InputManager.EsquerdaSegurar() == false && InputManager.DireitaSegurar() == false)
        {
            apertouOsBotoes = false;
        }

        if (intervaloInput > 0)
        {
            //Deve-se usar o unscaledDeltaTime, pois neste menu o jogo estara pausado, e o timeScale sera 0
            intervaloInput -= Time.unscaledDeltaTime;
        }

        //Diminuir Municao
        if (InputManager.EsquerdaSegurar() == true && intervaloInput <= 0)
        {
            if(quantidadeMunicao > 1)
            {
                quantidadeMunicao--;
                AtualizarPainelQuantidadeMunicao();

                intervaloInput = intervaloInputMax;

                if (apertouOsBotoes == false)
                {
                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
                    apertouOsBotoes = true;
                }
            }
        }

        //Aumentar Municao
        if (InputManager.DireitaSegurar() == true && intervaloInput <= 0)
        {
            if(quantidadeMunicao < MaximoDeMunicaoAdicionavel())
            {
                quantidadeMunicao++;
                AtualizarPainelQuantidadeMunicao();

                intervaloInput = intervaloInputMax;

                if (apertouOsBotoes == false)
                {
                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
                    apertouOsBotoes = true;
                }
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
            if (generalManager.Player.Inventario.Dinheiro >= (listaDeArmas[selecao].PrecoMunicao * quantidadeMunicao))
            {
                ComprarMunicao();

                SetMenuAtual(Menu.Inicio);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Comprar);
            }
            else
            {
                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
            }
        }
    }
}
