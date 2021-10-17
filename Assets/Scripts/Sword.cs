using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    public override string nome { get; protected set; }
    public int dano;

    //definir um sentido para as colisões 
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private Transform lancaTransform;

    private Transform pontaArma;
    private FisicalAttack fisicalAttack;


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
