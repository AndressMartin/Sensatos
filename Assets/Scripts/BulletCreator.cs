using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    // Start is called before the first frame update
    private Projetil bullet;
    private int dano;
    private Transform pontaArma;
    private float velocity;
    private GameObject FatherFromGun;
    private float knockBackValue;
    private float distanciaMaxProjetil;

    public void BulletReference(ArmaDeFogo _armaDeFogo)
    {
        CreateShot(_armaDeFogo);
    }
    void CreateShot (ArmaDeFogo _armaDeFogo)
    {
        pontaArma = _armaDeFogo.pontaArma;
        bullet = _armaDeFogo.projetil;
        dano = _armaDeFogo.dano;
        velocity = _armaDeFogo.velocityProjetil;
        FatherFromGun = _armaDeFogo.objQueChamou;
        knockBackValue = _armaDeFogo.knockbackValue;
        distanciaMaxProjetil = _armaDeFogo.distanciaMaxProjetil;

        Projetil novoProjetil;

        novoProjetil = Instantiate(bullet, pontaArma.position, Quaternion.identity);
        novoProjetil.direcao = (EntityModel.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direcao;
        novoProjetil.dano = dano;
        novoProjetil.FatherFromGun = FatherFromGun;
        novoProjetil.Shooted(pontaArma);
        novoProjetil.transform.parent = transform;
        novoProjetil.velocidadeProjetil = velocity;
        novoProjetil.knockBackValue = knockBackValue;
        novoProjetil.distanciaMaxProjetil = distanciaMaxProjetil;

    }
}
