using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    //Componentes
    private Canvas canvas;
    [SerializeField] private Camera cameraAtiva;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private MenuDoInventario menuDoInventario;
    [SerializeField] private MenuDePausa menuDePausa;
    [SerializeField] private MenuGameOver menuGameOver;
    [SerializeField] private MenuFimDoAssalto menuFimDoAssalto;
    [SerializeField] private MenuEscolherAssalto menuEscolherAssalto;
    [SerializeField] private TelaItemPrincipalPego telaItemPrincipalPego;
    [SerializeField] private TelaMissaoConcluida telaMissaoConcluida;
    [SerializeField] private MenuDaLoja menuDaLoja;
    [SerializeField] private EfeitosDeTela efeitosDeTela;
    [SerializeField] private TelaTransicaoDeMapa telaTransicaoDeMapa;
    [SerializeField] private TransicaoDeTela transicaoDeTela;
    [SerializeField] private TelaFimDoJogo telaFimDoJogo;
    [SerializeField] private LockDownUI lockDownUI;
    [SerializeField] private PlayerUIScript playerUI;
    [SerializeField] private BarraDeVisaoDoInimigo barraDeVisaoDoInimigo;

    [SerializeField] private SonsDeMenus sonsDeMenus;

    //Enuns
    public enum Menu { Nenhum, Pausa, Inventario, GameOver, FimDoAssalto, EscolherAssalto, TelaItemPrincipalPego, Loja, Transicao, TelaMissaoConcluida }

    //Variaveis
    private Menu menuAberto;

    //Getters
    public MenuDoInventario MenuDoInventario => menuDoInventario;
    public MenuDePausa MenuDePausa => menuDePausa;
    public MenuGameOver MenuGameOver => menuGameOver;
    public MenuFimDoAssalto MenuFimDoAssalto => menuFimDoAssalto;
    public MenuEscolherAssalto MenuEscolherAssalto => menuEscolherAssalto;
    public TelaItemPrincipalPego TelaItemPrincipalPego => telaItemPrincipalPego;
    public TelaMissaoConcluida TelaMissaoConcluida => telaMissaoConcluida;
    public MenuDaLoja MenuDaLoja => menuDaLoja;
    public EfeitosDeTela EfeitosDeTela => efeitosDeTela;
    public TelaTransicaoDeMapa TelaTransicaoDeMapa => telaTransicaoDeMapa;
    public TransicaoDeTela TransicaoDeTela => transicaoDeTela;
    public TelaFimDoJogo TelaFimDoJogo => telaFimDoJogo;
    public BarraDeVisaoDoInimigo BarraDeVisaoDoInimigo => barraDeVisaoDoInimigo;
    public SonsDeMenus SonsDeMenus => sonsDeMenus;
    public Menu MenuAberto => menuAberto;

    //Setters
    public void SetMenuAberto(Menu menuAberto)
    {
        this.menuAberto = menuAberto;

        if(menuAberto == Menu.Transicao)
        {
            return;
        }

        AtualizarPlayerHUD();
    }

    private void Awake()
    {
        menuAberto = Menu.Nenhum;
    }

    private void Start()
    {
        LockDownUIAtiva(false);
        BarraDeRecarregamentoAtiva(false);
    }

    public void AtualizarPlayerHUD()
    {
        playerHUD?.AtualizarInformacoes();
    }

    public void TremerBarraDeVida()
    {
        playerHUD?.TremerBarraDeVida();
    }

    public void BarraDeRecarregamentoAtiva(bool ativa)
    {
        playerUI.BarraDeRecarregamentoAtiva(ativa);
    }

    public void AtualizarBarraDeRecarregamento(float tempoRecarregar, float tempoRecarregarMax)
    {
        playerUI.AtualizarBarraDeRecarregamento(tempoRecarregar, tempoRecarregarMax);
    }

    public void AtualizarPosicaoDaBarraDeRecarregamento(Player player)
    {
        playerUI.AtualizarPosicaoDaBarraDeRecarregamento(cameraAtiva, player);
    }

    public void AtualizarBarraDeVisao(GameObject objeto, BarraDeVisaoDoInimigo barraDeVisaoDoInimigo, SpriteRenderer sprite, float diferencaY)
    {
        barraDeVisaoDoInimigo.AtualizarPosicao(cameraAtiva, objeto, sprite, diferencaY);
    }

    public void LockDownUIAtiva(bool ativa)
    {
        lockDownUI.gameObject.SetActive(ativa);
    }

    public void AtualizarTempoLockDown(float tempoEmSegundos)
    {
        TimeSpan tempo = TimeSpan.FromSeconds((double)tempoEmSegundos);

        //Exemplo: string tempoString = tempo.ToString(@"hh\:mm\:ss\:fff");

        //As barras antes dos 2 pontos serve para indicar que eles sao um caractere a ser incluido no resultado da string, e nao uma parte da formatacao do texto
        string tempoString = tempo.ToString(@"ss\:ff");

        lockDownUI.AtualizarTempo(tempoString);
    }

    public void AbrirOInventario()
    {
        menuDoInventario.AbrirOInventario();
    }
}
