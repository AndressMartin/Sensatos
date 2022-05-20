using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering;

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
    private SomDosTiros somDosTiros;
    private SonsDoInimigo sonsDoInimigo;
    private SortingGroup sortingGroup;

    private Player player;

    //Variaveis
    [SerializeField] LayerMask layerDasZonas;

    [SerializeField] private int vidaInicial;

    [SerializeField] private Color corEfeitoTomandoDano;
    [SerializeField] private float velocidadeEfeitoTomandoDano;

    private int zona;

    public enum Estado { Normal, TomandoDano };
    private Estado estado;

    private bool morto;
    private bool playerOnAttackRange;
    private bool playerMovementRange;


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
    public int Zona => zona;
    public Estado GetEstado => estado;
    public bool Morto => morto;
    public bool PlayerOnAttackRange => playerOnAttackRange;
    public bool PlayerMovementRange => playerMovementRange;

    public Player GetPlayer => player;
    public InventarioEnemy GetInventarioEnemy => inventario;
    public EnemyMovement GetEnemyMovement => enemyMovement;
    public IAEnemy GetIAEnemy => ia_Enemy;
    public SomDosTiros SomDosTiros => somDosTiros;
    public SonsDoInimigo SonsDoInimigo => sonsDoInimigo;
    public Vector2 VetorVelocidade => vetorVelocidade;

    public override bool IsMorto()
    {
        return morto;
    }

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
        somDosTiros = GetComponentInChildren<SomDosTiros>();
        sonsDoInimigo = GetComponent<SonsDoInimigo>();
        sortingGroup = GetComponent<SortingGroup>();

        player = generalManager.Player;

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

        SetarZona();

        iniciado = true;
    }

    void Update()
    { 
        if(generalManager.PauseManager.JogoPausado == false && generalManager.Player.GetEstado != Player.Estado.Morto)
        {
            RotinasDoInimigo();
        }
        else if (generalManager.Player.GetEstado == Player.Estado.Morto && morto == false)
        {
            enemyMovement.ZerarVelocidade();
            Animar();
        }
    }

    private void FixedUpdate()
    {
        /*
        if((Vector2)transform.position != posAnterior)
        {
            generalManager.PathfinderManager.EscanearPathfinder(colisao);
        }
        */

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
            ResetarVariaveis();
        }
        else
        {
            Desaparecer();
        }
    }

    public void ResetarVariaveis()
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

        sortingGroup.sortingOrder = 0;

        animacao.AtivarSpriteRenderers();

        somDosTiros.PararSom();

        ResetarVariaveisDeControle();
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

    private void SetarZona()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(colisao.bounds.center, colisao.bounds.extents, 0, layerDasZonas);
        foreach (Collider2D objeto in hitColliders)
        {
            if(objeto.GetComponent<Zona>())
            {
                zona = objeto.GetComponent<Zona>().GetZona;
                break;
            }
        }
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

            if(ia_Enemy.GetEstadoDeteccaoPlayer != IAEnemy.EstadoDeteccaoPlayer.PlayerDetectado)
            {
                enemyVision.FieldOfViewAtiva(true);
            }
            else
            {
                enemyVision.FieldOfViewAtiva(false);
            }
        }
        else
        {
            enemyVision.FieldOfViewAtiva(false);
        }
    }

    private void Animar()
    {
        animacao.AtualizarDirecao(direcao, direcao);

        switch (estado)
        {
            case Estado.Normal:
                if ((vetorVelocidade.x == 0 && vetorVelocidade.y == 0) && animacao.AnimacaoAtual != "Idle")
                {
                    animacao.TrocarAnimacao("Idle");
                }
                else if ((vetorVelocidade.x != 0 || vetorVelocidade.y != 0) && enemyMovement.GetVelocidade <= enemyMovement.GetVelocidadeModoNormal && animacao.AnimacaoAtual != "Andando")
                {
                    animacao.TrocarAnimacao("Andando");
                }
                else if ((vetorVelocidade.x != 0 || vetorVelocidade.y != 0) && enemyMovement.GetVelocidade > enemyMovement.GetVelocidadeModoNormal && animacao.AnimacaoAtual != "Correndo")
                {
                    animacao.TrocarAnimacao("Correndo");
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
    public void SetarAttackRange(bool _vendo, bool _colidiuMovimento)
    {
        playerOnAttackRange = _vendo;
        playerMovementRange = _colidiuMovimento;
    }
    public bool Atirar()
    {
        TrocarDirecaoAtaque(player.transform.position);

        if (tempoTiro <= 0)
        {
            inventario.ArmaSlot.Atirar(this, generalManager.BulletManager, pontaArma.transform.position, DirecaoPlayer(player), Alvo.Player);
            animacao.AtualizarArmaBracos(inventario.ArmaSlot.NomeAnimacao);

            SetCadenciaTiro(inventario.ArmaSlot.GetStatus.CadenciaDosTiros);
            somDosTiros.TocarSom(inventario.ArmaSlot.GetStatus.SomDoTiro);
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
        if (morto == false && estado == Estado.Normal)
        {
            if (ia_Enemy.GetEstadoDeteccaoPlayer != IAEnemy.EstadoDeteccaoPlayer.PlayerDetectado)
            {
                StealthKill(_direcao);
            }
            else
            {
                TomarDano(_dano, _knockBack, _knockBackTrigger, VetorDirecao(_direcao));
            }

            generalManager.Player.SonsDoJogador.TocarSom(SonsDoJogador.Som.AcertoAtaqueFisico);
        }
    }

    public override void TomarDano(int _dano, float _knockBack, float _knockBackTrigger, Vector2 _direcaoKnockBack)
    {
        if(morto == false && estado == Estado.Normal)
        {
            vida -= _dano;

            if (vida <= 0)
            {
                animacao.AtualizarArmaBracos("");
                animacao.TrocarAnimacao("Morto");
                Morrer();
            }
            else
            {
                ia_Enemy.ReceberDano();

                knockBackTrigger += _knockBackTrigger;
                knockBackTempo = 6;

                if (knockBackTrigger >= knockBackTriggerMax)
                {
                    knockBackTrigger = 0;
                    estado = Estado.TomandoDano;
                    KnockBack(_knockBack, _direcaoKnockBack);
                }

                EfeitoTomandoDano();
            }
        } 
    }

    public override void KnockBack(float _knockBack, Vector2 _direcaoKnockBack)
    {
        enemyMovement.KnockBack(_knockBack, _direcaoKnockBack);
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

        rb.bodyType = RigidbodyType2D.Static;

        colisao.enabled = false;

        animacao.AtualizarDirecao(direcao, direcao);

        sonsDoInimigo.TocarSom(SonsDoInimigo.Som.Morte);
    }

    public void MudarOrdemLayer()
    {
        sortingGroup.sortingOrder = -1;
    }

    public void AnimacaoDesaparecendo()
    {
        animacao.AtualizarArmaBracos("");
        animacao.TrocarAnimacao("Desaparecendo");
    }

    public void Desaparecer()
    {
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
                        Direcao.Baixo => new Vector2(-0.22f, 0.737f),
                        Direcao.Esquerda => new Vector2(-0.697f, 1.232f),
                        Direcao.Cima => new Vector2(0.22f, 1.562f),
                        Direcao.Direita => new Vector2(0.697f, 1.232f),
                        _ => Vector2.zero,
                    };
                    return offSet;

                case "Arma2":
                    offSet = direcao switch
                    {
                        Direcao.Baixo => new Vector2(-0.125f, 0.488f),
                        Direcao.Esquerda => new Vector2(-1.003f, 1.234f),
                        Direcao.Cima => new Vector2(0.125f, 1.632f),
                        Direcao.Direita => new Vector2(1.003f, 1.234f),
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

        if(Mathf.Abs(DistanciaX) > Mathf.Abs(DistanciaY))
        {
            if (DistanciaX > 0)
            {
                ChangeDirection(EntityModel.Direcao.Direita);
            }
            else
            {
                ChangeDirection(EntityModel.Direcao.Esquerda);
            }
        }
        else
        {
            if (DistanciaY < 0)
            {
                ChangeDirection(EntityModel.Direcao.Baixo);
            }
            else
            {
                ChangeDirection(EntityModel.Direcao.Cima);
            }
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

    public void SeDesativarNoLockdown()
    {
        if(gameObject.activeSelf == true)
        {
            StartCoroutine(SeDesativarNoLockdownCorrotina());
        }
    }

    private void EfeitoTomandoDano()
    {
        animacao.SetTintSolidEffect(corEfeitoTomandoDano, velocidadeEfeitoTomandoDano);
    }

    private IEnumerator SeDesativarNoLockdownCorrotina()
    {
        yield return null;
        animacao.AtualizarArmaBracos("");
        animacao.TrocarAnimacao("MortoSorrateiramente");
        Morrer();

        while (animacao.AnimacaoAtual != "Vazio")
        {
            yield return null;
        }

        morto = false;
        ResetarVariaveis();
    }
    
}
