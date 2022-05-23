using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLojaSlot : MonoBehaviour
{
    //Componentes
    [SerializeField] private Image imagem;
    [SerializeField] private TMP_Text preco;

    private Animator animacao;

    //Variaveis
    protected bool iniciado = false;

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

        animacao = GetComponent<Animator>();

        iniciado = true;
    }

    public void ZerarInformacoes()
    {
        imagem.sprite = null;
        preco.text = "";
    }

    public void AtualizarInformacoes(Sprite imagem, int preco)
    {
        this.imagem.sprite = imagem;
        this.preco.text = preco.ToString();
    }

    public void Selecionado(bool selecionado)
    {
        if (selecionado == true)
        {
            animacao.SetBool("Selecionado", true);
        }
        else
        {
            animacao.SetBool("Selecionado", false);
        }
    }
}
