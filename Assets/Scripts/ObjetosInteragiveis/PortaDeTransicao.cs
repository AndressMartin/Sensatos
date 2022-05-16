using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaDeTransicao : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private AnimacaoPorta animacao;
    private TransicaoDeMapaComPorta transicaoDeMapaComPorta;

    //Variaveis
    [SerializeField] private Porta.Direcao direcao;

    private void Awake()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<AnimacaoPorta>();
        transicaoDeMapaComPorta = GetComponent<TransicaoDeMapaComPorta>();
    }

    private void Start()
    {
        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);

        FecharPorta();
    }

    public override void Interagir(Player player)
    {
        transicaoDeMapaComPorta.IniciarTransicao();

        AbrirPorta();
    }

    public void AbrirPorta()
    {
        animacao.TrocarAnimacao("Aberta", direcao);
    }

    public void FecharPorta()
    {
        animacao.TrocarAnimacao("Fechada", direcao);
    }
}
