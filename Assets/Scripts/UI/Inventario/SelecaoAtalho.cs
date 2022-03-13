using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecaoAtalho : SelecaoItem
{
    private void Start()
    {
        Iniciar();
    }

    public override void Confirmar(MenuDoInventario menuDoInventario)
    {
        menuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Atalho);
        menuDoInventario.MenuDosItens.AtualizarPosicaoDoPainelDeExplicacaoDosItens(menuDoInventario.PosicaoXBarraDeExplicacaoAtalhos);
        menuDoInventario.MenuDosItens.IniciarSelecaoAtalho(this);
    }
}
