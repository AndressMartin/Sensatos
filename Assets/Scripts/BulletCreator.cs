using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreator : MonoBehaviour
{
    // Start is called before the first frame update
    private ProjetilScript bullet;
    private int dano;
    private Transform pontaArma;
    private float velocity;
    private EntityModel FatherFromGun;
    private float knockBackValue;
    private float distanciaMaxProjetil;

    public void BulletReference(ArmaDeFogo _armaDeFogo, EntityModel.Direcao direcao)
    {
        CreateShot(_armaDeFogo, direcao);
    }
    void CreateShot (ArmaDeFogo _armaDeFogo, EntityModel.Direcao direcao)
    {
        pontaArma = _armaDeFogo.pontaArma;
        bullet = _armaDeFogo.projetil;
        dano = _armaDeFogo.dano;
        velocity = _armaDeFogo.velocityProjetil;
        FatherFromGun = _armaDeFogo.objQueChamou;
        knockBackValue = _armaDeFogo.knockbackValue;
        distanciaMaxProjetil = _armaDeFogo.distanciaMaxProjetil;

        ProjetilScript novoProjetil;

        novoProjetil = Instantiate(bullet, pontaArma.position, Quaternion.identity);
        novoProjetil.direcao = direcao;
        novoProjetil.dano = dano;
        novoProjetil.FatherFromGun = FatherFromGun;
        novoProjetil.Shooted(pontaArma);
        novoProjetil.transform.parent = transform;
        novoProjetil.velocidadeProjetil = velocity;
        novoProjetil.knockBackValue = knockBackValue;
        novoProjetil.distanciaMaxProjetil = distanciaMaxProjetil;

    }
}
