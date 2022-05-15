using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVisaoDoInimigo : MonoBehaviour
{
    //Componentes
    [SerializeField] private RectTransform barraDeVisaoObjeto;
    [SerializeField] private Image barraDeVisao;
    [SerializeField] private RectTransform alertaObjeto;

    //Variaveis
    private bool iconeAtivo = false;
    private float tempoAlertaAtivo;
    private float tempoAlertaAtivoMax = 2f;

    //Getters
    public bool IconeAtivo => iconeAtivo;

    private void AtualizarIconeAtivo()
    {
        if(barraDeVisaoObjeto.gameObject.activeSelf == true || alertaObjeto.gameObject.activeSelf == true)
        {
            iconeAtivo = true;
        }
        else
        {
            iconeAtivo = false;
        }
    }

    public void BarraDeVisaoAtiva(bool ativa)
    {
        barraDeVisaoObjeto.gameObject.SetActive(ativa);
        AtualizarIconeAtivo();
    }

    public void IconeDeAlertaAtivo(bool ativo)
    {
        alertaObjeto.gameObject.SetActive(ativo);
        AtualizarIconeAtivo();
    }

    public void AtivarIconeDeAlerta()
    {
        BarraDeVisaoAtiva(false);
        IconeDeAlertaAtivo(true);
        StartCoroutine(AlertaAtivo());
    }

    public void AtualizarBarraDeVisao(float tempoVisao, float tempoVisaoMax)
    {
        barraDeVisao.fillAmount = tempoVisao / tempoVisaoMax;
    }

    public void AtualizarPosicao(Camera camera, GameObject objeto, SpriteRenderer sprite)
    {
        float diferencaY = 0.2f;
        Vector2 diferenca = new Vector2(0, sprite.bounds.extents.y + diferencaY);

        transform.position = (Vector2)objeto.transform.position + diferenca;
    }

    private IEnumerator AlertaAtivo()
    {
        tempoAlertaAtivo = tempoAlertaAtivoMax;

        while(tempoAlertaAtivo > 0)
        {
            tempoAlertaAtivo -= Time.deltaTime;

            yield return null;
        }

        IconeDeAlertaAtivo(false);
    }
}
