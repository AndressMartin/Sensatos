using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecaoArma : SelecaoDoInventario
{
    //Componentes
    private Image imagem;
    private InformacoesDaArma informacoesDaArma;

    //Variaveis
    [SerializeField] private int indiceArmaAtual;

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
        informacoesDaArma = GetComponent<InformacoesDaArma>();

        iniciado = true;
    }

    public override void Confirmar(MenuDoInventario menuDoInventario)
    {
        menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Arma);
        menuDoInventario.MenuDasArmas.AtualizarPosicaoDoScroolDasArmas(transform.position.y);
        menuDoInventario.MenuDasArmas.IniciarScrool(indiceArmaAtual);
    }

    public override void Voltar(MenuDoInventario menuDoInventario)
    {
        //Nada
    }

    public void ZerarInformacoes()
    {
        informacoesDaArma.ZerarInformacoes();
    }

    public void AtualizarInformacoes(ArmaDeFogo arma)
    {
        informacoesDaArma.AtualizarInformacoes(arma);
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
