using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioMissao : MonoBehaviour
{
    public List<Item> itens = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void add(Item item)
    {
        itens.Add(item);

    }
}
