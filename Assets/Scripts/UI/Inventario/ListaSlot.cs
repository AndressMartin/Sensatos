using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListaSlot : MonoBehaviour
{
    //Componentes
    [SerializeField] private TMP_Text nome;
    [SerializeField] private TMP_Text numero;
    [SerializeField] private Image imagem;

    private Animator animacao;

    //Enuns
    public enum Cor { Branco, Vermelho, Amarelho }

    //Variaveis
    private Cor cor;

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

        animacao = GetComponent<Animator>();

        cor = Cor.Branco;

        SetCor(cor);

        iniciado = true;
    }

    public void ZerarInformacoes()
    {
        nome.text = "";
        numero.text = "";
        imagem.sprite = null;
        numero.gameObject.SetActive(false);
        imagem.gameObject.SetActive(false);
    }

    public void AtualizarNome(string texto)
    {
        nome.text = texto;
    }

    public void AtualizarNumero(int numero)
    {
        this.numero.text = numero.ToString();
        this.numero.gameObject.SetActive(true);
    }

    public void AtualizarImagem(Sprite imagem)
    {
        this.imagem.sprite = imagem;
        this.imagem.gameObject.SetActive(true);
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

    public void SetCor(Cor cor)
    {
        animacao.SetFloat("Cor", (float)cor);
    }
}
