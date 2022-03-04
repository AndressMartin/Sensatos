using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : EntityModel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private PontaArmaScript pontaArma;
    private AnimacaoJogador animacao;
    private InventarioEnemy inventario;
    private EnemyMovement enemyMovement;
    private EnemyVisionScript enemyVision;
    private EnemyAttackRange enemyAttackRange;
    private BoxCollider2D colisao;
    private BoxCollider2D hitboxDano;
    private Rigidbody2D rb;
    private IAEnemy ia_Enemy;
    private Player player;

    //Variaveis
    [SerializeField] private int vidaInicial;
    [SerializeField] private float velocidadeAnimacaoCorrendo;

    public enum Estado { Normal, TomandoDano };
    private Estado estado;

    private bool morto;
    private bool playerOnAttackRange;

    private float tempoTiro;

    private float knockBackTrigger;
    private float knockBackTriggerMax;
    private float knockBackTempo;

    private Vector2 vetorVelocidade;
    private Vector2 posAnterior;

    //Variaveis de respawn
    private bool mortoRespawn;
    private Vector2 posicaoRespawn;
    private Direcao direcaoRespawn;

    private bool iniciado = false;

    //Getters
    public GeneralManagerScript GeneralManager => generalManager;
    public AnimacaoJogador Animacao => animacao;
    public Estado GetEstado => estado;
    public bool Morto => morto;
    public bool PlayerOnAttackRange => playerOnAttackRange;
    public Player GetPlayer => player;
    public InventarioEnemy GetInventarioEnemy => inventario;
    public EnemyMovement GetEnemyMovement => enemyMovement;
    public IAEnemy GetIAEnemy => ia_Enemy;
    public Vector2 VetorVelocidade => vetorVelocidade;

    //Setters
    public void SetMortoRespawn(bool morto)
    {
        mortoRespawn = morto;
    }
    public void SetPlayerOnAttackRange(bool ativo)
    {
        playerOnAttackRange = ativo;
    }

    void Start()
    {
        Iniciar();
    }

    private void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Se adicionar a lista de inimigos do ObjectManager
        generalManager.ObjectManager.AdicionarAosInimigos(this);

        //Componentes
        enemyMovement = GetComponent<EnemyMovement>();
        ia_Enemy = GetComponent<IAEnemy>();
        inventario = GetComponent<InventarioEnemy>();
        pontaArma = GetComponentInChildren<PontaArmaScript>();
        enemyVision = GetComponentInChildren<EnemyVisionScript>();
        enemyAttackRange = GetComponentInChildren<EnemyAttackRange>();
        animacao = GetComponent<AnimacaoJogador>();
        colisao = GetComponent<BoxCollider2D>();
        hitboxDano = transform.Find("HitboxDano").GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        player = FindObjectOfType<Player>();

        //Variaveis
        vida = vidaInicial;

        estado = Estado.Normal;

        morto = false;

        tempoTiro = 0;

        knockBackTrigger = 0;
        knockBackTriggerMax = 10;
        knockBackTempo = 0;

        vetorVelocidade = Vector2.zero;
        posAnterior = transform.position;

        ia_Enemy.Iniciar();
        enemyMovement.Iniciar();
        animacao.Iniciar();

        SetRespawnInicial();

        iniciado = true;
    }

    void Update()
    { 
        if(generalManager.PauseManager.JogoPausado == false)
        {
            RotinasDoInimigo();
        }
    }

    private void FixedUpdate()
    {
        if((Vector2)transform.position != posAnterior)
        {
            generalManager.PathfinderManager.EscanearPathfinder(colisao);
        }

        vetorVelocidade = (Vector2)transform.position - posAnterior;
        posAnterior = transform.position;
    }

    private void SetRespawnInicial()
    {
        posicaoRespawn = transform.position;
        direcaoRespawn = direcao;
    }

    public void SetRespawn()
    {
        mortoRespawn = morto;
        ia_Enemy.SetRespawn();
    }

    public void Respawn()
    {
        morto = mortoRespawn;

        if(morto == false)
        {
            vida = vidaInicial;
            transform.position = posicaoRespawn;
            ChangeDirection(direcaoRespawn);

            ia_Enemy.Respawn();
            enemyMovement.ZerarVelocidade();

            enemyVision.gameObject.SetActive(true);
            enemyAttackRange.gameObject.SetActive(true);

            colisao.enabled = true;
            hitboxDano.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;

            animacao.SetVelocidade(1);

            ResetarVariaveisDeControle();
        }
        else
        {
            Desaparecer();
        }
    }

    private void ResetarVariaveisDeControle()
    {
        estado = Estado.Normal;
        playerOnAttackRange = false;
        tempoTiro = 0;

        vetorVelocidade = Vector2.zero;
        posAnterior = transform.position;

        enemyMovement.ResetarVariaveisDeControle();
        enemyVision.ResetarVariaveisDeControle();
    }

    public void SerSpawnado(Vector2 _pontoSpawn)
    {
        Iniciar();

        this.gameObject.SetActive(false);
    }

    void RotinasDoInimigo()
    {
        if (!morto)
        {
            //Debug.Log("Inimigo Vel X: " + enemyMove.velX + ", Vel Y: " + enemyMove.velY);
            Animar();

            if(estado == Estado.Normal)
            {
                ia_Enemy.Main();
            }

            enemyMovement.Main();
            enemyVision.Main();

            KnockBackTriggerTempo();

            CadenciaTiro();
        }
    }

    private void Animar()
    {
        animacao.AtualizarDirecao(direcao, direcao);

        if(animacao.AnimacaoAtual == "Andando" && enemyMovement.GetVelocidade > enemyMovement.GetVelocidadeModoNormal)
        {
            animacao.SetVelocidade(velocidadeAnimacaoCorrendo);
        }
        else
        {
            animacao.SetVelocidade(1);
        }

        switch (estado)
        {
            case Estado.Normal:
                if ((vetorVelocidade.x == 0 && vetorVelocidade.y == 0) && animacao.AnimacaoAtual != "Idle")
                {
                    animacao.TrocarAnimacao("Idle");
                }
                else if ((vetorVelocidade.x != 0 || vetorVelocidade.y != 0) && animacao.AnimacaoAtual != "Andando")
                {
                    animacao.TrocarAnimacao("Andando");
                }
                break;

            case Estado.TomandoDano:
                if (animacao.AnimacaoAtual != "TomandoDano")
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

    public bool Atirar()
    {
        TrocarDirecaoAtaque(player.transform.position);

        if (tempoTiro <= 0)
        {
            inventario.ArmaSlot.Atirar(this, generalManager.BulletManager, pontaArma.transform.position, DirecaoPlayer(player), Alvo.Player);
            animacao.AtualizarArmaBracos(inventario.ArmaSlot.NomeAnimacao);

            SetCadenciaTiro(inventario.ArmaSlot.GetStatus.CadenciaDosTiros);
            return true;
        }   
        return false;
    }

    private Vector2 DirecaoPlayer(Player player)
    {
        Vector3 posicaoPlayer = player.transform.position;
        Vector3 direcaoPlayer = posicaoPlayer - transform.position;
        direcaoPlayer.Normalize();

        return direcaoPlayer;
    }

    public void TomarDanoFisico(int _dano, float _knockBack, float _knockBackTrigger, Direcao _direcao)
    {
        if (ia_Enemy.GetEstadoDeteccaoPlayer != IAEnemy.EstadoDeteccaoPlayer.PlayerDetectado)
        {
            StealthKill(_direcao);
        }
        else
        {
            Debug.Log("Entrei");
            TomarDano(_dano, _knockBack, _knockBackTrigger, VetorDirecao(_direcao));
        }
    }

    public override void TomarDano(int _dano, float _knockBack, float _knockBackTrigger, Vector2 _direcaoKnockBack)
    {
        if(estado == Estado.Normal)
        {
            if (vida <= 0)
            {
                animacao.SetVelocidade(1);
                animacao.AtualizarArmaBracos("");
                animacao.TrocarAnimacao("Morto");
                Morrer();
            }
            else
            {
                vida -= _dano;
                ia_Enemy.ReceberDano();

                knockBackTrigger += _knockBackTrigger;
                knockBackTempo = 6;

                if (knockBackTrigger >= knockBackTriggerMax)
                {
                    knockBackTrigger = 0;
                    estado = Estado.TomandoDano;
                    KnockBack(_knockBack, _direcaoKnockBack);
                }
            }
        } 
    }

    public override void KnockBack(float _knockBack, Vector2 _direcaoKnockBack)
    {
        enemyMovement.KnockBack(_knockBack, _direcaoKnockBack);
        Debug.Log("toamndo knock "+_knockBack+" Vetor "+_direcaoKnockBack);
    }

    private void KnockBackTriggerTempo()
    {
        if (knockBackTempo > 0)
        {
            knockBackTempo -= Time.deltaTime;

            if (knockBackTempo <= 0)
            {
                knockBackTempo = 0;
                knockBackTrigger = 0;
            }
        }
    }

    public void Morrer()
    {
        morto = true;
        generalManager.EnemyManager.PerdiVisaoInimigo();
        enemyMovement.ZerarVelocidade();
        ia_Enemy.DesativarIconeDeVisao();

        enemyVision.gameObject.SetActive(false);
        enemyAttackRange.gameObject.SetActive(false);

        rb.bodyType = RigidbodyType2D.Kinematic;

        animacao.AtualizarDirecao(direcao, direcao);
    }

    public void AnimacaoDesaparecendo()
    {
        animacao.SetVelocidade(1);
        animacao.AtualizarArmaBracos("");
        animacao.TrocarAnimacao("Desaparecendo");
    }

    public void Desaparecer()
    {
        animacao.SetVelocidade(1);
        animacao.AtualizarArmaBracos("");
        animacao.TrocarAnimacao("Vazio");

        colisao.enabled = false;
        hitboxDano.enabled = false;
    }

    public void StealthKill(Direcao direcao)
    {
        switch (direcao)
        {
            case Direcao.Baixo:
                ChangeDirection(Direcao.Cima);
                break;

            case Direcao.Esquerda:
                ChangeDirection(Direcao.Direita);
                break;

            case Direcao.Direita:
                ChangeDirection(Direcao.Esquerda);
                break;

            case Direcao.Cima:
                ChangeDirection(Direcao.Baixo);
                break;
        }

        animacao.SetVelocidade(1);
        animacao.AtualizarArmaBracos("");
        animacao.TrocarAnimacao("MortoSorrateiramente");
        Morrer();
    }

    public void ChangeDirection(Direcao _direction)
    {
        direcao = _direction;
        AtualizarPontaDaArma();
    }

    private Vector2 PontaDaArmaOffSet()
    {
        Vector2 offSet = Vector2.zero;

        if (inventario.ArmaSlot != null)
        {
            switch (inventario.ArmaSlot.NomeAnimacao)
            {
                case "Arma1":
                    offSet = direcao switch
                    {
                        Direcao.Baixo => new Vector2(-0.284f, 0.787f),
                        Direcao.Esquerda => new Vector2(-0.486f, 1.224f),
                        Direcao.Cima => new Vector2(0.283f, 1.56f),
                        Direcao.Direita => new Vector2(0.486f, 1.224f),
                        _ => Vector2.zero,
                    };
                    return offSet;

                case "Arma2":
                    offSet = direcao switch
                    {
                        Direcao.Baixo => new Vector2(-0.188f, 0.62f),
                        Direcao.Esquerda => new Vector2(-0.715f, 1.227f),
                        Direcao.Cima => new Vector2(0.157f, 1.727f),
                        Direcao.Direita => new Vector2(0.715f, 1.227f),
                        _ => Vector2.zero,
                    };
                    return offSet;

                default:
                    return Vector2.zero;
            }
        }

        return offSet;
    }

    private void AtualizarPontaDaArma()
    {
        pontaArma.AtualizarPontaArma(PontaDaArmaOffSet());
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
        ia_Enemy.ReceberSom(player.transform.position, somTiro);
    }
}
