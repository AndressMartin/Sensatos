using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public Item obj;
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    bool aberto;
    public bool trancado;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Movement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player encostando na porta");
            if (player.GetComponent<State>().interagindo)
            {
                if (player.GetComponent<InventarioMissao>().itens.Contains(obj) && trancado)
                {
                    Debug.Log("destrancou porta");
                    trancado = false;
                    spriteRenderer.color = (Color.blue);
                }

                else if(!trancado)
                {
                    Debug.Log("abriur porta");
                    aberto = true;
                    spriteRenderer.color = (Color.red);
                    Door(aberto);
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!trancado && aberto)
            {
                //if (player.GetComponent<State>().interagindo == false)
                //{
                    Debug.Log("fechou porta");
                    aberto = false;
                    spriteRenderer.color = (Color.yellow);
                    Door(aberto);
                //}
            }
        }
    }

 

    void Door(bool portaAberta)
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = portaAberta;
    }
}
