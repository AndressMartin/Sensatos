using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformacoesDaMelhoria : MonoBehaviour
{
    [SerializeField] private Image imagem;
    [SerializeField] private TMP_Text nome;
    [SerializeField] private TMP_Text descricao;

    public void AtualizarInformacoes(ArmaDeFogo.Melhoria melhoria)
    {
        imagem.sprite = melhoria.ImagemMelhoria;
        nome.text = melhoria.Nome;
        descricao.text = melhoria.Descricao;
    }
}
