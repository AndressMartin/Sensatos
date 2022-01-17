using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    //Managers
    private ObjectManagerScript objectManager;
    private PauseManagerScript pauseManager;
    private BulletManagerScript bulletManager;

    private HUDScript hud;
    private DialogueUI dialogueUI;

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

    //Enums
    public enum ModoMovimento { Normal, AndandoSorrateiramente, Strafing };
    public enum Estado { Normal, TomandoDano, Atacando, UsandoItem, Morto };

    //Variaveis
    public override int vida { get; protected set; }
    [SerializeField] private int vidaInicial;

    public Direcao direcaoMovimento;
    private float raioPassos;

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
    public ObjectManagerScript GetObjectManager => objectManager;
    public DialogueUI DialogueUI => dialogueUI;
    public Estado GetEstado => estado;
    public float TempoRecarregar => tempoRecarregar;
    public float TempoRecarregarMax => tempoRecarregarMax;
    public bool RapidFire => inventario.armaSlot1.RapidFire;

    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();
        pauseManager = FindObjectOfType<PauseManagerScript>();
        bulletManager = FindObjectOfType<BulletManagerScript>();
        hud = FindObjectOfType<HUDScript>();
        dialogueUI = FindObjectOfType<DialogueUI>();

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

        //Variaveis
        vida = vidaInicial;
        raioPassos = 1.5f;
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
        if (pauseManager.JogoPausado == false && estado != Estado.Morto)
        {
            if (imune)
            {
                EnquantoEstiverImuneContador();
            }
            else if (collisionState)
            {
                foreach (Enemy inimigo in objectManager.listaInimigos)
                {
                    Physics2D.IgnoreCollision(inimigo.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                }
                collisionState = false;
            }

            CadenciaTiro();

            if(recarregando == true)
            {
                RecarregarContador();
                hud.AtualizarBarraDeRecarregamento(tempoRecarregar, tempoRecarregarMax);
            }

            animacao.AtualizarDirecao(direcao, direcaoMovimento);
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
            hud.AtualizarPosicaoDaBarraDeRecarregamento(this);
            hud.BarraDeRecarregamentoAtiva(true);
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
        switch(estado)
        {
            case Estado.Normal:
                if (((rb.velocity.x == 0 && rb.velocity.y == 0) || !(PosicaoDiferente(posAnterior, transform.position))) && animacao.GetAnimacaoAtual() != "Idle")
                {
                    animacao.TrocarAnimacao("Idle");
                }
                else if ((rb.velocity.x != 0 || rb.velocity.y != 0) && PosicaoDiferente(posAnterior, transform.position))
                {
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
                }
                break;

            case Estado.TomandoDano:
                if (animacao.GetAnimacaoAtual() != "TomandoDano")
                {
                    animacao.TrocarAnimacao("TomandoDano");
                }
                break;

            case Estado.Atacando:
                if (animacao.GetAnimacaoAtual() != "Atacando")
                {
                    animacao.TrocarAnimacao("Atacando");
                }
                break;

            case Estado.UsandoItem:
                if (animacao.GetAnimacaoAtual() != inventario.itemAtual.GetNomeAnimacao() + "Usando")
                {
                    animacao.AtualizarArmaBracos("");
                    animacao.TrocarAnimacao(inventario.itemAtual.GetNomeAnimacao() + "Usando");
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
        }
    }

    public void AtaqueHitBox()
    {
        ataqueHitBox.Atacar(direcao, 1, 0.8f, 1.1f, 0.87f);
    }

    public void Atirar()
    {
        if (estado == Estado.Normal)
        {
            AtualizarPontaDaArma();

            if (tempoTiro <= 0 && pontaArma.ColidindoComParedes <= 0)
            {
                if (recarregando == false)
                {
                    GerarSom(inventario.armaSlot1.RaioDoSomDoTiro, true);
                    inventario.armaSlot1.Atirar(this, bulletManager, pontaArma.transform.position, VetorDirecao(direcao), Alvo.Enemy);
                    animacao.AtualizarArmaBracos(inventario.armaSlot1.NomeAnimacao);
                }
            }
        }
    }

    public void Recarregar()
    {
        if(recarregando == false && (inventario.armaSlot1.MunicaoCartucho < inventario.armaSlot1.GetStatus.MunicaoMaxCartucho))
        {
            recarregando = true;
            tempoRecarregar = 0;
            tempoRecarregarMax = inventario.armaSlot1.GetStatus.TempoParaRecarregar;
        }
    }

    private void RecarregarContador()
    {
        tempoRecarregar += Time.deltaTime;

        if(tempoRecarregar >= tempoRecarregarMax)
        {
            inventario.armaSlot1.Recarregar();
            FinalizarRecarregamento();
        }
    }

    private void FinalizarRecarregamento()
    {
        recarregando = false;
        hud.BarraDeRecarregamentoAtiva(false);
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
        animacao.AtualizarArmaBracos(inventario.armaSlot1.NomeAnimacao);
    }

    public void AdicionarAoInventario(Item item)
    {
        inventario.Add(item);
    }

    public void RemoverDoInventario(Item item)
    {
        inventario.Remove(item);
    }

    public void AdicionarAoInventarioMissao(Item item)
    {
        inventarioMissao.Add(item);
    }

    public void RemoverDoInventarioMissao(Item item)
    {
        inventarioMissao.Remove(item);
    }

    public void UsarItem(Item item)
    {
        inventario.UsarItemAtual();
    }

    public void UsarItemAtalho(int atalho)
    {
        //UsarItem(atalho[atalho])
        inventario.UsarItemAtual();
    }

    public void AnimacaoItem(Item item)
    {
        estado = Estado.UsandoItem;
        inventario.SetarItemAtual(item);
    }

    public void UsarItemGameplay()
    {
        inventario.itemAtual.UsarNaGameplay(this);
    }

    public override void TomarDano(int _dano, float _knockBack, float _knockBackTrigger, Vector2 _direcaoKnockBack)
    {
        if (!imune && estado != Estado.Morto)
        {
            if(dialogueUI.IsOpen == true)
            {
                dialogueUI.ForcedCloseDialogueBox();
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

        FinalizarRecarregamento();
    }

    public void GerarSom(float raio, bool somTiro)
    {
        if(somTiro == false)
        {
            raio = raioPassos;
        }
        sound.GerarSom(this, raio, somTiro);
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

    private void DefinirPontaDaArma(out float offSetX, out float offSetY)
    {
        offSetX = 0;
        offSetY = 0;

        switch(inventario.armaSlot1.NomeAnimacao)
        {
            case "Arma1":
                switch(direcao)
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
