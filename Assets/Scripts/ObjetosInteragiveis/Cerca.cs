using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerca : ParedeModel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private BoxCollider2D hitBoxTiro;
    private DialogueActivator dialogo;
    private DialogueList listaDeDialogos;

    //Enums
    public enum Tipo { Quebravel, Indestrutivel }
    public enum Direcao { Horizontal, Vertical }
    public enum Posicao { Meio, Esquerda, Direita, Vertical}

    //Variaveis
    private bool iniciado = false;

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
        Iniciar();
    }

    private void Iniciar()
    {
        if(iniciado == true)
        {
            return;
        }

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hitBoxTiro = transform.Find("HitBoxTiro").GetComponent<BoxCollider2D>();
        dialogo = GetComponent<DialogueActivator>();
        listaDeDialogos = GetComponent<DialogueList>();

        //Variaveis
        vida = vidaMax;
        ativo = true;
        spriteAtual = "Quebravel";

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);
        generalManager.ObjectManager.AdicionarAsParedesQuebraveis(this);

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

        iniciado = true;
    }

    public override void SetRespawn()
    {
        vidaRespawn = vida;
        ativoRespawn = ativo;
        spriteAtualRespawn = spriteAtual;
    }

    public override void Respawn()
    {
        vida = vidaRespawn;
        ativo = ativoRespawn;
        TrocarSprite(spriteAtualRespawn);

        if(ativo == true)
        {
            HitBoxAtiva(true);
        }
    }

    public override void Interagir(Player player)
    {
        switch(tipo)
        {
            case Tipo.Quebravel:
                dialogo.UpdateDialogueObject(listaDeDialogos.GetDialogueObject("CercaQuebravel"));
                dialogo.ShowDialogue(player.GeneralManager);
                break;
            case Tipo.Indestrutivel:
                dialogo.UpdateDialogueObject(listaDeDialogos.GetDialogueObject("CercaIndestrutivel"));
                dialogo.ShowDialogue(player.GeneralManager);
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
        HitBoxAtiva(false);
        ativo = false;
    }

    private void HitBoxAtiva(bool ativo)
    {
        if(ativo == true)
        {
            boxCollider2D.enabled = true;
            hitBoxTiro.enabled = true;

            boxCollider2D.isTrigger = false;
            generalManager.PathfinderManager.EscanearPathfinder(boxCollider2D);
        }
        else
        {
            boxCollider2D.isTrigger = true;
            generalManager.PathfinderManager.EscanearPathfinder(boxCollider2D);

            boxCollider2D.enabled = false;
            hitBoxTiro.enabled = false;
        }
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
                boxCollider2D.size = new Vector2(1, 0.9f);
                boxCollider2D.offset = new Vector2(0, 0.45f);
                hitBoxTiro.size = new Vector2(1, 1.5f);
                hitBoxTiro.offset = new Vector2(0, 2.25f);
                break;
            case Direcao.Vertical:
                boxCollider2D.size = new Vector2(0.2f, 1);
                boxCollider2D.offset = new Vector2(0, 0.5f);
                hitBoxTiro.size = new Vector2(0.2f, 3f);
                hitBoxTiro.offset = new Vector2(0, 1.5f);
                break;
        }
    }

    public void ArrumarPosicao(Cerca[] cercas)
    {
        Iniciar();

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
