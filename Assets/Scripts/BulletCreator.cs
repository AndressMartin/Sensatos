using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform bullet;
    [SerializeField] private Projetil projetil;
    private int dano;
    private Transform pontaArma;
    private float velocity;
    private GameObject FatherFromGun;
    private float knockBackValue;

    public void BulletReference(ArmaDeFogo _armaDeFogo)
    {
        //CreateBullet(_objQueChamou,_bullet,_pontaArma);
        CreateShot(_armaDeFogo);
    }
    void CreateShot (ArmaDeFogo _armaDeFogo)
    {
        pontaArma = _armaDeFogo.pontaArma;
        bullet = _armaDeFogo.bullet;
        dano = _armaDeFogo.dano;
        velocity = _armaDeFogo.velocityProjetil;
        FatherFromGun = _armaDeFogo.objQueChamou;
        knockBackValue = _armaDeFogo.knockbackValue;

        Instantiate(bullet, pontaArma);
        projetil = FindObjectOfType<Projetil>();
        projetil.direcao = (EntityModel.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direcao;
        projetil.dano = dano;
        projetil.FatherFromGun = FatherFromGun;
        projetil.Shooted(pontaArma);
        projetil.transform.parent = transform;
        projetil.velocidadeProjetil = velocity;
        projetil.knockBackValue = knockBackValue;


    }

    private void CreateBullet(GameObject _objQueChamou, Transform _bullet, Transform _pontaArma)
    {
        pontaArma = _pontaArma;
        bullet = _bullet;

        Instantiate(bullet, pontaArma); //onde cria o projetil
        projetil = FindObjectOfType<Projetil>();
        projetil.direcao = (EntityModel.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direcao;
        projetil.dano = dano;
        projetil.FatherFromGun = _objQueChamou;
        projetil.Shooted(pontaArma);
        projetil.transform.parent = transform;
        projetil.velocidadeProjetil = 30;
    }

}
