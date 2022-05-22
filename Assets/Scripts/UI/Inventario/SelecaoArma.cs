using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecaoArma : SelecaoDoInventario
{
    //Componentes
    private Animator animacao;
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

        animacao = GetComponent<Animator>();
        informacoesDaArma = GetComponent<InformacoesDaArma>();

        iniciado = true;
    }

    public override void Confirmar(MenuDoInventario menuDoInventario)
    {
        if (GameManager.instance.ModoDeJogo == GameManager.Modo.Cidade && menuDoInventario.GeneralManager.Player.Inventario.ArmaSlot[indiceArmaAtual] != null)
        {
            menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Arma);
            menuDoInventario.MenuDasArmas.AtualizarPosicaoDoScroolDasArmas(transform.position.y);
            menuDoInventario.MenuDasArmas.IniciarScrool(indiceArmaAtual);
        }
        else
        {
            menuDoInventario.GeneralManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
        }
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
