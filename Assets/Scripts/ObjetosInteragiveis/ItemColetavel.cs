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

    //Enuns
    public enum Tipo { Normal, ItemUnico, ItemLigadoAFlag }

    //Variaveis
    [SerializeField] private Item item;
    [SerializeField] private AudioClip somAoSerColetado;
    [SerializeField] private Tipo tipo;
    [SerializeField] private Flags.Flag flag;

    private bool itemFoiColetado;

    //Variaveis de respawn
    private bool ativoRespawn;

    //Getter
    public Item GetItem => item;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        //Variaveis
        itemFoiColetado = false;

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);

        if(tipo == Tipo.ItemUnico)
        {
            StartCoroutine(ConferirSeOItemEstaNoInventario());
        }
        else if(tipo == Tipo.ItemLigadoAFlag)
        {
            if(Flags.GetFlag(flag) == true)
            {
                Desativar();
            }
        }

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
        itemFoiColetado = false;

        if(item != null)
        {
            if(somAoSerColetado != null)
            {
                generalManager.SoundManager.TocarSom(somAoSerColetado);
            }

            switch(item.Tipo)
            {
                case Item.TipoItem.Consumivel:
                    AdicionarAoInventario(player);
                    break;

                case Item.TipoItem.Ferramenta:
                    AdicionarAoInventario(player);
                    break;

                case Item.TipoItem.ItemChave:
                    AdicionarAoInventarioMissao(player);
                    break;
            }

            if(itemFoiColetado == true)
            {
                Desativar();

                if(tipo == Tipo.ItemLigadoAFlag)
                {
                    Flags.SetFlag(flag, true);
                }
            }
            else
            {
                //Substituir pela funcao que vai dar ao jogador a opcao de escolher um item para colocar este no lugar ou nao.
                Debug.LogWarning("Nao havia espaco no inventario para adicionar o item");
            }
        }
        else
        {
            Debug.LogWarning("Nao ha um item para adicionar, alguem tem que ver isso ai, ne.");
        }
    }

    private void AdicionarAoInventario(Player player)
    {
        itemFoiColetado = player.Inventario.AdicionarItem(item);
    }

    private void AdicionarAoInventarioMissao(Player player)
    {
        ItemChave itemChave = (ItemChave)item;

        player.InventarioMissao.AdicionarItem(itemChave);
        itemFoiColetado = true;
    }

    private void Desativar()
    {
        ativo = false;
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
    }

    private IEnumerator ConferirSeOItemEstaNoInventario()
    {
        yield return null;

        if(item is ItemChave)
        {
            foreach (ItemChave itemNoInventario in generalManager.Player.InventarioMissao.Itens)
            {
                if (item.ID == itemNoInventario.ID)
                {
                    Desativar();
                    break;
                }
            }
        }
        else
        {
            foreach(Item itemNoInventario in generalManager.Player.Inventario.Itens)
            {
                if(item.ID == itemNoInventario.ID)
                {
                    Desativar();
                    break;
                }
            }
        }

        SetRespawn();
    }
}
