using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityModel
{
    private SpriteRenderer spriteRenderer;


    public override int vida { get; protected set; }
    [SerializeField] private int pontosVida;
    float horizontal;
    float vertical;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        vida = pontosVida;

    }

    // Update is called once per frame
    void Update()
    {
        switch (horizontal)
        {
            case -1:
                direction = Direction.Esquerda;
                break;
            case 1:
                direction = Direction.Direita;
                break;
        }

        switch (vertical)
        {
            case -1:
                direction = Direction.Baixo;
                break;
            case 1:
                direction = Direction.Cima;
                break;
        }
    }

    public override void TomarDano(int _dano)
    {
       

        if (vida <= 0)
            Destroy(gameObject);
        else
        {
            StartCoroutine(Piscar());
            vida -= _dano;
        }

        
    }
    public override IEnumerator Piscar()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
