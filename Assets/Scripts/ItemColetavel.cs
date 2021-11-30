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
                    AdicionarAoInventario(player);
                    SeExcluir();
                    break;

                case Item.Tipo.Ferramenta:
                    AdicionarAoInventario(player);
                    SeExcluir();
                    break;

                case Item.Tipo.ItemChave:
                    AdicionarAoInventarioMissao(player);
                    SeExcluir();
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Nao ha um item para adicionar, alguem tem que ver isso ai, ne.");
        }
    }

    private void AdicionarAoInventario(Player player)
    {
        Item novoItem;
        novoItem = Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
        player.AdicionarAoInventario(novoItem);
    }

    private void AdicionarAoInventarioMissao(Player player)
    {
        Item novoItem;
        novoItem = Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
        player.AdicionarAoInventarioMissao(novoItem);
    }

    private void SeExcluir()
    {
        objectManager.removerDosObjetosInteragiveis(this);
        Destroy(this.gameObject);
    }
}
