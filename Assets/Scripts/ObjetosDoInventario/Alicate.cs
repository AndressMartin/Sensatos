using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Ferramenta/Alicate")]

public class Alicate : Item
{
    //Variaveis
    public override TipoItem Tipo => TipoItem.Ferramenta;

    [SerializeField] private int dano;
    [SerializeField] private int quantidadeDeUsosMax;
    [SerializeField] private string nomeAnimacao;

    private ParedeModel paredeQuebravel;

    //Getters
    public override string GetNomeAnimacao => nomeAnimacao;
    public int QuantidadeDeUsosMax => quantidadeDeUsosMax;

    public override void Iniciar()
    {
        quantidadeDeUsos = quantidadeDeUsosMax;
    }

    public override bool UsarNoInventario(Player player)
    {
        if (player.GetEstado == Player.Estado.Normal && quantidadeDeUsos > 0)
        {
            BoxCollider2D boxCollider2D = player.GetHitBoxInteracao();
            GeneralManagerScript generalManager = player.GeneralManager;

            if (ProcurarCerca(player, boxCollider2D, generalManager) == true)
            {
                return true;
            }

            return false;
        }
        else
        {
            return false;
        }
    }

    public override void Usar(Player player)
    {
        if(player.GetEstado == Player.Estado.Normal && quantidadeDeUsos > 0)
        {
            BoxCollider2D boxCollider2D = player.GetHitBoxInteracao();
            GeneralManagerScript generalManager = player.GeneralManager;
            
            if(ProcurarCerca(player, boxCollider2D, generalManager) == true)
            {
                ChamarAnimacao(player);
            }
        }
    }

    private bool ProcurarCerca(Player player, BoxCollider2D boxCollider2D, GeneralManagerScript generalManager)
    {
        bool achouCerca = false;

        boxCollider2D.enabled = true;
        foreach (ParedeModel paredeQuebravel in generalManager.ObjectManager.ListaParedesQuebraveis)
        {
            if (Colisao.HitTest(boxCollider2D, paredeQuebravel.transform.GetComponent<BoxCollider2D>()))
            {
                if(paredeQuebravel.Ativo == true)
                {
                    this.paredeQuebravel = paredeQuebravel;
                    achouCerca = true;
                    break;
                }
            }
        }
        boxCollider2D.enabled = false;

        return achouCerca;
    }

    private void ChamarAnimacao(Player player)
    {
        player.AnimacaoItem(this);
    }

    public override void UsarNaGameplay(Player player)
    {
        paredeQuebravel.LevarDano(dano);
        quantidadeDeUsos--;
        if (quantidadeDeUsos <= 0)
        {
            JogarFora(player);
        }
    }
}