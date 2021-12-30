using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColetavel : ObjetoInteragivel
{
    [SerializeField] private Item item;

    //Componentes
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private ObjectManagerScript objectManager;

    //Variaveis de respawn
    private bool ativoRespawn;

    void Start()
    {
        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager = FindObjectOfType<ObjectManagerScript>();
        objectManager.adicionarAosObjetosInteragiveis(this);
    }

    public override void SetRespawn()
    {
        ativoRespawn = lockDownAtivo;
    }

    public override void Respawn()
    {
        lockDownAtivo = ativoRespawn;

        if(lockDownAtivo == true)
        {
            spriteRenderer.enabled = true;
            boxCollider2D.enabled = true;
        }
    }

    public override void Interagir(Player player)
    {
        if(item != null)
        {
            switch(item.tipo)
            {
                case Item.Tipo.Consumivel:
                    AdicionarAoInventario(player);
                    Desativar();
                    break;

                case Item.Tipo.Ferramenta:
                    AdicionarAoInventario(player);
                    Desativar();
                    break;

                case Item.Tipo.ItemChave:
                    AdicionarAoInventarioMissao(player);
                    Desativar();
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

    private void Desativar()
    {
        lockDownAtivo = false;
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
    }
}
