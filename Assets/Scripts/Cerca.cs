using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerca : ParedeModel
{
    [SerializeField]private int PontosVida;
    public override int vida { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        vida = PontosVida;
    }

    public override void LevarDano(int _dano)
    {
        vida -= _dano;
        Debug.Log("vida atual: " + vida + " dano: " + _dano + " vida após dano: " + (vida - _dano));


        if (vida <= 0)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            Destroy(this);
        }
    }

}
