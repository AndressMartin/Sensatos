using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    public Item obj;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
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
            if (player.GetComponent<Inventario>().itens.Contains(obj))
            {
                if (player.GetComponent<State>().interagindo)
                {
                    Debug.Log("abriu porta");
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}
