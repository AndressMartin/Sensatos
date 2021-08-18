using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public List<Item> itens = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in itens)
        {
            Debug.Log(item); 
        }
    }
    public void add(Item item)
    {
        itens.Add(item);
       
    }
}
