using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaVisual : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private AnimacaoPorta animacao;

    //Variaveis
    [SerializeField] private Porta.Direcao direcao;

    [SerializeField] private ItemChave chave;

    private bool trancado;
    private bool aberto;

    private void Awake()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<AnimacaoPorta>();
    }

    private void Start()
    {
        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);

        //Verificar se tem chave para estar trancada
        if (chave != null)
        {
            trancado = true;
        }
        else
        {
            trancado = false;
        }

        aberto = false;
        AbrirOuFecharPorta();
    }

    public override void Interagir(Player player)
    {
        if (trancado == true)
        {
            //Verifica se ha uma chave nos itens do jogador e se alguma delas tem o id igual ao da chave que destranca a porta
            foreach (ItemChave item in player.InventarioMissao.Itens)
            {
                if (item is ItemChave)
                {
                    if (item.ID == this.chave.ID)
                    {
                        trancado = false;
                        break;
                    }
                }
            }

            if (trancado == true)
            {
                return;
            }
        }

        aberto = !aberto;
        AbrirOuFecharPorta();
    }

    public void AbrirOuFecharPorta()
    {
        if(aberto == true)
        {
            animacao.TrocarAnimacao("Aberta", direcao);
        }
        else
        {
            animacao.TrocarAnimacao("Fechada", direcao);
        }
    }
}
