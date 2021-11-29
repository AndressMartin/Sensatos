using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chamarAtaque : MonoBehaviour
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

    //Finaliza o ataque
    public void FinalizarAtaque()
    {
        player.FinalizarAtaque();
    }
}
