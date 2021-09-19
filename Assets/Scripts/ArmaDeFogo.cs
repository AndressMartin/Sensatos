using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaDeFogo : Item
{
    [SerializeField] private  Transform bullet;
    private Projetil projetil;
    private Transform pontaArma;

    public override string nome { get; protected set; }
    public int dano;
   

    public override void Usar(GameObject objQueChamou)
    {
        if (objQueChamou.GetComponent<State>().estadoCombate)
        {
            pontaArma = objQueChamou.GetComponentInChildren<PontaArma>().transform;
            
            Instantiate(bullet, pontaArma); //onde cria o projetil
            projetil = FindObjectOfType<Projetil>();
            projetil.direcao = (Projetil.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
            projetil.dano = dano;
            projetil.FatherFromGun = objQueChamou;
            projetil.Shooted(this);
        }
    }
}
