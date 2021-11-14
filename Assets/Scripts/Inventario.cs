using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public List<Item> itens = new List<Item>();
    public Item itemAtual;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    public void add(Item item)
    {
        itens.Add(item);
        //EquiparItem(item);
       
    }

    public void EquiparItem(Item item)
    {
        itemAtual = item;
    }

    public void UsarItemAtual()
    {
        if (itemAtual != null)
        {
            itemAtual.Usar(gameObject);
        }
    }
    public void TrocarArma()
    {
        
        foreach (Item item in itens)
        {
            if(item.GetComponent<ArmaDeFogo>() !=null)
            {
                if(item.GetComponent<ArmaDeFogo>() !=  itemAtual)
                {
                    itemAtual = item;
                    break;
                }
            }
        }             
    }
}
