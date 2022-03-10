using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecaoArma : SelecaoDoInventario
{
    private Image imagem;

    [SerializeField] private float ScroolPosicaoX;
    [SerializeField] private float ScroolPosicaoY;

    private void Start()
    {
        imagem = GetComponent<Image>();
    }

    public override void Confirmar(MenuDoInventario menuDoInventario)
    {
        menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Arma);

    }

    public override void Voltar(MenuDoInventario menuDoInventario)
    {
        //Nada
    }

    public override void Selecionado(bool selecionado)
    {
        if(selecionado == true)
        {
            imagem.color = Color.blue;
        }
        else
        {
            imagem.color = Color.red;
        }
    }
}
