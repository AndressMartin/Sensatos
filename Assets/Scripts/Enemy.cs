using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityModel
{
    public override int vida { get; protected set; }

    public enum Estado {Normal, TomandoDano};
    public Estado estado;

    private PontaArma pontaArma;
    private AnimacaoJogador animacao;

    private Inventario inventario;
    private EnemyMove enemyMove;
    private EnemyVision enemyVision;
    
    [SerializeField] private int pontosVida;
    public bool dead = false;



    // Start is called before the first frame update
    void Start()
    {
        enemyVision = GetComponentInChildren<EnemyVision>();
        enemyMove = GetComponent<EnemyMove>();
        inventario = GetComponent<Inventario>();
        vida = pontosVida;

        pontaArma = GetComponentInChildren<PontaArma>();
        animacao = transform.GetComponent<AnimacaoJogador>();

        estado = Estado.Normal;
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("Inimigo Vel X: " + enemyMove.velX + ", Vel Y: " + enemyMove.velY);
            animacao.AtualizarDirecao(direcao, direcao);
            Animar();
            enemyMove.Main();
            enemyVision.Main();
        } 
    }

    private void Animar()
    {
        switch (estado)
        {
            case Estado.Normal:
                if ((enemyMove.velX == 0 && enemyMove.velY == 0) && animacao.GetAnimacaoAtual() != "Idle")
                {
                    animacao.TrocarAnimacao("Idle");
                }
                else if ((enemyMove.velX != 0 || enemyMove.velY != 0))
                {
                    /*
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
                    */
                    animacao.TrocarAnimacao("Andando");
                }
                break;

            case Estado.TomandoDano:
                if (animacao.GetAnimacaoAtual() != "TomandoDano")
                {
                    animacao.TrocarAnimacao("TomandoDano");
                }
                break;
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
            KnockBack(_horizontal, _vertical , _knockBack);
            vida -= _dano;
        }

        
    }
    public void ChangeDirection(Direcao _direction)
    {
        direcao = _direction;
        pontaArma.AtualizarPontaArma(direcao);
    }

    public override void KnockBack(float _horizontal, float _vertical,float _knockBack)
    {
        enemyMove.KnockBack(_horizontal, _vertical,_knockBack);
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
       
    }
}
