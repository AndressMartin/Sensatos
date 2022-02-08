using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Ferramenta/Alicate")]

public class Alicate : Item
{
    public override Tipo tipo { get; protected set; } = Tipo.Ferramenta;

    [SerializeField] private int dano;
    [SerializeField] private int quantidadeDeUsos;
    [SerializeField] private string nomeAnimacao;

    private ParedeModel paredeQuebravel;

    public override void Usar(Player player)
    {
        if(player.GetEstado == Player.Estado.Normal && quantidadeDeUsos > 0)
        {
            BoxCollider2D boxCollider2D = player.GetHitBoxInteracao();
            ObjectManagerScript objectManager = player.GetObjectManager;
            ProcurarCerca(player, boxCollider2D, objectManager);
        }
    }

    private void ProcurarCerca(Player player, BoxCollider2D boxCollider2D, ObjectManagerScript objectManager)
    {
        boxCollider2D.enabled = true;
        foreach (ParedeModel paredeQuebravel in objectManager.listaParedesQuebraveis)
        {
            if (Colisao.HitTest(boxCollider2D, paredeQuebravel.transform.GetComponent<BoxCollider2D>()))
            {
                if(paredeQuebravel.Ativo == true)
                {
                    this.paredeQuebravel = paredeQuebravel;
                    ChamarAnimacao(player);
                    break;
                }
            }
        }
        boxCollider2D.enabled = false;
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
            SeDestruir(player);
        }
    }

    private void SeDestruir(Player player)
    {
        player.Inventario.RemoveItem(this);
    }

    public override string GetNomeAnimacao()
    {
        return nomeAnimacao;
    }
}