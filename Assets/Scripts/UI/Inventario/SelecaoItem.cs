using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecaoItem : SelecaoDoInventario
{
    //Componentes
    private Animator animacao;
    protected InformacoesDoItem informacoesDoItem;

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
        informacoesDoItem = GetComponent<InformacoesDoItem>();

        iniciado = true;
    }

    public override void Confirmar(MenuDoInventario menuDoInventario)
    {
        menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Item);
        menuDoInventario.MenuDosItens.AtualizarPosicaoDoPainelDeExplicacaoDosItens(menuDoInventario.PosicaoXBarraDeExplicacaoItens);
        menuDoInventario.MenuDosItens.IniciarSelecaoItem(this);
    }

    public void ZerarInformacoes()
    {
        informacoesDoItem.ZerarInformacoes();
    }

    public void AtualizarInformacoes(Item item)
    {
        informacoesDoItem.AtualizarInformacoes(item);
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
