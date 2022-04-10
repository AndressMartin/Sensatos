using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Item Chave")]

public class ItemChave : Item
{
    public override TipoItem Tipo => TipoItem.ItemChave;

    //Variaveis
    private int quantidade = 0;

    //Getters
    public int Quantidade => quantidade;

    //Setters
    public void SetQuantidade(int quantidade)
    {
        this.quantidade = quantidade;
    }
}
