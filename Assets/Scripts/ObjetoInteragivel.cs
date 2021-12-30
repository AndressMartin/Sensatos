using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjetoInteragivel : MonoBehaviour
{
    protected bool ativo = true;
    public bool Ativo => ativo;
    abstract public void Interagir(Player player);
    public virtual void SetRespawn()
    {
        //Nada
    }
    public virtual void Respawn()
    {
        //Nada
    }
}
