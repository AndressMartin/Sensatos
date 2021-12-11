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
    public Chave obj;
    private bool aberto;
    public bool trancado;

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

        if(tipoPorta == TipoPorta.Contencao)
        {
            AbrirPorta();
        }
    }
    public override void Interagir(Player player)
    {
        if (player.GetComponent<InventarioMissao>().itens.Contains(obj) && trancado)
        {
            //Debug.Log("destrancou porta");
            VerificarPortaTrancadaSwitch();
        }

        if (!trancado)
        {
            //Debug.Log("abriur porta");
            AbrirPorta();
        }
    }
    void VerificarPortaTrancadaSwitch()
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
