using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerca : ParedeModel
{
    private ObjectManagerScript objectManager;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private DialogueActivator dialogueActivator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        dialogueActivator = GetComponent<DialogueActivator>();

        ativo = true;

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager = FindObjectOfType<ObjectManagerScript>();
        objectManager.adicionarAosObjetosInteragiveis(this);
        objectManager.adicionarAsParedesQuebraveis(this);
    }

    public override void Interagir(Player player)
    {
        if(ativo == true)
        {
            dialogueActivator.ShowDialogue(player);
        }
    }

    public override void LevarDano(int _dano)
    {
        vida -= _dano;

        if (vida <= 0)
        {
            SeDestruir();
        }
    }

    private void SeDestruir()
    {
        spriteRenderer.enabled = false;
        boxCollider2D.isTrigger = true;
        ativo = false;
    }
}
