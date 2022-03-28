using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ItemDoInventario
{
    //Enums
    public enum TipoItem { Consumivel, Ferramenta, ItemChave };

    //Variaveis
    protected int quantidadeDeUsos = 0;

    //Getters
    public virtual int ID => Listas.instance.ListaDeItens.GetID[this.name];
    public virtual TipoItem Tipo => TipoItem.Consumivel;
    virtual public string GetNomeAnimacao => "";
    public int QuantidadeDeUsos => quantidadeDeUsos;

    //Setters
    public void SetQuantidadeDeUsos(int quantidadeDeUsos)
    {
        this.quantidadeDeUsos = quantidadeDeUsos;
    }

    virtual public void Iniciar()
    {
        //Nada.
    }

    virtual public void Usar(Player player)
    {
        //Nada.
    }

    virtual public bool UsarNoInventario(Player player)
    {
        return false;
    }

    virtual public void UsarNaGameplay(Player player)
    {
        //Nada.
    }

    public void JogarFora(Player player)
    {
        player.Inventario.RemoverItem(this);
    }
}
