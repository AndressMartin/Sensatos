using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    private Inventario inventario;
    private InventarioMissao inventarioMissao;
    private Item obj;
    // Start is called before the first frame update
    void Start()
    {
        obj = GetComponent<Item>();
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
            if (inventario.gameObject.GetComponent<State>().interagindo)
            {
                AddToInventario();
            }
        }
    }

    void AddToInventario()
    {
        if (gameObject.tag == "Item")
        {
            inventario.add(obj);
            
        }
        else if (gameObject.tag == "ItemChave")
        {   
            inventarioMissao.add(obj);
             
        }
        gameObject.SetActive(false);
    }
}
