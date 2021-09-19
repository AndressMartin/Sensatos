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

    public override void Usar(GameObject _objQueChamou)
    {
        if (_objQueChamou.GetComponent<State>().estadoCombate)
        {
            pontaArma = _objQueChamou.GetComponentInChildren<PontaArma>().transform;

            Instantiate(lancaTransform, pontaArma); //onde cria a lança
            fisicalAttack = FindObjectOfType<FisicalAttack>();
            fisicalAttack.direcao = (FisicalAttack.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
            fisicalAttack.widthTemp = width;
            fisicalAttack.heightTemp = height;
            fisicalAttack.FatherFromWeapon = _objQueChamou;
            fisicalAttack.dano = dano;

            fisicalAttack.Usou(this);
        }
    }
}
