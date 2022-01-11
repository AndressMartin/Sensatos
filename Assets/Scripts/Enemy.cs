using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityModel
{
    //Managers
    private ObjectManagerScript objectManager;
    private PauseManagerScript pauseManager;
    private BulletCreator bulletCreator;

    //Componentes
    private PontaArmaScript pontaArma;
    private AnimacaoJogador animacao;
    private Inventario inventario;
    private EnemyMovement enemyMovement;
    private EnemyVisionScript enemyVision;

    //Variaveis
    public override int vida { get; protected set; }
    [SerializeField] private int vidaInicial;

    public enum Estado { Normal, TomandoDano };
    public Estado estado;

    public bool morto;

    bool tiroColldown;
    float timeCooldwon;
    float timeCooldownTiro;

    [SerializeField] private float distanceCenter;
    [SerializeField] private float distanceY;

    //Variaveis de respawn
    private bool mortoRespawn;
    private Vector2 posicaoRespawn;
    private Direcao direcaoRespawn;
    private EnemyMovement.ModoPatrulha modoPatrulhaRespawn;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();
        pauseManager = FindObjectOfType<PauseManagerScript>();
        bulletCreator = FindObjectOfType<BulletCreator>();

        //Se adicionar a lista de inimigos do ObjectManager
        objectManager.adicionarAosInimigos(this);

        //Componentes
        enemyVision = GetComponentInChildren<EnemyVisionScript>();
        enemyMovement = GetComponent<EnemyMovement>();
        inventario = GetComponent<Inventario>();
        pontaArma = GetComponentInChildren<PontaArmaScript>();
        animacao = transform.GetComponent<AnimacaoJogador>();

        //Variaveis
        vida = vidaInicial;

        estado = Estado.Normal;

        morto = false;

        tiroColldown = false;
        timeCooldwon = 0;
        timeCooldownTiro = 0.5f;

        SetRespawnInicial();
    }

    // Update is called once per frame
    void Update()
    { 
        if(pauseManager.JogoPausado == false)
        {
            AllEnemySubClass();
        }
    }

    private void SetRespawnInicial()
    {
        posicaoRespawn = transform.position;
        direcaoRespawn = direcao;
        modoPatrulhaRespawn = enemyMovement.modoPatrulha;
    }

    public void SetRespawn()
    {
        mortoRespawn = morto;
        modoPatrulhaRespawn = enemyMovement.modoPatrulha;
    }

    public void Respawn()
    {
        morto = mortoRespawn;

        if(morto == false)
        {
            vida = vidaInicial;
            transform.position = posicaoRespawn;
            direcao = direcaoRespawn;
            enemyMovement.modoPatrulha = modoPatrulhaRespawn;

            enemyMovement.estado = EnemyMovement.Estado.Rotina;
            enemyMovement.stance = EnemyMovement.Stances.Patrolling;
            enemyMovement.fazerMovimentoAlerta = EnemyMovement.FazerMovimentoAlerta.NA;
            enemyMovement.ZerarVelocidade();


            ResetarVariaveisDeControle();
        }
    }

    private void ResetarVariaveisDeControle()
    {
        estado = Estado.Normal;
        tiroColldown = false;
        timeCooldwon = 0;
        enemyMovement.ResetarVariaveisDeControle();
        enemyVision.ResetarVariaveisDeControle();
    }

    void AllEnemySubClass()
    {
        if (!morto)
        {
            //Debug.Log("Inimigo Vel X: " + enemyMove.velX + ", Vel Y: " + enemyMove.velY);
            animacao.AtualizarDirecao(direcao, direcao);
            Animar();
            enemyMovement.Main();
            enemyVision.Main();
            TiroColldown();
        } 
    }

    private void Animar()
    {
        switch (estado)
        {
            case Estado.Normal:
                if ((enemyMovement.rb.velocity.x == 0 && enemyMovement.rb.velocity.y == 0) && animacao.GetAnimacaoAtual() != "Idle")
                {
                    animacao.TrocarAnimacao("Idle");
                }
                else if ((enemyMovement.rb.velocity.x != 0 || enemyMovement.rb.velocity.y != 0) && animacao.GetAnimacaoAtual() != "Andando")
                {
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
            inventario.armaSlot1.Atirar(this, bulletCreator);
            animacao.AtualizarArmaBracos(inventario.armaSlot1.nomeVisual);
            tiroColldown = true;
        }
        
    }
    public void die()
    {
        morto = true;
        enemyMovement.ZerarVelocidade();
        Debug.Log("to morto");
    }
    public void stealthKill()
    {
        die();
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
        pontaArma.AtualizarPontaArma(direcao, distanceCenter, distanceY);
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
        enemyMovement.KnockBack(_horizontal, _vertical,_knockBack);
        
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

    public void EscutarSom(Player player, bool somTiro)
    {
        enemyMovement.EscutarSom(player, somTiro);
    }
}
