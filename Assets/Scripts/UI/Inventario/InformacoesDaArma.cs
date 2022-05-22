using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformacoesDaArma : MonoBehaviour
{
    [SerializeField] private Image imagem;
    [SerializeField] private RectTransform nomeBorda;
    [SerializeField] private TMP_Text nome;
    [SerializeField] private TMP_Text municao;

    public void ZerarInformacoes()
    {
        imagem.gameObject.SetActive(false);
        nomeBorda.gameObject.SetActive(false);
        imagem.sprite = null;
        nome.text = "";
        municao.text = "";
    }

    public void AtualizarInformacoes(ArmaDeFogo arma)
    {
        imagem.gameObject.SetActive(true);
        nomeBorda.gameObject.SetActive(true);
        imagem.sprite = arma.ImagemInventario;
        nome.text = arma.Nome;
        municao.text = arma.MunicaoCartucho.ToString() + "/" + arma.Municao.ToString();
    }
}
