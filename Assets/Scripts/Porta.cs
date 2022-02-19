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

    //Enums
    public enum TipoPorta { Simples, Normal, Contencao }
    private enum Estado { NaoLockdown, Lockdown }

    //Variaveis
    public Chave chave;
    private bool aberto;
    public bool trancado;

    //Variaveis de respawn
    private bool trancadoRespawn;
    private bool abertoRespawn;

    [SerializeField] public TipoPorta tipoPorta;
    [SerializeField]private Estado estado = Estado.NaoLockdown;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<AnimacaoPorta>();
        colisao = GetComponent<BoxCollider2D>();
        hitBoxTiro = transform.Find("HitBoxTiro").GetComponent<BoxCollider2D>();

        //Variaveis
        ativo = true;

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
            List<Item> listaItens = player.InventarioMissao.Itens;
            foreach(Item item in listaItens)
            {
                if(item is Chave)
                {
                    Chave chave = (Chave)item;
                    if(chave.IDChave == this.chave.IDChave)
                    {
                        Destrancar();
                    }
                }
            }
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

    void Destrancar()
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
        if (!trancado && aberto)
        {
            ForceFecharPorta();
        }
    }

    void AbrirPorta()
    {
        aberto = true;
        if (animacao.AnimacaoAtual != "Aberta")
        {
            animacao.TrocarAnimacao("Aberta");
        }
        Door(aberto);
    }

    void ForceFecharPorta()
    {
        aberto = false;
        if (animacao.AnimacaoAtual != "Fechada")
        {
            animacao.TrocarAnimacao("Fechada");
        }
        Door(aberto);
    }

    void Door(bool portaAberta)
    {
        colisao.isTrigger = portaAberta;
        hitBoxTiro.enabled = !portaAberta;
        generalManager.PathfinderManager.EscanearPathfinder(colisao);
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
            trancado = false;

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
}
