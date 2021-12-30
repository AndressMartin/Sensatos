using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : ObjetoInteragivel
{
    //Managers
    private ObjectManagerScript objectManager;

    //Componentes
    private AnimacaoPorta animacao;
    private BoxCollider2D colisao;

    //Enums
    public enum TipoPorta { Simples, Normal, Contencao }
    private enum Estado { NaoLockdown, Lockdown }

    //Variaveis
    public Chave chave;
    private bool aberto;
    public bool trancado;

    //Variaveis de respawn
    private bool trancadoRespawn;

    [SerializeField] public TipoPorta tipoPorta;
    [SerializeField]private Estado estado = Estado.NaoLockdown;

    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();

        //Componentes
        animacao = GetComponent<AnimacaoPorta>();
        colisao = GetComponent<BoxCollider2D>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager.adicionarAosObjetosInteragiveis(this);
        objectManager.adicionarAsPortas(this);

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
    }

    public override void Respawn()
    {
        aberto = false;
        trancado = trancadoRespawn;
        ForceFecharPorta();

        if (tipoPorta == TipoPorta.Contencao)
        {
            AbrirPorta();
        }
    }

    public override void Interagir(Player player)
    {
        if(trancado)
        {
            //Verifica se ha uma chave nos itens do jogador e se alguma delas tem o id igual ao da chave que destranca a porta
            List<Item> listaItens = player.GetComponent<InventarioMissao>().itens;
            if (listaItens.Contains(chave))
            {
                foreach(Item item in listaItens)
                {
                    if(item is Chave)
                    {
                        Chave chave = (Chave)item;
                        if(chave.ID == this.chave.ID)
                        {
                            Destrancar();
                        }
                    }
                }
            }
        }
        else
        {
            //Debug.Log("abriur porta");
            AbrirPorta();
        }
    }

    void Destrancar()
    {
        switch (tipoPorta)
        {
            case TipoPorta.Simples:
                switch (estado)
                {
                    case Estado.NaoLockdown:
                        trancado = false;
                        break;
                    case Estado.Lockdown:
                        break;
                }
                break;

            case TipoPorta.Normal:
                switch (estado)
                {
                    case Estado.NaoLockdown:
                        trancado = false;
                        break;
                    case Estado.Lockdown:
                        break;
                }
                break;

            case TipoPorta.Contencao:
                switch (estado)
                {
                    case Estado.NaoLockdown:
                        break;
                    case Estado.Lockdown:
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
        if (animacao.GetAnimacaoAtual() != "Aberta")
        {
            animacao.TrocarAnimacao("Aberta");
        }
        Door(aberto);
    }

    void ForceFecharPorta()
    {
        aberto = false;
        if (animacao.GetAnimacaoAtual() != "Fechada")
        {
            animacao.TrocarAnimacao("Fechada");
        }
        Door(aberto);
    }

    void Door(bool portaAberta)
    {
        colisao.isTrigger = portaAberta;
    }

    public void AtivarLockDown()
    {
        if (tipoPorta != TipoPorta.Simples)
        {
            Debug.Log("Estado de lockDown,... Trancando");
            estado = Estado.Lockdown;
            trancado = true;
            ForceFecharPorta();

        }

    }
    public void DesativarLockDown()
    {
        if (tipoPorta != TipoPorta.Simples)
        {
            Debug.Log("Estado de lockDown,... Desativando");
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
