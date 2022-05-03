using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmaLojaSlot : MonoBehaviour
{
    //Componentes
    [SerializeField] private TMP_Text nome;
    [SerializeField] private TMP_Text preco;
    [SerializeField] private Image imagem;
    [SerializeField] private Image imagemMunicao;

    private Image imagemFundo;

    //Variaveis
    private bool iniciado = false;

    private void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        imagemFundo = GetComponent<Image>();

        iniciado = true;
    }

    public void ZerarInformacoes()
    {
        imagem.sprite = null;
        imagemMunicao.gameObject.SetActive(false);
        nome.text = "";
        preco.text = "";
    }

    public void AtualizarInformacoes(ArmaDeFogo arma, int preco)
    {
        imagem.sprite = arma.ImagemInventario;
        nome.text = arma.Nome;
        this.preco.text = preco.ToString();
    }

    public void ImagemMunicaoAtiva(bool ativa)
    {
        imagemMunicao.gameObject.SetActive(ativa);
    }

    public void Selecionado(bool selecionado)
    {
        if (selecionado == true)
        {
            imagemFundo.color = Color.blue;
        }
        else
        {
            imagemFundo.color = Color.red;
        }
    }
}
