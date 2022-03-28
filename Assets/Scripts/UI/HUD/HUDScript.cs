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
    [SerializeField] private LockDownUI lockDownUI;
    [SerializeField] private PlayerUIScript playerUI;
    [SerializeField] private BarraDeVisaoDoInimigo barraDeVisaoDoInimigo;

    private SonsDeMenus sonsDeMenus;

    //Enuns
    public enum Menu { Nenhum, Pausa, Inventario }

    //Variaveis
    private Menu menuAberto;

    //Getters
    public MenuDoInventario MenuDoInventario => menuDoInventario;
    public MenuDePausa MenuDePausa => menuDePausa;
    public BarraDeVisaoDoInimigo BarraDeVisaoDoInimigo => barraDeVisaoDoInimigo;
    public SonsDeMenus SonsDeMenus => sonsDeMenus;
    public Menu MenuAberto => menuAberto;

    //Setters
    public void SetMenuAberto(Menu menuAberto)
    {
        this.menuAberto = menuAberto;
    }

    private void Start()
    {
        //Componentes
        sonsDeMenus = GetComponent<SonsDeMenus>();

        //Variaveis
        menuAberto = Menu.Nenhum;

        LockDownUIAtiva(false);
        BarraDeRecarregamentoAtiva(false);
    }

    public void AtualizarPlayerHUD()
    {
        playerHUD.AtualizarInformacoes();
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

    public void AtualizarBarraDeVisao(Enemy enemy, BarraDeVisaoDoInimigo barraDeVisaoDoInimigo, SpriteRenderer sprite)
    {
        barraDeVisaoDoInimigo.AtualizarPosicao(cameraAtiva, enemy, sprite);
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
