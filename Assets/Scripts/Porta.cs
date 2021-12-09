using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : ObjetoInteragivel
{
    //Managers
    private ObjectManagerScript objectManager;

    public Chave obj;
    private AnimacaoPorta animacao;
    private BoxCollider2D colisao;

    public enum TipoPorta { simples,normal,Contencao}
    [SerializeField] public TipoPorta tipoPorta;
    private enum Estado {naoLockdown,lockdown }
    [SerializeField]private Estado estado = Estado.naoLockdown;

    bool aberto;
    public bool trancado;

    void Start()
    {
        animacao = GetComponent<AnimacaoPorta>();
        colisao = GetComponent<BoxCollider2D>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager = FindObjectOfType<ObjectManagerScript>();
        objectManager.adicionarAosObjetosInteragiveis(this);
        objectManager.adicionarAsPortas(this);

        if(tipoPorta == TipoPorta.Contencao)
        {
            ForceAbrirPorta();
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
            ForceAbrirPorta();
        }
    }
    void VerificarPortaTrancadaSwitch()
    {
        switch (tipoPorta)
        {
            case TipoPorta.simples:
                switch (estado)
                {
                    case Estado.naoLockdown:
                        trancado = false;
                        break;
                    case Estado.lockdown:
                        break;
                }
                break;

            case TipoPorta.normal:
                switch (estado)
                {
                    case Estado.naoLockdown:
                        trancado = false;
                        break;
                    case Estado.lockdown:
                        break;
                }
                break;

            case TipoPorta.Contencao:
                switch (estado)
                {
                    case Estado.naoLockdown:
                        break;
                    case Estado.lockdown:
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
    void ForceAbrirPorta()
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
        if (tipoPorta != TipoPorta.simples)
        {
            Debug.Log("Estado de lockDown,... Trancando");
            estado = Estado.lockdown;
            trancado = true;
            ForceFecharPorta();

        }

    }
    public void DesativarLockDown()
    {
        if (tipoPorta != TipoPorta.simples)
        {
            Debug.Log("Estado de lockDown,... Desativando");
            estado = Estado.naoLockdown;
            trancado = false;

            switch (tipoPorta)
            {
                case TipoPorta.normal:
                    break;
                case TipoPorta.Contencao:
                    ForceAbrirPorta();
                    break;
                default:
                    break;
            }

        }
    }
}
