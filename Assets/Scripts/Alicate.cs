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
        directionHitbox=FindObjectOfType<DirectionHitbox>();
        quantidadeUsos = 1;
        quantidadeUsos = quantidadeUsos * nivel;
    }

    private void Update()
    {
        /*if(directionHitbox != null)
        {
            Debug.Log("chmado");
            parede = (Cerca)directionHitbox.objetectCollision;
            if (parede != null)
            {
                Debug.Log("chmad1212121o");

                parede.LevarDano(dano);
                ConsumirRecurso();
                directionHitbox = null;
            }
            
        }*/
    }


    public override void Usar(GameObject objQueChamou)
    {
        //directionHitbox = objQueChamou.GetComponentInChildren<DirectionHitbox>();

        if(directionHitbox.objetectCollision != null)
            directionHitbox.objetectCollision.LevarDano(dano);
        
    }

  
}
