using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ferramenta : Item
{
    public virtual int quantidadeUsos { get; protected set; }

    
    public virtual void Destroy() { }

    public override void ConsumirRecurso()
    {
        quantidadeUsos--;
    }

    public virtual bool VerificarFerramenta() { if (quantidadeUsos <= 0) return false; else return true; }
}
