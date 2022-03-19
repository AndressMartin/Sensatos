using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformacoesDaArmaHUD : MonoBehaviour
{
    [SerializeField] private Image imagem;
    [SerializeField] private TMP_Text municao;

    public void ZerarInformacoes()
    {
        imagem.sprite = null;
        municao.text = "";
    }

    public void AtualizarInformacoes(ArmaDeFogo arma)
    {
        imagem.sprite = arma.ImagemInventario;
        municao.text = arma.MunicaoCartucho.ToString() + "/" + arma.Municao.ToString();
    }
}
