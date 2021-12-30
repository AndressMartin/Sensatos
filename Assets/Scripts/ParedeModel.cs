using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeModel : ObjetoInteragivel
{
    protected int vida;
    [SerializeField] protected int vidaMax;

    public override void Interagir(Player player)
    {
        throw new System.NotImplementedException();
    }

    public virtual void LevarDano(int _dano)
    {
        //Nada.
    }
}
