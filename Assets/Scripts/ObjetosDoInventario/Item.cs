using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Usable
{
    //Variaveis
    public enum Tipo { Consumivel, Ferramenta, ItemChave };

    [HideInInspector] public virtual Tipo tipo { get; protected set; }

    virtual public void Usar(Player player)
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

    public void ThrowAway()
    {

    }
}
