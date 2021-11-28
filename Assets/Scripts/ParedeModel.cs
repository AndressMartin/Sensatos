using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeModel : ObjetoInteragivel
{
    public int vida;
    public bool ativo;

    public override void Interagir(Player player)
    {
        throw new System.NotImplementedException();
    }

    public virtual void LevarDano(int _dano)
    {
        //Nada.
    }
}
