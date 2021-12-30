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
    private BoxCollider2D hitBoxTiro;
    private DialogueActivator dialogo;
    private DialogueList listaDeDialogos;
    private Animator animator;

    //Enums
    public enum Tipo { Quebravel, Indestrutivel }
    public enum Direcao { Horizontal, Vertical }
    public enum Posicao { Meio, Esquerda, Direita, Vertical}

    //Variaveis
    [SerializeField] private string nome;
    [SerializeField] private Tipo tipo;
    [SerializeField] private Direcao direcao;
    private Posicao posicao;
    private string spriteAtual;

    [SerializeField] private Sprite[] sprites;

    //Variaveis de respawn
    private int vidaRespawn;
    private bool ativoRespawn;
    private string spriteAtualRespawn;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hitBoxTiro = transform.Find("HitBoxTiro").GetComponent<BoxCollider2D>();
        dialogo = GetComponent<DialogueActivator>();
        listaDeDialogos = GetComponent<DialogueList>();
        animator = GetComponent<Animator>();

        //Variaveis
        vida = vidaMax;
        lockDownAtivo = true;
        spriteAtual = "Quebravel";

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager.adicionarAosObjetosInteragiveis(this);
        objectManager.adicionarAsParedesQuebraveis(this);

        AtualizarHitBox();

        switch (tipo)
        {
            case Tipo.Quebravel:
                spriteAtual = "Quebravel";
                break;

            case Tipo.Indestrutivel:
                spriteAtual = "Indestrutivel";
                break;
        }

        SetRespawn();
    }

    public override void SetRespawn()
    {
        vidaRespawn = vida;
        ativoRespawn = lockDownAtivo;
        spriteAtualRespawn = spriteAtual;

        if(lockDownAtivo == true)
        {
            boxCollider2D.enabled = true;
            hitBoxTiro.enabled = true;
        }
    }

    public override void Respawn()
    {
        vida = vidaRespawn;
        lockDownAtivo = ativoRespawn;
        TrocarSprite(spriteAtualRespawn);
    }

    public override void Interagir(Player player)
    {
        switch(tipo)
        {
            case Tipo.Quebravel:
                dialogo.UpdateDialogueObject(listaDeDialogos.GetDialogueObject("CercaQuebravel"));
                dialogo.ShowDialogue(player);
                break;
            case Tipo.Indestrutivel:
                dialogo.UpdateDialogueObject(listaDeDialogos.GetDialogueObject("CercaIndestrutivel"));
                dialogo.ShowDialogue(player);
                break;
        }
    }

    public override void LevarDano(int _dano)
    {
        if(tipo == Tipo.Quebravel)
        {
            vida -= _dano;


            if(vida < vidaMax)
            {
                spriteAtual = "Dano1";
                TrocarSprite(spriteAtual);
            }
            if(vida <= vidaMax / 2)
            {
                spriteAtual = "Dano2";
                TrocarSprite(spriteAtual);
            }
            if (vida <= 0)
            {
                SeDestruir();
            }
        }
    }

    private void SeDestruir()
    {
        spriteAtual = "Destruida";
        TrocarSprite(spriteAtual);
        boxCollider2D.enabled = false;
        hitBoxTiro.enabled = false;
        lockDownAtivo = false;
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
                hitBoxTiro.size = new Vector2(1, 1);
                break;
            case Direcao.Vertical:
                boxCollider2D.size = new Vector2(0.2f, 1);
                boxCollider2D.offset = new Vector2(0, 0.5f);
                hitBoxTiro.size = new Vector2(0.2f, 1);
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

        TrocarSprite(spriteAtual);
    }
}
