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
    [SerializeField] private LockDownUI lockDownUI;
    [SerializeField] private PlayerUIScript playerUI;
    [SerializeField] private BarraDeVisaoDoInimigo barraDeVisaoDoInimigo;

    private SonsDeMenus sonsDeMenus;

    //Getters
    public MenuDoInventario MenuDoInventario => menuDoInventario;
    public BarraDeVisaoDoInimigo BarraDeVisaoDoInimigo => barraDeVisaoDoInimigo;
    public SonsDeMenus SonsDeMenus => sonsDeMenus;

    private void Start()
    {
        //Componentes
        sonsDeMenus = GetComponent<SonsDeMenus>();

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

    public void AtualizarTempoLockDown(float tempo)
    {
        int tempoTemp = (int)(tempo * 100.0f);
        char[] tempoString = tempoTemp.ToString().ToCharArray();

        string textoFinal = "";

        for(int i = 0; i < tempoString.Length; i++)
        {
            if(tempoString.Length < 4 && i == 0)
            {
                textoFinal += "0";
            }

            if (tempoString.Length < 3 && i == 0)
            {
                textoFinal += "0";
            }

            if (i == 0 && tempoString.Length <= 2)
            {
                textoFinal += ":";
            }

            if(i == 0 && tempoString.Length <= 1)
            {
                textoFinal += "0";
            }

            textoFinal += tempoString[i];

            if(i == 1 && tempoString.Length >= 4)
            {
                textoFinal += ":";
            }

            if (i == 0 && tempoString.Length == 3)
            {
                textoFinal += ":";
            }
        }

        lockDownUI.AtualizarTempo(textoFinal);
    }

    public void AbrirOInventario()
    {
        menuDoInventario.AbrirOInventario();
    }
}
