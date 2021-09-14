using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alicate : Ferramenta
{
    [SerializeField] private int dano;
    private DirectionHitbox directionHitbox;
    private Cerca parede;
    public int nivel;
    public override int quantidadeUsos { get; protected set; }

    void Start()
    {
        quantidadeUsos = 1;
        quantidadeUsos = quantidadeUsos * nivel;
    }

    private void Update()
    {
        if(directionHitbox != null)
        {
            parede = (Cerca)directionHitbox.objetectCollision;
            if (parede != null)
            {
                parede.LevarDano(dano);
                ConsumirRecurso();
                directionHitbox = null;
            }
        }
    }


    public override void Usar(GameObject objQueChamou)
    {
        if(VerificarFerramenta())//caso o item tenha uso
        {
            directionHitbox = objQueChamou.GetComponentInChildren<DirectionHitbox>();         
        }
    }

  
}
