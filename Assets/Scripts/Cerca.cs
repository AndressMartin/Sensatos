using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerca : ParedeModel
{
    //Managers
    private ObjectManagerScript objectManager;

    //Componentes
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private DialogueActivator dialogueActivator;
    private Animator animator;

    //Enums
    public enum Tipo { Quebravel, Indestrutivel }

    //Variaveis
    [SerializeField] public Tipo tipo;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        dialogueActivator = GetComponent<DialogueActivator>();
        animator = GetComponent<Animator>();

        //Variaveis
        vida = vidaMax;
        ativo = true;

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager.adicionarAosObjetosInteragiveis(this);
        objectManager.adicionarAsParedesQuebraveis(this);

        switch(tipo)
        {
            case Tipo.Quebravel:
                TrocarAnimacao("NormalQuebravel");
                break;

            case Tipo.Indestrutivel:
                TrocarAnimacao("Indestrutivel");
                break;
        }
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
        if(tipo == Tipo.Quebravel)
        {
            vida -= _dano;


            if(vida < vidaMax)
            {
                TrocarAnimacao("Dano1");
            }
            if(vida <= vidaMax / 2)
            {
                TrocarAnimacao("Dano2");
            }
            if (vida <= 0)
            {
                SeDestruir();
            }
        }
    }

    private void SeDestruir()
    {
        TrocarAnimacao("Destruida");
        boxCollider2D.isTrigger = true;
        ativo = false;
    }

    public void TrocarAnimacao(string animacao)
    {
        animator.Play(animacao);
    }
}
