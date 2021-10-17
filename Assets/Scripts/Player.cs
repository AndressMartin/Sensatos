using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    public override int vida { get; protected set; }
    private DirectionHitbox directionHitbox;
    private ItemDirectionHitbox itemDirectionHitbox;
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private BoxCollider2D boxCollider2D;
    public Enemy[] enemies;


    public int initialLife; 

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
        vida = initialLife;


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
                direction = Direction.Esquerda;
                break;
            case "Direita":
                direction = Direction.Direita;
                break;
            case "Cima":
                direction = Direction.Cima;
                break;
            case "Baixo":
                direction = Direction.Baixo;
                break;
        }
        directionHitbox.ChangeDirection(direction);
        itemDirectionHitbox.ChangeDirection(direction);
    }

    public override void TomarDano(int _dano, float _horizontal, float _vertical)
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
                KnockBack(_horizontal,_vertical);
            }
        }
    }

    public override void KnockBack(float _horizontal, float _vertical)
    {

        movement.KnockBack(_horizontal, _vertical);
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
