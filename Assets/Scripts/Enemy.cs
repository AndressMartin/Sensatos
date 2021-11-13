using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityModel
{
    public override int vida { get; protected set; }
    public enum ModoPatrulha { ronda,parado };
    public ModoPatrulha modoPatrulha;

    private SpriteRenderer spriteRenderer;
    private Inventario inventario;
    private EnemyMove enemyMove;
    private EnemyVision enemyVision;
    
    [SerializeField] private int pontosVida;
    float horizontal;
    float vertical;
    float enemyDirection;
    public bool dead = false;



    // Start is called before the first frame update
    void Start()
    {
        enemyVision = GetComponentInChildren<EnemyVision>();
        enemyMove = GetComponent<EnemyMove>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        inventario = GetComponent<Inventario>();
        vida = pontosVida;

    }

    // Update is called once per frame
    void Update()
    {
        if(direction==Direction.Esquerda && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
        if (direction == Direction.Direita && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        /* if (Input.GetKeyDown(KeyCode.W))
         {
             direction = Direction.Cima;
         }
         if (Input.GetKeyDown(KeyCode.S))
         {
             direction = Direction.Baixo;
         }
         if (Input.GetKeyDown(KeyCode.A))
         {
             direction = Direction.Esquerda;
         }
         if (Input.GetKeyDown(KeyCode.D))
         {
             direction = Direction.Direita;
         }*/

        AllEnemySubClass();
    }
    void AllEnemySubClass()
    {
        if (!dead)
        {
            enemyMove.Main();
            enemyVision.Main();
        } 
    }

    public void UseItem()
    {
        inventario.UsarItemAtual();
    }
    public void die()
    {
        dead = true;
        Debug.Log("to morto");
    }
    public void stealthKill()
    {
        dead = true;
        gameObject.SetActive(false);
    }

    public override void TomarDano(int _dano, float _horizontal, float _vertical,float _knockBack)
    {
       
        if (vida <= 0)
        {
            die();
        }
        else
        {
            StartCoroutine(Piscar());
            KnockBack(_horizontal, _vertical , _knockBack);
            vida -= _dano;
        }

        
    }
    public void ChangeDirection(Direction _direction)
    {
        direction = _direction;

    }
    public void ChangeHorizontal(float _direction)
    {
        horizontal = _direction;
    }
    public void ChangeVertical(float _direction)
    {
        vertical = _direction;
    }

    public override IEnumerator Piscar()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public override void KnockBack(float _horizontal, float _vertical,float _knockBack)
    {
        enemyMove.KnockBack(_horizontal, _vertical,_knockBack);
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
       
    }
}
