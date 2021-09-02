using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaDeFogo : Item
{
    [SerializeField] private Transform bullet;
    public override string nome { get; protected set; }
    public int dano;
    private Projetil projetil;
    private Transform movement;
    private Transform pontaArma;

    void Start()
    {

        movement = FindObjectOfType<Movement>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Usar(GameObject objQueChamou)//onnde cria o projeti
    {
        pontaArma = objQueChamou.transform;

        Instantiate(bullet, movement);
        projetil = FindObjectOfType<Projetil>();
        projetil.direcao = (Projetil.Direcao)pontaArma.GetComponentInChildren<PontaArma>().direction;
        projetil.dano = dano;
        projetil.Shooted(this);
    }
}
