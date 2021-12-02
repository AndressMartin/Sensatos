using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmaDeFogo : MonoBehaviour
{
    private BulletCreator bulletCreator;
    [SerializeField] public Projetil projetil;   
    public Transform pontaArma;
    public Sprite myImage;
    public int dano;
    public float velocityProjetil;
    public GameObject objQueChamou;
    public float knockbackValue;
    public int municaoMax;
    public int municaoAtual;
    public int index;
    public string nome;
    public string nomeVisual;
    public float distanciaMaxProjetil;
    private void Start()
    {
        bulletCreator = FindObjectOfType<BulletCreator>();
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
    public void Atirar(GameObject objQueChamou)
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
