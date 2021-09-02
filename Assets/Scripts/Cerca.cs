using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerca : ParedeModel
{
    [SerializeField]private int PontosVida;
    public override int vida { get; protected set; }
    private State state;
    // Start is called before the first frame update
    void Start()
    {
        vida = PontosVida;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = collision.gameObject.GetComponent<State>();
            state.objetoQualEstaColidindo = this;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = collision.gameObject.GetComponent<State>();
            state.objetoQualEstaColidindo = null;
        }
    }
    public override void LevarDano(int _dano)
    {
        Debug.Log("Levei dano");
        vida -= _dano;


        if (vida <= 0)
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

}
