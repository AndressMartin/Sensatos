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
    private DialogueActivator dialogoQuebravel;
    private DialogueActivator dialogoIndestrutivel;
    private Animator animator;

    //Enums
    public enum Tipo { Quebravel, Indestrutivel }
    public enum Posicao { Meio, Esquerda, Direita }

    //Variaveis
    [SerializeField] public Tipo tipo;
    private Posicao posicao;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        dialogoQuebravel = transform.Find("DialogoQuebravel").GetComponent<DialogueActivator>();
        dialogoIndestrutivel = transform.Find("DialogoIndestrutivel").GetComponent<DialogueActivator>();
        animator = GetComponent<Animator>();

        //Variaveis
        vida = vidaMax;
        ativo = true;

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager.adicionarAosObjetosInteragiveis(this);
        objectManager.adicionarAsParedesQuebraveis(this);
    }

    public override void Interagir(Player player)
    {
        if(ativo == true)
        {
            switch(tipo)
            {
                case Tipo.Quebravel:
                    dialogoQuebravel.ShowDialogue(player);
                    break;
                case Tipo.Indestrutivel:
                    dialogoIndestrutivel.ShowDialogue(player);
                    break;
            }
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

    private void TrocarAnimacao(string animacao)
    {
        animator.Play(animacao + posicao.ToString());
    }

    public void ArrumarPosicao(Cerca[] cercas)
    {
        bool colisaoEsquerda, colisaoDireita;
        colisaoEsquerda = false;
        colisaoDireita = false;

        foreach (Cerca cerca in cercas)
        {
            if(cerca != this)
            {
                if (Colisao.HitTest((boxCollider2D.bounds.center.x - (boxCollider2D.bounds.extents.x * 2)), boxCollider2D.bounds.center.y, cerca.transform.GetComponent<BoxCollider2D>()))
                {
                    colisaoEsquerda = true;
                }
                else if (Colisao.HitTest((boxCollider2D.bounds.center.x + (boxCollider2D.bounds.extents.x * 2)), boxCollider2D.bounds.center.y, cerca.transform.GetComponent<BoxCollider2D>()))
                {
                    colisaoDireita = true;
                }
            }
        }

        if(colisaoEsquerda == true && colisaoDireita == true)
        {
            posicao = Posicao.Meio;
        }
        else if (colisaoEsquerda == true)
        {
            posicao = Posicao.Direita;
        }
        else if (colisaoDireita == true)
        {
            posicao = Posicao.Esquerda;
        }
        else
        {
            posicao = Posicao.Meio;
        }

        switch (tipo)
        {
            case Tipo.Quebravel:
                TrocarAnimacao("Quebravel");
                break;

            case Tipo.Indestrutivel:
                TrocarAnimacao("Indestrutivel");
                break;
        }
    }
}
