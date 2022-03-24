using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListaSlot : MonoBehaviour
{
    //Componentes
    [SerializeField] private TMP_Text nome;
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
        imagemConquista.sprite = null;
        imagemConquista.gameObject.SetActive(false);
    }

    public void AtualizarNome(string texto)
    {
        nome.text = texto;
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
        }
        else
        {
            imagem.color = Color.white;
            nome.color = Color.black;
        }
    }
}
