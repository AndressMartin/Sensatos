using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosAnimacaoJogador : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }

    // Chama a hitBox de ataque
    public void Atacar()
    {
        player.AtaqueHitBox();
    }

    //Finaliza a animacao e volta ao estado normal
    public void FinalizarAnimacao()
    {
        player.FinalizarAnimacao();
    }

    //Chama a funcao UsarItemGameplay do item atual
    public void UsarItemGameplay()
    {
        player.UsarItemGameplay();
    }

    public void TocarSomAlitace()
    {
        player.SonsDoJogador.TocarSom(SonsDoJogador.Som.UsarOAlicate);
    }
}
