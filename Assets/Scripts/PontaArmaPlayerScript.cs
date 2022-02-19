using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArmaPlayerScript : PontaArmaScript
{
    //Variaveis
    private int colidindoComParedes;

    //Getters
    public int ColidindoComParedes => colidindoComParedes;
    void Start()
    {
        colidindoComParedes = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ParedeTiro"))
        {
            colidindoComParedes++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ParedeTiro"))
        {
            colidindoComParedes--;
        }
    }
}
