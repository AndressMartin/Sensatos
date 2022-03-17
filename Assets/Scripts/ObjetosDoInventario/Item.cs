using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ItemDoInventario
{
    //Enums
    public enum TipoItem { Consumivel, Ferramenta, ItemChave };

    //Variaveis
    [SerializeField] protected int id;

    //Getters
    public virtual int ID => id;
    public virtual TipoItem Tipo => TipoItem.Consumivel;
    virtual public string GetNomeAnimacao => "";

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
