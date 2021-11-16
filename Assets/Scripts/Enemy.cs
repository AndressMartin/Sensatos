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

    bool tiroColldown=false;
    float timeCooldwon;
    float timeCooldownTiro;
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

         timeCooldwon = 0;
         timeCooldownTiro = 0.5f;
    }

    // Update is called once per frame
    void Update()
    { 
        AllEnemySubClass();
    }
    void AllEnemySubClass()
    {
        if (!dead)
        {
            //Debug.Log("Inimigo Vel X: " + enemyMove.velX + ", Vel Y: " + enemyMove.velY);
            animacao.AtualizarDirecao(direcao, direcao);
            Animar();
            enemyMove.Main();
            enemyVision.Main();
            TiroColldown();
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
        TrocarDirecaoAtaque(FindObjectOfType<Player>().transform.position);
        if (!tiroColldown)
        {
            inventario.armaSlot1.AtualizarBulletCreator(FindObjectOfType<BulletCreator>());
            inventario.armaSlot1.Usar(gameObject);
            animacao.AtualizarArmaBracos(inventario.armaSlot1.nomeVisual);
            tiroColldown = true;
        }
        
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
    public void TrocarDirecaoAtaque(Vector3 alvo)
    {
        float DistanciaX,
              DistanciaY;

        DistanciaX = alvo.x - transform.position.x;
        DistanciaY = alvo.y - transform.position.y;

        if (DistanciaY < 0)
        {
            ChangeDirection(EntityModel.Direcao.Baixo);
        }
        else
        {
            ChangeDirection(EntityModel.Direcao.Cima);
        }

        if (DistanciaX > 0.6)
        {
            ChangeDirection(EntityModel.Direcao.Direita);
        }
        else if (DistanciaX < -0.6)
        {
            ChangeDirection(EntityModel.Direcao.Esquerda);
        }
    }
    public override void KnockBack(float _horizontal, float _vertical,float _knockBack)
    {
        enemyMove.KnockBack(_horizontal, _vertical,_knockBack);
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
       
    }
    private void TiroColldown()
    {
        if (tiroColldown)
        {
            timeCooldwon += Time.deltaTime;
            if (timeCooldwon > timeCooldownTiro)
            {
                tiroColldown = false;
                timeCooldwon = 0;
            }
        }
    }
}
