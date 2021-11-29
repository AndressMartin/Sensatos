using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColetavel : ObjetoInteragivel
{
    [SerializeField] private Item item;

    private SpriteRenderer spriteRenderer;

    private ObjectManagerScript objectManager;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager = FindObjectOfType<ObjectManagerScript>();
        objectManager.adicionarAosObjetosInteragiveis(this);
    }

    public override void Interagir(Player player)
    {
        if(item != null)
        {
            switch(item.tipo)
            {
                case Item.Tipo.Consumivel:
                    player.AdicionarAoInventario(item);
                    SeExcluir();
                    break;

                case Item.Tipo.Ferramenta:
                    player.AdicionarAoInventario(item);
                    SeExcluir();
                    break;

                case Item.Tipo.ItemChave:
                    player.AdicionarAoInventarioMissao(item);
                    SeExcluir();
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Nao ha um item para adicionar, alguem tem que ver isso ai, ne.");
        }
    }

    private void SeExcluir()
    {
        objectManager.removerDosObjetosInteragiveis(this);
        Destroy(this.gameObject);
    }
}
