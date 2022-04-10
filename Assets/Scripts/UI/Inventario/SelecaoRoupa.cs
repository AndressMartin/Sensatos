using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecaoRoupa : SelecaoDoInventario
{
    //Componentes
    private Image imagem;
    private InformacoesDaRoupa informacoesDaRoupa;

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
        informacoesDaRoupa = GetComponent<InformacoesDaRoupa>();

        iniciado = true;
    }

    public override void Confirmar(MenuDoInventario menuDoInventario)
    {
        if (GameManager.instance.ModoDeJogo == GameManager.Modo.Cidade)
        {
            menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Roupa);
            menuDoInventario.MenuDasRoupas.IniciarScrool();
        }
        else
        {
            menuDoInventario.GeneralManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
        }
    }

    public void ZerarInformacoes()
    {
        informacoesDaRoupa.ZerarInformacoes();
    }

    public void AtualizarInformacoes(RoupaDeCamuflagem roupa)
    {
        informacoesDaRoupa.AtualizarInformacoes(roupa);
    }

    public override void Selecionado(bool selecionado)
    {
        if (selecionado == true)
        {
            imagem.color = Color.blue;
        }
        else
        {
            imagem.color = Color.red;
        }
    }
}
