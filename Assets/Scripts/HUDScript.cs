using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    //Componentes
    private Canvas canvas;
    [SerializeField] private Camera cameraAtiva;
    [SerializeField] private PlayerUIScript playerUI;

    private void Start()
    {
        BarraDeRecarregamentoAtiva(false);
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
}
