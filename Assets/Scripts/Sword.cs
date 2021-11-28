using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    //public override string nome { get; protected set; }
    public int dano;

    //definir um sentido para as colis�es 
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private Transform lancaTransform;

    private Transform pontaArma;
    private FisicalAttack fisicalAttack;


    public override void Usar(Player player)
    {
        CreateShoot(player);
    }

    void CreateShoot(Player player)
    {
        pontaArma = player.GetComponentInChildren<PontaArma>().transform;

        Instantiate(lancaTransform, pontaArma); //onde cria a lan�a
        fisicalAttack = FindObjectOfType<FisicalAttack>();
        fisicalAttack.direcao = (FisicalAttack.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direcao;
        fisicalAttack.widthTemp = width;
        fisicalAttack.heightTemp = height;
        fisicalAttack.FatherFromWeapon = player;
        fisicalAttack.dano = dano;

        fisicalAttack.Usou();
    }
}
