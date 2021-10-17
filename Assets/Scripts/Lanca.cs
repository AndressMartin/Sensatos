using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanca : Item
{
    public override string nome { get; protected set; }
    public int dano;

    //definir um sentido para as colisões 
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private Transform lancaTransform;

    private Transform pontaArma;
    private FisicalAttack FisicalAttack;
 

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

        Instantiate(lancaTransform, pontaArma); //onde cria a lança
        FisicalAttack = FindObjectOfType<FisicalAttack>();
        FisicalAttack.direcao = (FisicalAttack.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
        FisicalAttack.widthTemp = width;
        FisicalAttack.heightTemp = height;
        FisicalAttack.FatherFromWeapon = __objQueChamou;
        FisicalAttack.dano = dano;

        FisicalAttack.Usou();
    }

    /*
  *  public override string nome { get; protected set; }
 public override int dano { get; protected set; }

 //definir um sentido para as colisões 
 public override float width { get; protected set; }
 public override float height { get; protected set; }
 public override Transform lancaTransform { get; protected set; }

 public override Transform pontaArma { get; protected set; }
 public override FisicalAttack fisicalAttack { get; protected set; }


 public override void Usar(GameObject _objQueChamou)
 {
     base.Usar(_objQueChamou);
 }
  */
}
