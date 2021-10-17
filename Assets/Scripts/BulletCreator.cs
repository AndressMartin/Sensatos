using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform bullet;
    [SerializeField]private Projetil projetil;
    private int dano;
    private Transform pontaArma;


    public void BulletReference(GameObject _objQueChamou, Transform _bullet, Transform _pontaArma)
    {
        CreateBullet(_objQueChamou,_bullet,_pontaArma);
    }

    private void CreateBullet(GameObject _objQueChamou, Transform _bullet, Transform _pontaArma)
    {
        pontaArma = _pontaArma;
 
        bullet = _bullet;
        Instantiate(bullet, pontaArma); //onde cria o projetil
        projetil = FindObjectOfType<Projetil>();
        projetil.direcao = (Projetil.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
        projetil.dano = dano;
        projetil.FatherFromGun = _objQueChamou;
        projetil.Shooted(pontaArma);
        projetil.transform.parent = transform;
    }

}
