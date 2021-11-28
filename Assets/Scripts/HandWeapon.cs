using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWeapon : Item
{
    public virtual float width { get; protected set; }
    public virtual float height { get; protected set; }
    public virtual Transform lancaTransform { get; protected set; }
    public virtual Transform pontaArma { get; protected set; }
    public virtual FisicalAttack fisicalAttack { get; protected set; }
    public virtual int dano { get; protected set; }

    public override void Usar(Player player)
    {
        pontaArma = player.GetComponentInChildren<PontaArma>().transform;

        Instantiate(lancaTransform, pontaArma); //onde cria a lança
        fisicalAttack = FindObjectOfType<FisicalAttack>();
        fisicalAttack.direcao = (FisicalAttack.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direcao;
        fisicalAttack.widthTemp = width;
        fisicalAttack.heightTemp = height;
        fisicalAttack.FatherFromWeapon = player;
        fisicalAttack.dano = dano;

        fisicalAttack.Usou();
    }
}
