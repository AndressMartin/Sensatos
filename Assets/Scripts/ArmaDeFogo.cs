using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmaDeFogo : Item
{
    [SerializeField] public  Transform bullet;
    private Projetil projetil;
    private BulletCreator bulletCreator;
    public Transform pontaArma;
    public Sprite myImage;
    public override string nome { get; protected set; }
    public int dano;
    public float velocityProjetil;
    public GameObject objQueChamou;
    public string tempNome;
    public float knockbackValue;
    public int municaoMax;
    public int municaoAtual;
    public int index;
    public string nomeVisual;
    private void Start()
    {
        bulletCreator = FindObjectOfType<BulletCreator>();
        nome = tempNome;
        municaoAtual = municaoMax; //TODO: Precisa salvar essa info
        if(knockbackValue <=0)
        {
            knockbackValue = 1;
        }
        
    }

    public void AtualizarBulletCreator(BulletCreator bulletCreator)
    {
        this.bulletCreator = bulletCreator;
    }
    public override void Usar(GameObject objQueChamou)
    {
        CreateShoot(objQueChamou);
    }

    void CreateShoot(GameObject _objQueChamou)
    {
        pontaArma = _objQueChamou.GetComponentInChildren<PontaArma>().transform;
        objQueChamou = _objQueChamou;
        bulletCreator.BulletReference(this);
    }
}
