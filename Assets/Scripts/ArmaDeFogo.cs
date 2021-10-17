using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaDeFogo : Item
{
    [SerializeField] private  Transform bullet;
    private Projetil projetil;
    private BulletCreator bulletCreator;

    public override string nome { get; protected set; }
    public int dano;


    private void Start()
    {
        bulletCreator = FindObjectOfType<BulletCreator>();
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
        Transform pontaArma = _objQueChamou.GetComponentInChildren<PontaArma>().transform;
        bulletCreator.BulletReference(_objQueChamou,bullet,pontaArma);

        /*pontaArma = _objQueChamou.GetComponentInChildren<PontaArma>().transform;
        Instantiate(bullet, pontaArma); //onde cria o projetil
        projetil = FindObjectOfType<Projetil>();
        projetil.direcao = (Projetil.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
        projetil.dano = dano;
        projetil.FatherFromGun = _objQueChamou;
        projetil.Shooted();*/

    }
}
