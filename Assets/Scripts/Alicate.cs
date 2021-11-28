using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alicate : Item
{
    [SerializeField] private int dano;
    [SerializeField] private int quantidadeDeUsos;

    public override void Usar(Player player)
    {
        if(player.estado == Player.Estado.Normal)
        {
            BoxCollider2D boxCollider2D = player.GetHitBoxInteracao();
            ObjectManagerScript objectManager = player.GetObjectManager();
            ProcurarCerca(player, boxCollider2D, objectManager);

            if(quantidadeDeUsos <= 0)
            {
                SeDestruir(player);
            }
        }
    }

    private void ProcurarCerca(Player player, BoxCollider2D boxCollider2D, ObjectManagerScript objectManager)
    {
        boxCollider2D.enabled = true;
        foreach (ParedeModel paredeQuebravel in objectManager.listaParedesQuebraveis)
        {
            if (Colisao.HitTest(boxCollider2D, paredeQuebravel.transform.GetComponent<BoxCollider2D>()))
            {
                if(paredeQuebravel.ativo == true)
                {
                    UsarAlicate(paredeQuebravel);
                    break;
                }
            }
        }
        boxCollider2D.enabled = false;
    }

    private void UsarAlicate(ParedeModel paredeQuebravel)
    {
        paredeQuebravel.LevarDano(dano);
        quantidadeDeUsos--;
    }

    private void SeDestruir(Player player)
    {
        player.RemoverDoInventario(this);
        Destroy(this.gameObject);
    }
}