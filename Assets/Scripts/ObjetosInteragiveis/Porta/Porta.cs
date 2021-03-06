using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private AnimacaoPorta animacao;
    private BoxCollider2D colisao;
    private BoxCollider2D hitBoxTiro;
    private BoxCollider2D hitBoxVisao;

    //Enums
    public enum TipoPorta { Simples, Normal, Contencao }
    public enum Estado { NaoLockdown, Lockdown }
    public enum Direcao { F, L }

    //Variaveis
    [SerializeField] private ItemChave chave;
    private bool aberto;
    private bool trancado;

    //Variaveis de respawn
    private bool trancadoRespawn;
    private bool abertoRespawn;

    [SerializeField] private TipoPorta tipoPorta;
    private Estado estado;

    [SerializeField] private Direcao direcao;

    [SerializeField] private float distanciaParaTocarSom;

    [SerializeField] private AudioClip somTrancada;
    [SerializeField] private AudioClip somDestrancar;
    [SerializeField] private AudioClip somAbrir;
    [SerializeField] private AudioClip somFechar;

    //Getters
    public bool Trancado => trancado;
    public TipoPorta GetTipoPorta => tipoPorta;
    public Estado GetEstado => estado;

    private void Awake()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<AnimacaoPorta>();
        colisao = GetComponent<BoxCollider2D>();
        hitBoxTiro = transform.Find("HitBoxTiro").GetComponent<BoxCollider2D>();
        hitBoxVisao = transform.Find("HitBoxVisao").GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        //Variaveis
        estado = Estado.NaoLockdown;

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);
        generalManager.ObjectManager.AdicionarAsPortas(this);

        //Verificar se tem chave para estar trancada
        if (chave != null)
        {
            trancado = true;
        }
        else
        {
            trancado = false;
        }

        //Definir se a porta vai influenciar o pathfinder
        if (tipoPorta == TipoPorta.Contencao)
        {
            gameObject.layer = LayerMask.NameToLayer("GridsComColisao");
        }

        animacao.TrocarAnimacao("Fechada", direcao);

        SetRespawn();
        Respawn();
    }

    public override void SetRespawn()
    {
        trancadoRespawn = trancado;
        abertoRespawn = aberto;
    }

    public override void Respawn()
    {
        aberto = abertoRespawn;
        trancado = trancadoRespawn;

        if (tipoPorta == TipoPorta.Contencao)
        {
            AbrirPorta();
        }
        else
        {
            ForceFecharPorta();
        }
    }

    public override void Interagir(Player player)
    {
        if(tipoPorta == TipoPorta.Contencao)
        {
            return;
        }

        if(trancado)
        {
            //Verifica se ha uma chave nos itens do jogador e se alguma delas tem o id igual ao da chave que destranca a porta
            foreach(ItemChave item in player.InventarioMissao.Itens)
            {
                if(item is ItemChave)
                {
                    if(item.ID == this.chave.ID)
                    {
                        Destrancar();
                        TocarSom(somDestrancar);
                        return;
                    }
                }
            }

            TocarSom(somTrancada);
        }
        else
        {
            switch (tipoPorta)
            {
                case TipoPorta.Simples:
                    AbrirPorta();
                    break;

                case TipoPorta.Normal:
                    switch (estado)
                    {
                        case Estado.NaoLockdown:
                            AbrirPorta();
                            break;
                    }
                    break;
            }
        }
    }

    private void Destrancar()
    {
        switch (tipoPorta)
        {
            case TipoPorta.Simples:
                trancado = false;
                break;

            case TipoPorta.Normal:
                switch (estado)
                {
                    case Estado.NaoLockdown:
                        trancado = false;
                        break;
                }
                break;
        }
    }

    public void FecharPorta()
    {
        if (aberto)
        {
            ForceFecharPorta();
        }
    }

    public void AbrirPorta()
    {
        aberto = true;
        if (animacao.AnimacaoAtual != "Aberta")
        {
            animacao.TrocarAnimacao("Aberta", direcao);
        }
        PortaAberta(aberto);
    }

    private void ForceFecharPorta()
    {
        aberto = false;
        if (animacao.AnimacaoAtual != "Fechada")
        {
            animacao.TrocarAnimacao("Fechada", direcao);
        }
        PortaAberta(aberto);
    }

    private void PortaAberta(bool portaAberta)
    {
        if(portaAberta == true)
        {
            colisao.isTrigger = portaAberta;

            if (tipoPorta == TipoPorta.Contencao)
            {
                generalManager.PathfinderManager.EscanearPathfinder(colisao);
            }

            colisao.enabled = !portaAberta;
            hitBoxTiro.enabled = !portaAberta;
            hitBoxVisao.enabled = !portaAberta;

            TocarSom(somAbrir);
        }
        else
        {
            colisao.enabled = !portaAberta;
            hitBoxTiro.enabled = !portaAberta;
            hitBoxVisao.enabled = !portaAberta;

            colisao.isTrigger = portaAberta;

            if (tipoPorta == TipoPorta.Contencao)
            {
                generalManager.PathfinderManager.EscanearPathfinder(colisao);
            }

            TocarSom(somFechar);
        }
    }

    public void AtivarLockDown()
    {
        if (tipoPorta != TipoPorta.Simples)
        {
            estado = Estado.Lockdown;
            trancado = true;
            ForceFecharPorta();
        }

    }
    public void DesativarLockDown()
    {
        if (tipoPorta != TipoPorta.Simples)
        {
            estado = Estado.NaoLockdown;

            switch (tipoPorta)
            {
                case TipoPorta.Normal:
                    break;
                case TipoPorta.Contencao:
                    AbrirPorta();
                    break;
                default:
                    break;
            }
        }
    }

    private bool DistanciaDoPlayer()
    {
        //Ve se a distancia do projetil do seu ponto inicial e maior que a distancia maxima que ele pode percorrer, usando sqrMagnitude para ser um pouco mais otimizado
        Vector3 diferenca = transform.position - generalManager.Player.transform.position;
        float distancia = diferenca.sqrMagnitude;

        if (distancia < distanciaParaTocarSom * distanciaParaTocarSom)
        {
            return true;
        }

        return false;
    }

    private void TocarSom(AudioClip audioClip)
    {
        if(audioClip == null)
        {
            return;
        }

        if(DistanciaDoPlayer() == true)
        {
            generalManager.SoundManager.TocarSom(audioClip);
        }
    }
}
