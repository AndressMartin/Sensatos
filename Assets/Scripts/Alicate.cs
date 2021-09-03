using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alicate : Ferramenta
{
    [SerializeField] private int dano;
    private State state;
    public int nivel;
    public override int quantidadeUsos { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        quantidadeUsos = 1;
        quantidadeUsos = quantidadeUsos * nivel;
    }

    // Update is called once per frame

    public override void Usar(GameObject objQueChamou)
    {
        if(VerificarFerramenta())
        {
            state = objQueChamou.GetComponent<State>();

            if (state.objetoQualEstaColidindo is Cerca)
            {
                state.objetoQualEstaColidindo.LevarDano(dano);
                ConsumirRecurso();
                Debug.Log(quantidadeUsos);
            }
        }
    }

  
}
