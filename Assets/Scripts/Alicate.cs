using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alicate : Ferramenta
{
    private State state;
    [SerializeField] private int dano;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Usar(GameObject objQueChamou)
    {
        state = objQueChamou.GetComponent<State>();

        if (state.objetoQualEstaColidindo is Cerca)
        {
            state.objetoQualEstaColidindo.LevarDano(dano);
            
        } 
    }

   
}
