using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    private Inventario inventario;
    private InventarioMissao inventarioMissao;
    private ItemDirectionHitbox itemDirectionHitbox;

    // Start is called before the first frame update
    void Start()
    {
        inventario = GetComponentInParent<Inventario>();
        inventarioMissao =GetComponentInParent<InventarioMissao>();
        itemDirectionHitbox = GetComponent<ItemDirectionHitbox>();
    }

    // Update is called once per frame
    void Update()
    {
        if(itemDirectionHitbox.objetectCollision != null)
        {
            AddToInventario(itemDirectionHitbox.objetectCollision);
        }
    }

    void AddToInventario(Item _obj)
    {
        if (itemDirectionHitbox.objetectCollision.gameObject.tag == "Item")
        {
            inventario.add(_obj);            
        }
        else if (itemDirectionHitbox.objetectCollision.gameObject.tag == "ItemChave")
        {   
            inventarioMissao.add(_obj);
             
        }
        itemDirectionHitbox.objetectCollision.gameObject.SetActive(false);//item sumuir
        itemDirectionHitbox.objetectCollision = null; //perder referencia


    }
}
