using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformacoesDaRoupa : MonoBehaviour
{
    //Componentes
    [SerializeField] private Image imagem;

    public void ZerarInformacoes()
    {
        imagem.sprite = null;
    }

    public void AtualizarInformacoes(RoupaDeCamuflagem roupa)
    {
        imagem.sprite = roupa.ImagemInventario;
    }
}
