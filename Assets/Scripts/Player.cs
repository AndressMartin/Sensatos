using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    //Managers
    private ObjectManagerScript objectManager;
    private PauseManagerScript pauseManager;
    private BulletManagerScript bulletManager;
    private DialogueUI dialogueUI;

    //Componentes
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private AnimacaoJogador animacao;
    private InteragirScript interacaoHitBox;
    private AtaqueFisico ataqueHitBox;
    private PlayerSound sound;
    private PontaArmaScript pontaArma;
    private Inventario inventario;
    private InventarioMissao inventarioMissao;

    //Enums
    public enum ModoMovimento { Normal, AndandoSorrateiramente, Strafing };
    public enum Estado { Normal, TomandoDano, Atacando, UsandoItem, Morto };

    //Variaveis
    public override int vida { get; protected set; }
    [SerializeField] private int vidaInicial;

    public Direcao direcaoMovimento;

    [SerializeField] private float distanceCenter;
    [SerializeField] private float distanceY;
    private float raioPassos;

    //Variaveis de controle
    private Vector3 posAnterior;

    public ModoMovimento modoMovimento;
    private Estado estado;

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
    public float DistanceY => distanceY;
    public Estado GetEstado => estado;
    public float TempoRecarregar => tempoRecarregar;
    public float TempoRecarregarMax => tempoRecarregarMax;

    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();
        pauseManager = FindObjectOfType<PauseManagerScript>();
        bulletManager = FindObjectOfType<BulletManagerScript>();
        dialogueUI = FindObjectOfType<DialogueUI>();

        //Componentes
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        animacao = transform.GetComponent<AnimacaoJogador>();
        interacaoHitBox = GetComponentInChildren<InteragirScript>();
        ataqueHitBox = GetComponentInChildren<AtaqueFisico>();
        sound = FindObjectOfType<PlayerSound>();
        pontaArma = GetComponentInChildren<PontaArmaScript>();
        inventario = GetComponent<Inventario>();
        inventarioMissao = GetComponent<InventarioMissao>();

        //Variaveis
        vida = vidaInicial;
        raioPassos = 1.5f;

        //Variaveis de controle
        posAnterior = transform.position;

        modoMovimento = ModoMovimento.Normal;
        estado = Estado.Normal;

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
        if(pauseManager.JogoPausado == false && estado != Estado.Morto)
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

            if(recarregando == true)
            {
                RecarregarContador();
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
        direcao = direcaoRespawn;
        playerMovement.ResetarVariaveisDeControle();

        ResetarVariaveisDeControle();
    }

    private void ResetarVariaveisDeControle()
    {
        posAnterior = transform.position;
        modoMovimento = ModoMovimento.Normal;
        estado = Estado.Normal;
        tempoImune = 0;
        imune = false;
        collisionState = false;
        tempoSoftlock = 0;

        animacao.SetarVisibilidade(true);
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
            if(recarregando == false)
            {
                GerarSom(inventario.armaSlot1.RaioDoSomDoTiro, true);
                inventario.armaSlot1.Atirar(this, bulletManager, pontaArma.transform.position, VetorDirecao(direcao), Alvo.Enemy);
                animacao.AtualizarArmaBracos(inventario.armaSlot1.NomeAnimacao);
            }
        }
    }

    public void Recarregar()
    {
        if(recarregando == false)
        {
            recarregando = true;
            tempoRecarregar = 0;
            tempoRecarregarMax = inventario.armaSlot1.TempoParaRecarregar;
        }
    }

    private void RecarregarContador()
    {
        tempoRecarregar += Time.deltaTime;

        if(tempoRecarregar >= tempoRecarregarMax)
        {
            inventario.armaSlot1.Recarregar();
            recarregando = false;
        }
    }

    private void CancelarRecarregamento()
    {
        recarregando = false;
    }

    public void SemMunicao()
    {
        Debug.Log("Sem Municao!");
    }

    public void AtualizarArma()
    {
        if(recarregando == true)
        {
            CancelarRecarregamento();
        }
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
    }

    public void GerarSom(float raio, bool somTiro)
    {
        if(somTiro == false)
        {
            raio = raioPassos;
        }
        sound.GerarSom(this, raio, somTiro);
    }

    public void ChangeDirection(string lado)
    {
        switch (lado)
        {
            case "Esquerda":
                direcao = Direcao.Esquerda;
                break;
            case "Direita":
                direcao = Direcao.Direita;
                break;
            case "Cima":
                direcao = Direcao.Cima;
                break;
            case "Baixo":
                direcao = Direcao.Baixo;
                break;
        }
        pontaArma.AtualizarPontaArma(direcao, distanceCenter, distanceY);
    }

    public void ChangeDirectionMovement(string lado)
    {
        switch (lado)
        {
            case "Esquerda":
                direcaoMovimento = Direcao.Esquerda;
                break;
            case "Direita":
                direcaoMovimento = Direcao.Direita;
                break;
            case "Cima":
                direcaoMovimento = Direcao.Cima;
                break;
            case "Baixo":
                direcaoMovimento = Direcao.Baixo;
                break;
        }
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
}
