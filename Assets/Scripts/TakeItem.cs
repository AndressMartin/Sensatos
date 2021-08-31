using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    private Inventario inventario;
    private InventarioMissao inventarioMissao;
    private Rigidbody2D rb;
    private Item obj;
    // Start is called before the first frame update
    void Start()
    {
        obj = GetComponent<Item>();
        rb = GetComponent<Rigidbody2D>();
        inventario = FindObjectOfType<Player>().GetComponent<Inventario>();
        inventarioMissao = FindObjectOfType<Player>().GetComponent<InventarioMissao>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Item")
            {
                if (inventario.gameObject.GetComponent<State>().interagindo)
                {
                    inventario.add(obj);
                    gameObject.SetActive(false);
                }
            }
            else if(gameObject.tag =="ItemChave")
            {
                if (inventario.gameObject.GetComponent<State>().interagindo)
                {
                    inventarioMissao.add(obj);
                    gameObject.SetActive(false);
                }
            }

        }
    }
}
