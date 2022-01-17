using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityModel
{
    //Managers
    private ObjectManagerScript objectManager;
    private PauseManagerScript pauseManager;
    private BulletManagerScript bulletManager;

    //Componentes
    private PontaArmaScript pontaArma;
    private AnimacaoJogador animacao;
    private InventarioEnemy inventario;
    private EnemyMovement enemyMovement;
    private EnemyVisionScript enemyVision;

    private Player player;

    //Variaveis
    public override int vida { get; protected set; }
    [SerializeField] private int vidaInicial;

    public enum Estado { Normal, TomandoDano };
    private Estado estado;

    public bool morto;
    public bool playerOnAttackRange;
    public bool vendoPlayer;
    public bool vendoPlayerCircular;

    float tempoTiro;

    private float knockBackTrigger;
    private float knockBackTriggerMax;

    //Variaveis de respawn
    private bool mortoRespawn;
    private Vector2 posicaoRespawn;
    private Direcao direcaoRespawn;
    private EnemyMovement.ModoPatrulha modoPatrulhaRespawn;

    //Getters
    public Estado GetEstado => estado;

    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();
        pauseManager = FindObjectOfType<PauseManagerScript>();
        bulletManager = FindObjectOfType<BulletManagerScript>();

        //Se adicionar a lista de inimigos do ObjectManager
        objectManager.adicionarAosInimigos(this);

        //Componentes
        enemyVision = GetComponentInChildren<EnemyVisionScript>();
        enemyMovement = GetComponent<EnemyMovement>();
        inventario = GetComponent<InventarioEnemy>();
        pontaArma = GetComponentInChildren<PontaArmaScript>();
        animacao = GetComponent<AnimacaoJogador>();

        player = FindObjectOfType<Player>();

        //Variaveis
        vida = vidaInicial;

        estado = Estado.Normal;

        morto = false;

        tempoTiro = 0;

        knockBackTrigger = 0;
        knockBackTriggerMax = 10;

        SetRespawnInicial();
    }

    void Update()
    { 
        if(pauseManager.JogoPausado == false)
        {
            AllEnemySubClass();
        }
    }
    void VariaveisAtualizamTodoFrame()
    {
        vendoPlayer = enemyVision.vendoPlayer;
        vendoPlayerCircular=enemyVision.vendoPlayerCircular;
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
            ChangeDirection(direcaoRespawn);
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
        playerOnAttackRange = false;
        tempoTiro = 0;
        enemyMovement.ResetarVariaveisDeControle();
        enemyVision.ResetarVariaveisDeControle();
    }

    void AllEnemySubClass()
    {
        if (!morto)
        {
            VariaveisAtualizamTodoFrame();
            //Debug.Log("Inimigo Vel X: " + enemyMove.velX + ", Vel Y: " + enemyMove.velY);
            animacao.AtualizarDirecao(direcao, direcao);
            Animar();
            enemyMovement.Main();
            enemyVision.Main();

            CadenciaTiro();
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

    public void FinalizarAnimacao()
    {
        estado = Estado.Normal;
    }

    public void Atirar()
    {
        TrocarDirecaoAtaque(player.transform.position);

        if (tempoTiro <= 0)
        {
            inventario.ArmaSlot.Atirar(this, bulletManager, pontaArma.transform.position, DirecaoPlayer(player), Alvo.Player);
            animacao.AtualizarArmaBracos(inventario.ArmaSlot.NomeAnimacao);

            SetCadenciaTiro(inventario.ArmaSlot.GetStatus.CadenciaDosTiros);
        }   
    }

    private Vector2 DirecaoPlayer(Player player)
    {
        Vector3 posicaoPlayer = player.transform.position;
        Vector3 direcaoPlayer = posicaoPlayer - transform.position;
        direcaoPlayer.Normalize();

        return direcaoPlayer;
    }

    public void Die()
    {
        morto = true;
        enemyMovement.ZerarVelocidade();
        Debug.Log("to morto");
    }
    public void stealthKill()
    {
        Die();
    }

    public void TomarDanoFisico(int _dano, float _knockBack, Vector2 _direcaoKnockBack)
    {
        // mudar pra quando tiver variavel de inimgo vendo
        if (!enemyVision.vendoPlayer)//ageitar
        {
            stealthKill();
        }
        else
        {
            TomarDano(_dano, _knockBack, 0, _direcaoKnockBack);
        }
    }

    public override void TomarDano(int _dano, float _knockBack, float _knockBackTrigger, Vector2 _direcaoKnockBack)
    {
        if(estado == Estado.Normal)
        {
            if (vida <= 0)
            {
                Die();
            }
            else
            {
                vida -= _dano;

                knockBackTrigger += _knockBackTrigger;

                if (knockBackTrigger >= knockBackTriggerMax)
                {
                    estado = Estado.TomandoDano;
                    KnockBack(_knockBack, _direcaoKnockBack);
                }
            }
        } 
    }

    public override void KnockBack(float _knockBack, Vector2 _direcaoKnockBack)
    {
        enemyMovement.KnockBack(_knockBack, _direcaoKnockBack);

    }

    public void ChangeDirection(Direcao _direction)
    {
        direcao = _direction;
        AtualizarPontaDaArma();
    }

    private void DefinirPontaDaArma(out float offSetX, out float offSetY)
    {
        offSetX = 0;
        offSetY = 0;

        switch (inventario.ArmaSlot.NomeAnimacao)
        {
            case "Arma1":
                switch (direcao)
                {
                    case Direcao.Baixo:
                        offSetX = -0.284f;
                        offSetY = 0.787f;
                        break;

                    case Direcao.Esquerda:
                        offSetX = -0.486f;
                        offSetY = 1.224f;
                        break;

                    case Direcao.Cima:
                        offSetX = 0.283f;
                        offSetY = 1.56f;
                        break;

                    case Direcao.Direita:
                        offSetX = 0.486f;
                        offSetY = 1.224f;
                        break;
                }
                break;

            case "Arma2":
                switch (direcao)
                {
                    case Direcao.Baixo:
                        offSetX = -0.188f;
                        offSetY = 0.62f;
                        break;

                    case Direcao.Esquerda:
                        offSetX = -0.715f;
                        offSetY = 1.227f;
                        break;

                    case Direcao.Cima:
                        offSetX = 0.157f;
                        offSetY = 1.727f;
                        break;

                    case Direcao.Direita:
                        offSetX = 0.715f;
                        offSetY = 1.227f;
                        break;
                }
                break;

            default:
                offSetX = 0;
                offSetY = 0;
                break;
        }
    }

    private void AtualizarPontaDaArma()
    {
        DefinirPontaDaArma(out float offSetX, out float offSetY);
        pontaArma.AtualizarPontaArma(offSetX, offSetY);
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
    
    private void OnCollisionStay2D(Collision2D collision)
    {
       
    }

    private void CadenciaTiro()
    {
        if (tempoTiro > 0)
        {
            tempoTiro -= Time.deltaTime;

            if (tempoTiro < 0)
            {
                tempoTiro = 0;
            }
        }
    }

    public void SetCadenciaTiro(float cadenciaDosTiros)
    {
        tempoTiro = cadenciaDosTiros;
    }

    public void EscutarSom(Player player, bool somTiro)
    {
        enemyMovement.EscutarSom(player, somTiro);
    }
}
