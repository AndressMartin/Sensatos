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

    void Start()
    {
        movement = FindObjectOfType<Movement>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Usar()//onnde cria o projeti
    {
        Instantiate(bullet, movement);
        projetil = FindObjectOfType<Projetil>();
        projetil.direcao = (Projetil.Direcao)movement.gameObject.GetComponent<Movement>().direction;
        projetil.dano = dano;
        projetil.Shooted(this);
    }
}
