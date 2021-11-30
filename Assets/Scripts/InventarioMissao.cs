using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioMissao : MonoBehaviour
{
    public List<Item> itens = new List<Item>();

    public void Add(Item item)
    {
        itens.Add(item);
    }

    public void Remove(Item item)
    {
        itens.Remove(item);
    }
}
