using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaDeFogo : Item
{
    [SerializeField] public  Transform bullet;
    private Projetil projetil;
    private BulletCreator bulletCreator;
    public Transform pontaArma;
    public override string nome { get; protected set; }
    public int dano;
    public float velocityProjetil;
    public GameObject objQueChamou;
    public string tempNome;
    public float knockbackValue;
    private void Start()
    {
        bulletCreator = FindObjectOfType<BulletCreator>();
        nome = tempNome;
        if(knockbackValue <=0)
        {
            knockbackValue = 1;
        }
        
    }

    public override void Usar(GameObject objQueChamou)
    {
        if (objQueChamou.gameObject.tag == "Enemy")
        {
            CreateShoot(objQueChamou);
        }

        else if(objQueChamou.GetComponent<State>().estadoCombate)
        {
            CreateShoot(objQueChamou);
        }
    }

    void CreateShoot(GameObject _objQueChamou)
    {
        pontaArma = _objQueChamou.GetComponentInChildren<PontaArma>().transform;
        objQueChamou = _objQueChamou;
        //bulletCreator.BulletReference(_objQueChamou,bullet,pontaArma);
        bulletCreator.BulletReference(this);
        /*pontaArma = _objQueChamou.GetComponentInChildren<PontaArma>().transform;
        Instantiate(bullet, pontaArma); //onde cria o projetil
        projetil = FindObjectOfType<Projetil>();
        projetil.direcao = (Projetil.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
        projetil.dano = dano;
        projetil.FatherFromGun = _objQueChamou;
        projetil.Shooted();*/

    }
}
