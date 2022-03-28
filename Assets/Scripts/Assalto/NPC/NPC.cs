using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private Missao missao;



    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

            Interagir(collision.GetComponent<Player>());
        }
    }
    public override void Interagir(Player player)
    {
        VerificarAssaltoMissao.VerificarMissao(missao,player);
    }
}
