using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    public override int vida { get; protected set; }
    private DirectionHitbox directionHitbox;
    private Rigidbody2D rb;
    private ItemDirectionHitbox itemDirectionHitbox;
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private BoxCollider2D boxCollider2D;
    public Enemy[] enemies;

    private PontaArma pontaArma;
    private AnimacaoJogador animacao;

    public Direcao direcaoMovimento;

    public int initialLife;
    public bool andandoSorrateiramente;

    [SerializeField] private float time = 0.0F;
    [SerializeField] private float timeMax = 0;
    [SerializeField] private bool imune = false;

    bool collisionState;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        movement = GetComponent<Movement>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        directionHitbox = GetComponentInChildren<DirectionHitbox>();
        itemDirectionHitbox = GetComponentInChildren<ItemDirectionHitbox>();
        rb = GetComponent<Rigidbody2D>();
        vida = initialLife;
        andandoSorrateiramente = false;

        pontaArma = GetComponentInChildren<PontaArma>();
        animacao = transform.GetComponent<AnimacaoJogador>();

        enemies = FindObjectsOfType<Enemy>();//pegando todos os inmigos
    }

    // Update is called once per frame
    void Update()
    {
        if (imune)
        {
            time += Time.deltaTime;
            if (time > timeMax)
            {
                imune = false;
                timeMax = 0.0F;
                time = 0;
            }
        }
        else if(collisionState)
        {
            foreach (Enemy enemy in enemies)
            {
                Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            }
            collisionState = false;
        }

        movement.Mover();

        animacao.AtualizarDirecao(direcao, direcaoMovimento);
        Animar();
    }

    private void Animar()
    {
        if((rb.velocity.x == 0 && rb.velocity.y == 0) && animacao.GetAnimacaoAtual() != "Idle")
        {
            animacao.TrocarAnimacao("Idle");
        }
        else if ((rb.velocity.x != 0 || rb.velocity.y != 0))
        {
            if(andandoSorrateiramente == true)
            {
                if(animacao.GetAnimacaoAtual() != "AndandoSorrateiramente")
                {
                    animacao.TrocarAnimacao("AndandoSorrateiramente");
                }
            }
            else if (animacao.GetAnimacaoAtual() != "Andando")
            {
                animacao.TrocarAnimacao("Andando");
            }
        }
    }

    public void curar(int _cura)
    {
        Debug.Log(vida);
        vida = +_cura;
        Debug.Log(vida);
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
        directionHitbox.ChangeDirection(direcao);
        itemDirectionHitbox.ChangeDirection(direcao);
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

    public override void TomarDano(int _dano, float _horizontal, float _vertical, float _knockBack)
    {
        if (!imune)
        {
            if (vida <= 0)
            {
                //Destroy(gameObject);
                Debug.Log("to morto!");
            }

            else
            {
                StartCoroutine(Piscar());
                vida -= _dano;

                imune = true;
                timeMax = 0.3F;
                time = 0;
                KnockBack(_horizontal,_vertical,_knockBack);
            }
        }
    }

    public override void KnockBack(float _horizontal, float _vertical,float _knockBack)
    {

        movement.KnockBack(_horizontal, _vertical,_knockBack);
    }

    public override IEnumerator Piscar()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
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
