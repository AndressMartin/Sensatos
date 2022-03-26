using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Item Chave")]

public class ItemChave : Item
{
    public override TipoItem Tipo => TipoItem.ItemChave;

    //Variaveis
    private int numero = 0;

    //Getters
    public int Numero => numero;

    //Setters
    public void SetNumero(int numero)
    {
        this.numero = numero;
    }
}
