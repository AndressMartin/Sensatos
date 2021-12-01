using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    //Managers
    private ObjectManagerScript objectManager;
    private PauseManagerScript pauseManager;

    public override int vida { get; protected set; }

    private Rigidbody2D rb;
    private Movement movement;
    public Enemy[] enemies;

    private PontaArma pontaArma;
    private InteragirScript interacaoHitBox;
    private AtaqueFisico ataqueHitBox;
    private AnimacaoJogador animacao;
    private Inventario inventario;
    private InventarioMissao inventarioMissao;

    public Direcao direcaoMovimento;
    public Vector3 posAnterior;
    private Sound sound;

    private float tempoImunidade;

    public int initialLife;
    public float distanciaTiroY;
    public enum ModoMovimento {Normal, AndandoSorrateiramente, Strafing};
    public enum Estado {Normal, TomandoDano, Atacando, UsandoItem, Morto};

    public ModoMovimento modoMovimento;
    public Estado estado;

    [SerializeField] private float time = 0.0F;
    [SerializeField] private float timeMax = 0;
    [SerializeField] private bool imune = false;

    bool collisionState;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();
        pauseManager = FindObjectOfType<PauseManagerScript>();

        sound = FindObjectOfType<Sound>();
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();

        vida = initialLife;

        modoMovimento = ModoMovimento.Normal;
        estado = Estado.Normal;

        inventario = GetComponent<Inventario>();
        inventarioMissao = GetComponent<InventarioMissao>();

        posAnterior = transform.position;

        tempoImunidade = 1f;
        pontaArma = GetComponentInChildren<PontaArma>();
        interacaoHitBox = GetComponentInChildren<InteragirScript>();
        ataqueHitBox = GetComponentInChildren<AtaqueFisico>();
        animacao = transform.GetComponent<AnimacaoJogador>();

        enemies = FindObjectsOfType<Enemy>();//pegando todos os inmigos
        distanciaTiroY = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseManager.GetJogoPausado() == false && estado != Estado.Morto)
        {
            if (imune)
            {
                animacao.Piscar();
                time += Time.deltaTime;
                if (time > timeMax)
                {
                    animacao.SetarVisibilidade(true);
                    imune = false;
                    timeMax = 0.0F;
                    time = 0;
                }
            }
            else if (collisionState)
            {
                foreach (Enemy enemy in enemies)
                {
                    Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                }
                collisionState = false;
            }

            //Debug.Log("\nPosicao Anter: " + posAnterior + ", Posicao Atual: " + transform.position + ", Posicao diferente: " + PosicaoDiferente(posAnterior, transform.position));
            animacao.AtualizarDirecao(direcao, direcaoMovimento);
            Animar();
            movement.Mover();
        }
    }

    private void FixedUpdate()
    {
        posAnterior = transform.position;
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
            interacaoHitBox.Interagir(direcao);
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
            sound.ChangeColliderRadius(5);
            inventario.armaSlot1.AtualizarBulletCreator(FindObjectOfType<BulletCreator>());
            inventario.armaSlot1.Atirar(gameObject);
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
            if (vida <= 0)
            {
                Morrer();
            }

            else
            {
                vida -= _dano;

                imune = true;
                timeMax = tempoImunidade;
                time = 0;
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

    private void changeCollision(Collision2D collision, bool _onOff)
    {
        Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), _onOff);
        collisionState = _onOff;
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
