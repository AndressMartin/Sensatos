using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioMissao : MonoBehaviour
{
    //Variaveis
    private List<Item> itens = new List<Item>();

    //Getters
    public List<Item> Itens => itens;

    public void Add(Item item)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        Item novoItem = ScriptableObject.Instantiate(item);
        itens.Add(novoItem);
    }

    public void Remove(Item item)
    {
        itens.Remove(item);
        Destroy(item);
    }
}
