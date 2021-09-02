using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaDeFogo : Item
{
    [SerializeField] private Transform bullet;
    public override string nome { get; protected set; }
    public int dano;
    private Projetil projetil;
    private Transform pontaArma;

    public override void Usar(GameObject objQueChamou)
    {
        pontaArma = objQueChamou.transform;

        Instantiate(bullet, pontaArma); //onnde cria o projetil
        projetil = FindObjectOfType<Projetil>();
        projetil.direcao = (Projetil.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
        projetil.dano = dano;
        projetil.donoDoProjeto = objQueChamou;
        projetil.Shooted(this);
    }
}
