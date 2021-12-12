using System;
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
    public enum Direcao { Horizontal, Vertical }
    public enum Posicao { Meio, Esquerda, Direita, Vertical}

    public enum SpriteName { Quebravel, Indestrutivel, Dano1, Dano2, Destruido }

    //Variaveis
    [SerializeField] private string nome;
    [SerializeField] private Tipo tipo;
    [SerializeField] private Direcao direcao;
    private Posicao posicao;
    private SpriteName sprite;

    [SerializeField] private Sprite[] sprites;

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

        AtualizarHitBox();
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
                TrocarSprite("Dano1");
            }
            if(vida <= vidaMax / 2)
            {
                TrocarSprite("Dano2");
            }
            if (vida <= 0)
            {
                SeDestruir();
            }
        }
    }

    private void SeDestruir()
    {
        TrocarSprite("Destruida");
        boxCollider2D.isTrigger = true;
        ativo = false;
    }

    private void TrocarSprite(string spriteName)
    {
        int indice = Array.FindIndex(sprites, sprite => sprite.name == (nome + "_" + spriteName + posicao.ToString())); //Da o indice do sprite no array que tiver o nome igual ao que for passado
        spriteRenderer.sprite = sprites[indice];
    }

    private void AtualizarHitBox()
    {
        switch(direcao)
        {
            case Direcao.Horizontal:
                boxCollider2D.size = new Vector2(1, 0.2f);
                boxCollider2D.offset = new Vector2(0, 0.1f);
                transform.Find("HitBoxTiro").GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                break;
            case Direcao.Vertical:
                boxCollider2D.size = new Vector2(0.2f, 1);
                boxCollider2D.offset = new Vector2(0, 0.5f);
                transform.Find("HitBoxTiro").GetComponent<BoxCollider2D>().size = new Vector2(0.2f, 1);
                break;
        }
    }

    public void ArrumarPosicao(Cerca[] cercas)
    {
        bool colisaoEsquerda, colisaoDireita;
        colisaoEsquerda = false;
        colisaoDireita = false;

        if(direcao == Direcao.Horizontal)
        {
            foreach (Cerca cerca in cercas)
            {
                if (cerca != this)
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

            if (colisaoEsquerda == true && colisaoDireita == true)
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
        }
        else
        {
            posicao = Posicao.Vertical;
        }

        switch (tipo)
        {
            case Tipo.Quebravel:
                TrocarSprite("Quebravel");
                break;

            case Tipo.Indestrutivel:
                TrocarSprite("Indestrutivel");
                break;
        }
    }
}
