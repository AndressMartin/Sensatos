using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private Image barraDeVida;
    [SerializeField] private TMP_Text vidaTexto;

    [SerializeField] private InformacoesDaArmaHUD[] armas;
    [SerializeField] private InformacoesDoItem[] atalhos;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        StartCoroutine(AtualizarInformacoesInicio());
    }

    public void AtualizarInformacoes()
    {
        AtualizarBarraDeVida();
        AtualizarArmas();
        AtualizarAtalhos();
    }

    private void AtualizarBarraDeVida()
    {
        barraDeVida.fillAmount = (float)generalManager.Player.Vida / (float)generalManager.Player.VidaMax;
        vidaTexto.text = generalManager.Player.Vida.ToString() + "/" + generalManager.Player.VidaMax.ToString();
    }

    private void AtualizarArmas()
    {
        int indiceOutraArma;

        if(generalManager.Player.Inventario.ArmaAtual == 0)
        {
            indiceOutraArma = 1;
        }
        else
        {
            indiceOutraArma = 0;
        }

        if (generalManager.Player.Inventario.ArmaSlot[generalManager.Player.Inventario.ArmaAtual] != null)
        {
            armas[0].AtualizarInformacoes(generalManager.Player.Inventario.ArmaSlot[generalManager.Player.Inventario.ArmaAtual]);
        }
        else
        {
            armas[0].ZerarInformacoes();
        }

        if (generalManager.Player.Inventario.ArmaSlot[indiceOutraArma] != null)
        {
            armas[1].AtualizarInformacoes(generalManager.Player.Inventario.ArmaSlot[indiceOutraArma]);
        }
        else
        {
            armas[1].ZerarInformacoes();
        }
    }

    private void AtualizarAtalhos()
    {
        for (int i = 0; i < atalhos.Length; i++)
        {
            atalhos[i].AtualizarNumeroAtalho(i + 1);

            if (generalManager.Player.Inventario.AtalhosDeItens[i].ID != Listas.instance.ListaDeItens.GetID["ItemVazio"])
            {
                atalhos[i].AtualizarInformacoes(generalManager.Player.Inventario.AtalhosDeItens[i]);
            }
            else
            {
                atalhos[i].ZerarInformacoes();
            }
        }
    }

    private IEnumerator AtualizarInformacoesInicio()
    {
        yield return null;

        AtualizarInformacoes();
    }
}
