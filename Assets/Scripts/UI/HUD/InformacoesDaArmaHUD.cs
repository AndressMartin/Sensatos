using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformacoesDaArmaHUD : MonoBehaviour
{
    [SerializeField] private Image imagem;
    [SerializeField] private RectTransform borda;
    [SerializeField] private TMP_Text municao;

    public void ZerarInformacoes()
    {
        imagem.gameObject.SetActive(false);
        borda.gameObject.SetActive(false);
        imagem.sprite = null;
        municao.text = "";
    }

    public void AtualizarInformacoes(ArmaDeFogo arma)
    {
        imagem.gameObject.SetActive(true);
        borda.gameObject.SetActive(true);
        imagem.sprite = arma.ImagemInventario;
        municao.text = arma.MunicaoCartucho.ToString() + "/" + arma.Municao.ToString();
    }

    public void BarraDeMunicaoAtiva(bool ativa)
    {
        borda.gameObject.SetActive(ativa);
    }
}
