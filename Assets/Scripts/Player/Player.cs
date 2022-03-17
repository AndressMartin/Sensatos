using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private AnimacaoJogador animacao;
    private InteragirScript interacaoHitBox;
    private AtaqueFisico ataqueHitBox;
    private PlayerSound sound;
    private PontaArmaPlayerScript pontaArma;
    private Inventario inventario;
    private InventarioMissao inventarioMissao;
    private SomDosTiros somDosTiros;
    private SonsDoJogador sonsDoJogador;

    //Enums
    public enum ModoMovimento { Normal, AndandoSorrateiramente, Strafing };
    public enum Estado { Normal, TomandoDano, Atacando, UsandoItem, Morto };

    //Variaveis
    [SerializeField] private int vidaInicial;

    public Direcao direcaoMovimento;
    private float raioPassos;

    private bool modoDeCombate;

    //Variaveis de controle
    private Vector3 posAnterior;

    public ModoMovimento modoMovimento;
    private Estado estado;

    private float tempoTiro;

    private bool recarregando;
    private float tempoRecarregar;
    private float tempoRecarregarMax;

    private float tempoImune;
    private float tempoImuneMax;
    private float tempoSoftlock,
                  tempoSoftlockMax;

    private bool imune;
    private bool collisionState;

    //Variaveis de respawn
    private Vector2 posicaoRespawn;
    private Direcao direcaoRespawn;

    //Getters
    public GeneralManagerScript GeneralManager => generalManager;
    public AnimacaoJogador Animacao => animacao;
    public Inventario Inventario => inventario;
    public InventarioMissao InventarioMissao => inventarioMissao;
    public SonsDoJogador SonsDoJogador => sonsDoJogador;
    public int Vida => vida;
    public int VidaMax => vidaMax;
    public bool ModoDeCombate => modoDeCombate;
    public Estado GetEstado => estado;
    public float TempoRecarregar => tempoRecarregar;
    public float TempoRecarregarMax => tempoRecarregarMax;
    public bool RapidFire => inventario.ArmaSlot[inventario.ArmaAtual].RapidFire;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        animacao = transform.GetComponent<AnimacaoJogador>();
        interacaoHitBox = GetComponentInChildren<InteragirScript>();
        ataqueHitBox = GetComponentInChildren<AtaqueFisico>();
        sound = FindObjectOfType<PlayerSound>();
        pontaArma = GetComponentInChildren<PontaArmaPlayerScript>();
        inventario = GetComponent<Inventario>();
        inventarioMissao = GetComponent<InventarioMissao>();
        somDosTiros = GetComponentInChildren<SomDosTiros>();
        sonsDoJogador = GetComponent<SonsDoJogador>();

        //Variaveis
        vida = vidaInicial;
        vidaMax = vidaInicial;
        raioPassos = 1.5f;

        modoDeCombate = true;

        ChangeDirection(Direcao.Baixo);

        //Variaveis de controle
        posAnterior = transform.position;

        modoMovimento = ModoMovimento.Normal;
        estado = Estado.Normal;

        tempoTiro = 0;

        recarregando = false;
        tempoRecarregar = 0;
        tempoRecarregarMax = 0;

        tempoImune = 0;
        tempoImuneMax = 1.5f;
        imune = false;
        collisionState = false;

        tempoSoftlock = 0;
        tempoSoftlockMax = 10f;

        SetRespawn();
    }

    void Update()
    {
        if (generalManager.PauseManager.JogoPausado == false && estado != Estado.Morto)
        {
            if (imune)
            {
                EnquantoEstiverImuneContador();
            }
            else if (collisionState)
            {
                foreach (Enemy inimigo in generalManager.ObjectManager.ListaInimigos)
                {
                    Physics2D.IgnoreCollision(inimigo.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                }
                collisionState = false;
            }

            CadenciaTiro();

            if(recarregando == true)
            {
                RecarregarContador();
                generalManager.Hud.AtualizarBarraDeRecarregamento(tempoRecarregar, tempoRecarregarMax);
            }

            Animar();
            playerMovement.Mover();

            ImpedirSoftlock();
        }
    }

    private void FixedUpdate()
    {
        posAnterior = transform.position;

        if(recarregando == true)
        {
            generalManager.Hud.AtualizarPosicaoDaBarraDeRecarregamento(this);
            generalManager.Hud.BarraDeRecarregamentoAtiva(true);
        }
    }

    private void SetRespawn()
    {
        posicaoRespawn = transform.position;
        direcaoRespawn = direcao;
    }

    public void SetRespawn(Vector2 posicao, Direcao direcao)
    {
        posicaoRespawn = posicao;
        direcaoRespawn = direcao;
    }

    public void Respawn()
    {
        vida = vidaInicial;
        transform.position = posicaoRespawn;
        ChangeDirection(direcaoRespawn);
        playerMovement.ResetarVariaveisDeControle();

        ResetarVariaveisDeControle();
    }

    private void ResetarVariaveisDeControle()
    {
        posAnterior = transform.position;
        modoMovimento = ModoMovimento.Normal;
        estado = Estado.Normal;
        tempoTiro = 0;
        tempoImune = 0;
        imune = false;
        collisionState = false;
        tempoSoftlock = 0;

        animacao.SetarVisibilidade(true);
        FinalizarRecarregamento();
    }

    private bool PosicaoDiferente(Vector3 posAnterior, Vector3 posAtual)
    {
        return Mathf.Abs(posAtual.x - posAnterior.x) > 0.01f || Mathf.Abs(posAtual.y - posAnterior.y) > 0.01f;
    }

    private void Animar()
    {
        animacao.AtualizarDirecao(direcao, direcaoMovimento);

        switch (estado)
        {
            case Estado.Normal:
                if (((rb.velocity.x == 0 && rb.velocity.y == 0) || !(PosicaoDiferente(posAnterior, transform.position))) && animacao.AnimacaoAtual != "Idle")
                {
                    animacao.TrocarAnimacao("Idle");
                }
                else if ((rb.velocity.x != 0 || rb.velocity.y != 0) && PosicaoDiferente(posAnterior, transform.position))
                {
                    if (modoMovimento == ModoMovimento.AndandoSorrateiramente)
                    {
                        if (animacao.AnimacaoAtual != "AndandoSorrateiramente")
                        {
                            animacao.TrocarAnimacao("AndandoSorrateiramente");
                        }
                    }
                    else if (animacao.AnimacaoAtual != "Andando")
                    {
                        animacao.TrocarAnimacao("Andando");
                    }
                }
                break;

            case Estado.TomandoDano:
                if (animacao.AnimacaoAtual != "TomandoDano")
                {
                    animacao.TrocarAnimacao("TomandoDano");
                }
                break;

            case Estado.Atacando:
                if (animacao.AnimacaoAtual != "Atacando")
                {
                    animacao.TrocarAnimacao("Atacando");
                }
                break;

            case Estado.UsandoItem:
                if (animacao.AnimacaoAtual != inventario.ItemAtual.GetNomeAnimacao + "Usando")
                {
                    animacao.AtualizarArmaBracos("");
                    animacao.TrocarAnimacao(inventario.ItemAtual.GetNomeAnimacao + "Usando");
                }
                break;
        }
    }

    public void FinalizarAnimacao()
    {
        estado = Estado.Normal;
    }

    public void Interagir()
    {
        if (estado == Estado.Normal)
        {
            interacaoHitBox.Interagir(this, direcao);
        }
    }

    public BoxCollider2D GetHitBoxInteracao()
    {
        interacaoHitBox.AtualizarHitBox(direcao);
        return interacaoHitBox.GetBoxCollider2D();
    }

    public void Atacar()
    {
        if (estado == Estado.Normal)
        {
            estado = Estado.Atacando;
            animacao.AtualizarArmaBracos("");

            sonsDoJogador.TocarSom(SonsDoJogador.Som.AtaqueFisico);
        }
    }

    public void AtaqueHitBox()
    {
        ataqueHitBox.Atacar(direcao, 3, 5, 0.8f, 1.1f, 0.87f);
    }

    public void Atirar()
    {
        if (estado == Estado.Normal && inventario.ArmaSlot[inventario.ArmaAtual] != null)
        {
            AtualizarPontaDaArma();

            if (tempoTiro <= 0 && pontaArma.ColidindoComParedes <= 0)
            {
                if (recarregando == false)
                {
                    inventario.ArmaSlot[inventario.ArmaAtual].Atirar(this, generalManager.BulletManager, pontaArma.transform.position, VetorDirecao(direcao), Alvo.Enemy);
                    animacao.AtualizarArmaBracos(inventario.ArmaSlot[inventario.ArmaAtual].NomeAnimacao);
                }
            }
        }
    }

    public void Recarregar()
    {
        if(recarregando == false && (inventario.ArmaSlot[inventario.ArmaAtual].MunicaoCartucho < inventario.ArmaSlot[inventario.ArmaAtual].GetStatus.MunicaoMaxCartucho))
        {
            recarregando = true;
            tempoRecarregar = 0;
            tempoRecarregarMax = inventario.ArmaSlot[inventario.ArmaAtual].GetStatus.TempoParaRecarregar;
        }
    }

    private void RecarregarContador()
    {
        tempoRecarregar += Time.deltaTime;

        if(tempoRecarregar >= tempoRecarregarMax)
        {
            inventario.ArmaSlot[inventario.ArmaAtual].Recarregar();
            FinalizarRecarregamento();
        }
    }

    private void FinalizarRecarregamento()
    {
        recarregando = false;
        generalManager.Hud.BarraDeRecarregamentoAtiva(false);
    }

    public void SemMunicao()
    {
        Debug.Log("Sem Municao!");
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

    public void AtualizarArma()
    {
        if(recarregando == true)
        {
            FinalizarRecarregamento();
        }
        AtualizarPontaDaArma();
        animacao.AtualizarArmaBracos(inventario.ArmaSlot[inventario.ArmaAtual].NomeAnimacao);
    }

    public void GerarSomDoTiro()
    {
        GerarSom(inventario.ArmaSlot[inventario.ArmaAtual].RaioDoSomDoTiro, true);
        somDosTiros.TocarSom(inventario.ArmaSlot[inventario.ArmaAtual].GetStatus.SomDoTiro);
    }

    public void UsarItem(Item item)
    {
        item.Usar(this);
    }

    public void UsarItemAtalho(int atalho)
    {
        if (inventario.AtalhosDeItens[atalho].ID != 0)
        {
            UsarItem(inventario.AtalhosDeItens[atalho]);
        }
    }

    public void AnimacaoItem(Item item)
    {
        estado = Estado.UsandoItem;
        inventario.SetarItemAtual(item);
    }

    public void UsarItemGameplay()
    {
        inventario.ItemAtual.UsarNaGameplay(this);
    }

    public override void TomarDano(int _dano, float _knockBack, float _knockBackTrigger, Vector2 _direcaoKnockBack)
    {
        if (!imune && estado != Estado.Morto)
        {
            
            if (generalManager.DialogueUI.IsOpen == true)
            {
                generalManager.DialogueUI.ForcedCloseDialogueBox();
            }

            if (recarregando == true)
            {
                FinalizarRecarregamento();
            }

            if (vida <= 0)
            {
                Morrer();
            }
            else
            {
                vida -= _dano;

                imune = true;
                tempoImune = 0;
                estado = Estado.TomandoDano;
                KnockBack(_knockBack, _direcaoKnockBack);

                sonsDoJogador.TocarSom(SonsDoJogador.Som.Dano);
            }
        }
    }

    public override void KnockBack(float _knockBack, Vector2 _direcaoKnockBack)
    {
        playerMovement.KnockBack(_knockBack, _direcaoKnockBack);
    }

    private void Morrer()
    {
        estado = Estado.Morto;
        playerMovement.ZerarVelocidade();
        animacao.AtualizarArmaBracos("");
        animacao.TrocarAnimacao("Morto");

        animacao.AtualizarDirecao(direcao, direcaoMovimento);

        FinalizarRecarregamento();

        sonsDoJogador.TocarSom(SonsDoJogador.Som.Morte);
    }

    public void GerarSom(float raio, bool somTiro)
    {
        if(somTiro == false)
        {
            raio = raioPassos;
        }
        sound.GerarSom(this, raio, somTiro);
    }

    public void SetModoDeCombate(bool ativo)
    {
        modoDeCombate = ativo;

        if(modoDeCombate == false)
        {
            animacao.AtualizarArmaBracos("");
        }
    }

    public void ChangeDirection(Direcao direcao)
    {
        this.direcao = direcao;
        AtualizarPontaDaArma();
    }

    public void ChangeDirectionMovement(Direcao direcao)
    {
        direcaoMovimento = direcao;
    }

    private Vector2 PontaDaArmaOffSet()
    {
        Vector2 offSet = Vector2.zero;

        if(inventario.ArmaSlot[inventario.ArmaAtual] != null)
        {
            switch (inventario.ArmaSlot[inventario.ArmaAtual].NomeAnimacao)
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

    private void ImpedirSoftlock()
    {
        if (estado == Estado.TomandoDano || estado == Estado.Atacando || estado == Estado.UsandoItem)
        {
            tempoSoftlock += Time.deltaTime;

            if(tempoSoftlock >= tempoSoftlockMax)
            {
                FinalizarAnimacao();
            }
        }
        else
        {
            tempoSoftlock = 0;
        }
    }

    void EnquantoEstiverImuneContador()
    {
        animacao.Piscar();
        tempoImune += Time.deltaTime;
        if (tempoImune > tempoImuneMax)
        {
            animacao.SetarVisibilidade(true);
            imune = false;
            tempoImune = 0;
        }
    }

    private void ChangeCollision(Collision2D collision, bool ignorarColisao)
    {
        Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), ignorarColisao);
        collisionState = ignorarColisao;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (imune)
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                ChangeCollision(collision, true);
            }
        }
    }
}
