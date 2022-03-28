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
    [SerializeField] private Image imagemConquista;

    private Image imagem;

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

        imagem = GetComponent<Image>();

        iniciado = true;
    }

    public void ZerarInformacoes()
    {
        nome.text = "";
        numero.text = "";
        imagemConquista.sprite = null;
        numero.gameObject.SetActive(false);
        imagemConquista.gameObject.SetActive(false);
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
        this.imagemConquista.sprite = imagem;
        this.imagemConquista.gameObject.SetActive(true);
    }

    public void Selecionado(bool selecionado)
    {
        if (selecionado == true)
        {
            imagem.color = Color.blue;
            nome.color = Color.white;
            numero.color = Color.white;
        }
        else
        {
            imagem.color = Color.white;
            nome.color = Color.black;
            numero.color = Color.black;
        }
    }
}
