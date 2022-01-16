using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
    //Componentes
    [SerializeField] private RectTransform barraDeRecarregamentoObjeto;
    [SerializeField] private Image barraDeRegarregamento;

    public void BarraDeRecarregamentoAtiva(bool ativa)
    {
        barraDeRecarregamentoObjeto.gameObject.SetActive(ativa);
    }

    public void AtualizarBarraDeRecarregamento(float tempoRecarregar, float tempoRecarregarMax)
    {
        barraDeRegarregamento.fillAmount = tempoRecarregar / tempoRecarregarMax;
    }

    public void AtualizarPosicaoDaBarraDeRecarregamento(Camera camera, Player player)
    {
        float diferencaX = 0.9f;
        float diferencaY = 1.5f;
        Vector2 diferenca = new Vector2(diferencaX, diferencaY);

        barraDeRecarregamentoObjeto.position = (Vector2)player.transform.position + diferenca;
    }
}
