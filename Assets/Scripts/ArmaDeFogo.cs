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
    private Sound sound;
    private void Start()
    {
        sound = FindObjectOfType<Player>().GetComponentInChildren<Sound>();
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
        sound.changeColliderRadius(5);
        pontaArma = _objQueChamou.GetComponentInChildren<PontaArma>().transform;
        objQueChamou = _objQueChamou;
        bulletCreator.BulletReference(this);
    }
}
