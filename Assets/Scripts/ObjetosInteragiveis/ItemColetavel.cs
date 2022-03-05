using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColetavel : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    //Variaveis
    [SerializeField] private Item item;

    //Variaveis de respawn
    private bool ativoRespawn;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        //Variaveis
        ativo = true;

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);

        SetRespawn();
    }

    public override void SetRespawn()
    {
        ativoRespawn = ativo;
    }

    public override void Respawn()
    {
        ativo = ativoRespawn;

        if(ativo == true)
        {
            spriteRenderer.enabled = true;
            boxCollider2D.enabled = true;
        }
    }

    public override void Interagir(Player player)
    {
        if(item != null)
        {
            switch(item.Tipo)
            {
                case Item.TipoItem.Consumivel:
                    AdicionarAoInventario(player);
                    Desativar();
                    break;

                case Item.TipoItem.Ferramenta:
                    AdicionarAoInventario(player);
                    Desativar();
                    break;

                case Item.TipoItem.ItemChave:
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
        player.Inventario.AddItem(item);
    }

    private void AdicionarAoInventarioMissao(Player player)
    {
        player.InventarioMissao.Add(item);
    }

    private void Desativar()
    {
        ativo = false;
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
    }
}
