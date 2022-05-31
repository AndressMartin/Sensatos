using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Itens/Maça")]

public class Maca : Item
{
    //Variaveis
    public override TipoItem Tipo => TipoItem.Consumivel;

    [SerializeField] private int quantidadeDeRecuperacao;
    [SerializeField] private Color corEfeitoRecuperandoVida;
    [SerializeField] private float velocidadeEfeitoRecuperandoVida;
    [SerializeField] private AudioClip somRecuperacao;

    public override bool UsarNoInventario(Player player)
    {
        if (player.Vida < player.VidaMax)
        {
            RecuperarVida(player);
            JogarFora(player);

            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Usar(Player player)
    {
        if (player.Vida < player.VidaMax)
        {
            RecuperarVida(player);
            UsarNaGameplay(player);

            JogarFora(player);
        }
        else
        {
            player.GeneralManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
        }
    }

    private void RecuperarVida(Player player)
    {
        player.RecuperarVida(quantidadeDeRecuperacao);
        player.GeneralManager.SoundManager.TocarSomIgnorandoPause(somRecuperacao);
    }

    public override void UsarNaGameplay(Player player)
    {
        player.Animacao.SetTintSolidEffect(corEfeitoRecuperandoVida, velocidadeEfeitoRecuperandoVida);
    }
}
