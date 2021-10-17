using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    public override string nome { get; protected set; }
    public int dano;

    //definir um sentido para as colis�es 
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private Transform lancaTransform;

    private Transform pontaArma;
    private FisicalAttack fisicalAttack;


    public override void Usar(GameObject _objQueChamou)
    {
        if (_objQueChamou.gameObject.tag == "Enemy")
        {
            CreateShoot(_objQueChamou);
        }

        else if (_objQueChamou.GetComponent<State>().estadoCombate)
        {
            CreateShoot(_objQueChamou);
        }
    }

    void CreateShoot(GameObject __objQueChamou)
    {
        pontaArma = __objQueChamou.GetComponentInChildren<PontaArma>().transform;

        Instantiate(lancaTransform, pontaArma); //onde cria a lan�a
        fisicalAttack = FindObjectOfType<FisicalAttack>();
        fisicalAttack.direcao = (FisicalAttack.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
        fisicalAttack.widthTemp = width;
        fisicalAttack.heightTemp = height;
        fisicalAttack.FatherFromWeapon = __objQueChamou;
        fisicalAttack.dano = dano;

        fisicalAttack.Usou();
    }
}