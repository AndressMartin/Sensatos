using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Transform bullet;
    public string nome;
    public int dano;
    private Projetil projetil;
    private Transform movement;

    private void Start()
    {
        movement = FindObjectOfType<Movement>().transform;
        
    }

    public void Shoot()
    {
        Instantiate(bullet, movement);

        projetil = FindObjectOfType<Projetil>();
        projetil.Shooted(this);
    }
}
