using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    //Managers
    private ObjectManagerScript objectManager;
    private PauseManagerScript pauseManager;
    private BulletCreator bulletCreator;
    private DialogueUI dialogueUI;

    //Componentes
    private Rigidbody2D rb;
    private Movement movement;
    private AnimacaoJogador animacao;
    private InteragirScript interacaoHitBox;
    private AtaqueFisico ataqueHitBox;
    private Sound sound;
    private PontaArma pontaArma;
    private Inventario inventario;
    private InventarioMissao inventarioMissao;

    //Enums
    public enum ModoMovimento { Normal, AndandoSorrateiramente, Strafing };
    public enum Estado { Normal, TomandoDano, Atacando, UsandoItem, Morto };

    //Variaveis
    public override int vida { get; protected set; }
    public int vidaInicial;

    public Direcao direcaoMovimento;

    private float raioPassos;

    //Variaveis de controle
    public Vector3 posAnterior;

    public ModoMovimento modoMovimento;
    public Estado estado;

    private float tempoImune;
    private float tempoImuneMax;
    private bool imune;
    private bool collisionState;
    private float tempoSoftlock,
                  tempoSoftlockMax;

    //Variaveis de respawn
    private Vector2 posicaoRespawn;
    private Direcao direcaoRespawn;

    //Retirar
    public float distanciaTiroY;

    //Getter
    public DialogueUI DialogueUI => dialogueUI;

    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();
        pauseManager = FindObjectOfType<PauseManagerScript>();
        bulletCreator = FindObjectOfType<BulletCreator>();
        dialogueUI = FindObjectOfType<DialogueUI>();

        //Componentes
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        animacao = transform.GetComponent<AnimacaoJogador>();
        interacaoHitBox = GetComponentInChildren<InteragirScript>();
        ataqueHitBox = GetComponentInChildren<AtaqueFisico>();
        sound = FindObjectOfType<Sound>();
        pontaArma = GetComponentInChildren<PontaArma>();
        inventario = GetComponent<Inventario>();
        inventarioMissao = GetComponent<InventarioMissao>();


        //Variaveis
        vida = vidaInicial;
        raioPassos = 1.5f;

        //Variaveis de controle
        posAnterior = transform.position;

        modoMovimento = ModoMovimento.Normal;
        estado = Estado.Normal;

        tempoImune = 0;
        tempoImuneMax = 1.5f;
        imune = false;
        collisionState = false;

        tempoSoftlock = 0;
        tempoSoftlockMax = 10f;

        //Retirar
        distanciaTiroY = 1f;

        SetRespawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseManager.JogoPausado == false && estado != Estado.Morto)
        {
            if (imune)
            {
                animacao.Piscar();
                tempoImune += Time.deltaTime;
                if (tempoImune > tempoImuneMax)
                {
                    animacao.SetarVisibilidade(true);
                    imune = false;
                    tempoImune = 0;
                }
            }
            else if (collisionState)
            {
                foreach (Enemy inimigo in objectManager.listaInimigos)
                {
                    Physics2D.IgnoreCollision(inimigo.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                }
                collisionState = false;
            }

            //Debug.Log("\nPosicao Anter: " + posAnterior + ", Posicao Atual: " + transform.position + ", Posicao diferente: " + PosicaoDiferente(posAnterior, transform.position));
            animacao.AtualizarDirecao(direcao, direcaoMovimento);
            Animar();
            movement.Mover();

            ImpedirSoftlock();
        }
    }

    private void FixedUpdate()
    {
        posAnterior = transform.position;
    }

    private void SetRespawn()
    {
        posicaoRespawn = transform.position;
        direcaoRespawn = direcao;
    }

    public void SetRespawn(Vector2 posicao, Direcao direcao)
    {
        posicaoRespawn = posicao;
        direcaoRespawn = direcao;
    }

    public void Respawn()
    {
        vida = vidaInicial;
        transform.position = posicaoRespawn;
        direcao = direcaoRespawn;
        movement.ZerarVelocidade();

        ResetarVariaveisDeControle();
    }

    private void ResetarVariaveisDeControle()
    {
        posAnterior = transform.position;
        modoMovimento = ModoMovimento.Normal;
        estado = Estado.Normal;
        movement.canMove = true;
        tempoImune = 0;
        imune = false;
        collisionState = false;
        tempoSoftlock = 0;
    }

    private void Animar()
    {
        switch(estado)
        {
            case Estado.Normal:
                if (((rb.velocity.x == 0 && rb.velocity.y == 0) || !(PosicaoDiferente(posAnterior, transform.position))) && animacao.GetAnimacaoAtual() != "Idle")
                {
                    animacao.TrocarAnimacao("Idle");
                }
                else if ((rb.velocity.x != 0 || rb.velocity.y != 0) && PosicaoDiferente(posAnterior, transform.position))
                {
                    if (modoMovimento == ModoMovimento.AndandoSorrateiramente)
                    {
                        if (animacao.GetAnimacaoAtual() != "AndandoSorrateiramente")
                        {
                            animacao.TrocarAnimacao("AndandoSorrateiramente");
                        }
                    }
                    else if (animacao.GetAnimacaoAtual() != "Andando")
                    {
                        animacao.TrocarAnimacao("Andando");
                    }
                }
                break;

            case Estado.TomandoDano:
                if (animacao.GetAnimacaoAtual() != "TomandoDano")
                {
                    animacao.TrocarAnimacao("TomandoDano");
                }
                break;

            case Estado.Atacando:
                if (animacao.GetAnimacaoAtual() != "Atacando")
                {
                    animacao.TrocarAnimacao("Atacando");
                }
                break;

            case Estado.UsandoItem:
                if (animacao.GetAnimacaoAtual() != inventario.itemAtual.GetNomeAnimacao() + "Usando")
                {
                    animacao.AtualizarArmaBracos("");
                    animacao.TrocarAnimacao(inventario.itemAtual.GetNomeAnimacao() + "Usando");
                }
                break;
        }
    }

    private bool PosicaoDiferente(Vector3 posAnterior, Vector3 posAtual)
    {
        return Mathf.Abs(posAtual.x - posAnterior.x) > 0.01f || Mathf.Abs(posAtual.y - posAnterior.y) > 0.01f;
    }

    public void FinalizarKnockback()
    {
        animacao.TrocarAnimacao("Idle");
    }

    public void Interagir()
    {
        if (estado == Estado.Normal)
        {
            interacaoHitBox.Interagir(this, direcao);
        }
    }

    public BoxCollider2D GetHitBoxInteracao()
    {
        interacaoHitBox.AtualizarHitBox(direcao);
        return interacaoHitBox.GetBoxCollider2D();
    }

    public ObjectManagerScript GetObjectManager()
    {
        return objectManager;
    }

    public void Atacar()
    {
        if (estado == Estado.Normal)
        {
            estado = Estado.Atacando;
            movement.canMove = false;
            animacao.AtualizarArmaBracos("");
        }
    }

    public void AtaqueHitBox()
    {
        ataqueHitBox.Atacar(direcao, 1, 0.8f, 1.1f, 0.87f);
    }

    public void FinalizarAnimacao()
    {
        estado = Estado.Normal;
        movement.canMove = true;
    }

    public void Atirar()
    {
        if (estado == Estado.Normal)
        {
            GerarSom(inventario.armaSlot1.RaioTiro, true);
            inventario.armaSlot1.Atirar(gameObject, bulletCreator);
            animacao.AtualizarArmaBracos(inventario.armaSlot1.nomeVisual);
        }
    }

    public void AtualizarArma()
    {
        animacao.AtualizarArmaBracos(inventario.armaSlot1.nomeVisual);
    }

    public void AdicionarAoInventario(Item item)
    {
        inventario.Add(item);
    }

    public void RemoverDoInventario(Item item)
    {
        inventario.Remove(item);
    }

    public void AdicionarAoInventarioMissao(Item item)
    {
        inventarioMissao.Add(item);
    }

    public void RemoverDoInventarioMissao(Item item)
    {
        inventarioMissao.Remove(item);
    }

    public void UsarItem(Item item)
    {
        inventario.UsarItemAtual();
    }

    public void UsarItemAtalho(int atalho)
    {
        //UsarItem(atalho[atalho])
        inventario.UsarItemAtual();
    }

    public void AnimacaoItem(Item item)
    {
        estado = Estado.UsandoItem;
        movement.canMove = false;
        inventario.SetarItemAtual(item);
    }

    public void UsarItemGameplay()
    {
        inventario.itemAtual.UsarNaGameplay(this);
    }

    public override void TomarDano(int _dano, float _horizontal, float _vertical, float _knockBack)
    {
        if (!imune && estado != Estado.Morto)
        {
            if(dialogueUI.IsOpen == true)
            {
                dialogueUI.ForcedCloseDialogueBox();
            }

            if (vida <= 0)
            {
                Morrer();
            }
            else
            {
                vida -= _dano;

                imune = true;
                tempoImune = 0;
                estado = Estado.TomandoDano;
                KnockBack(_horizontal,_vertical,_knockBack);
            }
        }
    }

    public override void KnockBack(float _horizontal, float _vertical,float _knockBack)
    {
        movement.KnockBack(_horizontal, _vertical, _knockBack);
    }

    private void Morrer()
    {
        estado = Estado.Morto;
        movement.canMove = false;
        movement.ZerarVelocidade();
        animacao.AtualizarArmaBracos("");
        animacao.TrocarAnimacao("Morto");
    }

    public void GerarSom(float raio, bool somTiro)
    {
        if(somTiro == false)
        {
            raio = raioPassos;
        }
        sound.GerarSom(this, raio, somTiro);
    }

    public void ChangeDirection(string lado)
    {
        switch (lado)
        {
            case "Esquerda":
                direcao = Direcao.Esquerda;
                break;
            case "Direita":
                direcao = Direcao.Direita;
                break;
            case "Cima":
                direcao = Direcao.Cima;
                break;
            case "Baixo":
                direcao = Direcao.Baixo;
                break;
        }
        pontaArma.AtualizarPontaArma(direcao);
    }

    public void ChangeDirectionMovement(string lado)
    {
        switch (lado)
        {
            case "Esquerda":
                direcaoMovimento = Direcao.Esquerda;
                break;
            case "Direita":
                direcaoMovimento = Direcao.Direita;
                break;
            case "Cima":
                direcaoMovimento = Direcao.Cima;
                break;
            case "Baixo":
                direcaoMovimento = Direcao.Baixo;
                break;
        }
    }

    private void ImpedirSoftlock()
    {
        if (estado == Estado.TomandoDano || estado == Estado.Atacando || estado == Estado.UsandoItem)
        {
            tempoSoftlock += Time.deltaTime;

            if(tempoSoftlock >= tempoSoftlockMax)
            {
                FinalizarAnimacao();
            }
        }
        else
        {
            tempoSoftlock = 0;
        }
    }

    private void changeCollision(Collision2D collision, bool ignorarColisao)
    {
        Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), ignorarColisao);
        collisionState = ignorarColisao;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (imune)
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                changeCollision(collision, true);
            }
        }
    }
}
