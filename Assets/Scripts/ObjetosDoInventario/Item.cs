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

    virtual public void Usar(Player player)
    {
        //Nada.
    }

    virtual public void UsarNoInventario(Player player)
    {
        //Nada.
    }

    virtual public void UsarNaGameplay(Player player)
    {
        //Nada.
    }

    virtual public string GetNomeAnimacao()
    {
        return "";
    }

    public void JogarFora()
    {
        //Alguma coisa
    }
}
