using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeModel : MonoBehaviour
{
    public virtual int vida { get; protected set; }
    private State state;


    

    private  void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = collision.gameObject.GetComponent<State>();
            state.objetoQualEstaColidindo = this;
        }
    }
    private  void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = collision.gameObject.GetComponent<State>();
            state.objetoQualEstaColidindo = null;
        }
    }

    public virtual void LevarDano(int _dano)
    { }
}
