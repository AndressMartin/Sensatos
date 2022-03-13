using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformacoesDoItem : MonoBehaviour
{
    //Componentes
    [SerializeField] private Image imagem;
    [SerializeField] private RectTransform barraDeUsoObjeto;
    [SerializeField] private Image barraDeUso;
    [SerializeField] private RectTransform numeroAtalhoObjeto;
    [SerializeField] private TMP_Text numeroAtalho;

    public void ZerarInformacoes()
    {
        imagem.sprite = null;
        barraDeUsoObjeto.gameObject.SetActive(false);
    }

    public void AtualizarInformacoes(Item item)
    {
        imagem.sprite = item.ImagemInventario;
        barraDeUsoObjeto.gameObject.SetActive(false);

        if(item is Alicate)
        {
            Alicate alicate = (Alicate)item;

            barraDeUsoObjeto.gameObject.SetActive(true);

            barraDeUso.fillAmount = (float)alicate.QuantidadeDeUsos / (float)alicate.QuantidadeDeUsosMax;

            return;
        }
    }

    public void AtualizarNumeroAtalho(int numero)
    {
        numeroAtalhoObjeto.gameObject.SetActive(true);
        numeroAtalho.text = numero.ToString();
    }
}
