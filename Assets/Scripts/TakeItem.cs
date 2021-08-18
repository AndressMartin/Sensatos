using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    public Inventario Inventario;
    private Rigidbody2D rb;
    private Item obj;
    // Start is called before the first frame update
    void Start()
    {
        obj = GetComponent<Item>();
        rb = GetComponent<Rigidbody2D>();
        Inventario = FindObjectOfType<Movement>().GetComponent<Inventario>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (FindObjectOfType<State>().GetComponent<State>().interagindo)
            {
                Inventario.add(obj);
                gameObject.SetActive(false);
            }

        }
    }
}
