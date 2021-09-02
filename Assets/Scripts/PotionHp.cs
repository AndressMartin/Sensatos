using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionHp : consumivel
{
    private Player player;
    public int quantidadeCura;
    public int quantidade;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }


    public override void Usar(GameObject objQueChamou)
    {
        player.curar(quantidadeCura);
        quantidade--;
    }
}
