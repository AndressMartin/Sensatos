using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    [SerializeField]public override int vida { get; protected set; }
    public int Pv;
    // Start is called before the first frame update
    void Start()
    {
        vida = Pv;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void curar(int _cura)
    {
        Debug.Log(vida);
        vida = +_cura;
        Debug.Log(vida);
    }

    public override void TomarDano(int _dano)
    {
        Debug.Log("tomei tiro, player");

    }

}
