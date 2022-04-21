using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeAssaltoColetavel : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    //Variaveis
    [SerializeField] private ItemDeAssalto item;
    [SerializeField] private AudioClip somAoSerColetado;

    //Variaveis de respawn
    private bool ativoRespawn;

    //Getter
    public ItemDeAssalto GetItem => item;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

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

        if (ativo == true)
        {
            spriteRenderer.enabled = true;
            boxCollider2D.enabled = true;
        }
    }

    public override void Interagir(Player player)
    {
        if (item != null)
        {
            if (somAoSerColetado != null)
            {
                generalManager.SoundManager.TocarSom(somAoSerColetado);
            }

            generalManager.AssaltoInfo.AdicionarItemDeAssalto(item);

            Desativar();
        }
        else
        {
            Debug.LogWarning("Nao ha um item para adicionar, alguem tem que ver isso ai, ne.");
        }
    }

    private void Desativar()
    {
        ativo = false;
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
    }
}
