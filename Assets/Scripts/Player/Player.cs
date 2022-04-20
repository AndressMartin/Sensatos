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
    private SomDosTiros somDosTiros;
    private SonsDoJogador sonsDoJogador;

    private Inventario inventario;
    private InventarioMissao inventarioMissao;

    //Enums
    public enum ModoMovimento { Normal, AndandoSorrateiramente, Strafing };
    public enum Estado { Normal, TomandoDano, Atacando, UsandoItem, Morto };

    //Variaveis
    private static int vidaMaxima = 0;
    [SerializeField] private int vidaInicial;

    [HideInInspector] public Direcao direcaoMovimento;
    [SerializeField] private float raioPassos;

    private bool modoDeCombate;

    //Variaveis de controle
    private Vector3 posAnterior;

    [HideInInspector] public ModoMovimento modoMovimento;
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
    public SomDosTiros SomDosTiros => somDosTiros;
    public SonsDoJogador SonsDoJogador => sonsDoJogador;
    public Inventario Inventario => inventario;
    public InventarioMissao InventarioMissao => inventarioMissao;
    public int Vida => vida;
    public int VidaMax => vidaMax;
    public int VidaMaxima => vidaMaxima;
    public bool ModoDeCombate => modoDeCombate;
    public Estado GetEstado => estado;
    public float TempoRecarregar => tempoRecarregar;
    public float TempoRecarregarMax => tempoRecarregarMax;
    public bool RapidFire => inventario.ArmaSlot[inventario.ArmaAtual].RapidFire;

    //Setters
    public void SetVidaMaxima(int novaVidaMaxima)
    {
        vidaMaxima = novaVidaMaxima;
    }

    void Start()
    {
        //Eventos
        SaveManager.instance.OnSavingGame.AddListener(AtualizarSaveFile);
        SaveManager.instance.OnGameLoaded.AddListener(CarregarSaveFile);

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
        somDosTiros = GetComponentInChildren<SomDosTiros>();
        sonsDoJogador = GetComponent<SonsDoJogador>();

        inventario = GetComponent<Inventario>();
        inventarioMissao = GetComponent<InventarioMissao>();

        //Variaveis

        if(vidaMaxima == 0)
        {
            vidaMaxima = vidaInicial;
        }

        vidaMax = vidaMaxima;
        vida = vidaMax;

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

        SetRespawn(transform.position, direcao);
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

    public void SetRespawn(Vector2 posicao, Direcao direcao)
    {
        posicaoRespawn = posicao;
        direcaoRespawn = direcao;

        SaveData.AtualizarInventarioRespawn(this);
    }

    public void Respawn()
    {
        vida = vidaMax;
        transform.position = posicaoRespawn;
        ChangeDirection(direcaoRespawn);
        playerMovement.ResetarVariaveisDeControle();

        inventario.Respawn();
        inventarioMissao.Respawn();

        ResetarVariaveisDeControle();

        generalManager.Hud.AtualizarPlayerHUD();
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

            sonsDoJogador.TocarSom(SonsDoJogador.Som.RecarregarArma);
        }
    }

    private void RecarregarContador()
    {
        tempoRecarregar += Time.deltaTime;

        if(tempoRecarregar >= tempoRecarregarMax)
        {
            inventario.ArmaSlot[inventario.ArmaAtual].Recarregar();
            FinalizarRecarregamento();

            sonsDoJogador.TocarSom(SonsDoJogador.Som.TerminarDeRecarregarArma);
        }
    }

    private void FinalizarRecarregamento()
    {
        recarregando = false;
        generalManager.Hud.BarraDeRecarregamentoAtiva(false);

        generalManager.Hud.AtualizarPlayerHUD();
    }

    public void SemMunicao()
    {
        sonsDoJogador.TocarSom(SonsDoJogador.Som.SemMunicao);
    }

    public bool TemMunicao()
    {
        return inventario.ArmaSlot[inventario.ArmaAtual].MunicaoCartucho > 0;
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
        if (inventario.Armas.Count < 2)
        {
            return;
        }

        if (recarregando == true)
        {
            FinalizarRecarregamento();
        }
        AtualizarPontaDaArma();
        animacao.AtualizarArmaBracos(inventario.ArmaSlot[inventario.ArmaAtual].NomeAnimacao);

        sonsDoJogador.TocarSom(SonsDoJogador.Som.TrocarDeArma);
    }

    public void GerarSomDoTiro()
    {
        GerarSom(inventario.ArmaSlot[inventario.ArmaAtual].RaioDoSomDoTiro, true);
        somDosTiros.TocarSom(inventario.ArmaSlot[inventario.ArmaAtual].GetStatus.SomDoTiro);
    }

    public void UsarItem(Item item)
    {
        item.Usar(this);
        generalManager.Hud.AtualizarPlayerHUD();
    }

    public void UsarItemAtalho(int atalho)
    {
        if (inventario.AtalhosDeItens[atalho].ID != Listas.instance.ListaDeItens.GetID["ItemVazio"])
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
        generalManager.Hud.AtualizarPlayerHUD();
    }

    public override void TomarDano(int _dano, float _knockBack, float _knockBackTrigger, Vector2 _direcaoKnockBack)
    {
        if (!imune && estado != Estado.Morto)
        {
            vida -= _dano;

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
                vida = 0;
                Morrer();
            }
            else
            {
                imune = true;
                tempoImune = 0;
                estado = Estado.TomandoDano;
                KnockBack(_knockBack, _direcaoKnockBack);

                sonsDoJogador.TocarSom(SonsDoJogador.Som.Dano);
            }

            generalManager.Hud.AtualizarPlayerHUD();
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
                        Direcao.Baixo => new Vector2(-0.277f, 0.687f),
                        Direcao.Esquerda => new Vector2(-0.663f, 1.251f),
                        Direcao.Cima => new Vector2(0.283f, 1.642f),
                        Direcao.Direita => new Vector2(0.663f, 1.251f),
                        _ => Vector2.zero,
                    };
                    return offSet;

                case "Arma2":
                    offSet = direcao switch
                    {
                        Direcao.Baixo => new Vector2(-0.188f, 0.515f),
                        Direcao.Esquerda => new Vector2(-0.846f, 1.262f),
                        Direcao.Cima => new Vector2(0.157f, 1.682f),
                        Direcao.Direita => new Vector2(0.846f, 1.262f),
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

    private void AtualizarSaveFile()
    {
        SaveData.AtualizarSaveFile(this);
    }

    private void CarregarSaveFile()
    {
        vidaMaxima = SaveData.SaveAtual.vidaMaxima;
        vidaMax = vidaMaxima;
        vida = vidaMax;

        inventario.CarregarSave(SaveData.SaveAtual.inventarioSave);
        inventarioMissao.CarregarSave(SaveData.SaveAtual.inventarioSave);
    }

    public void ResetarPlayer()
    {
        vidaMaxima = 0;
        vidaMax = vidaMaxima;
        vida = vidaMax;

        inventario.ResetarInventario();
        inventarioMissao.ResetarInventario();
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
