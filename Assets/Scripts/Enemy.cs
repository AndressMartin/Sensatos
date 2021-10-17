using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityModel
{
    public override int vida { get; protected set; }
    public enum ModoPatrulha { ronda,parado };
    public ModoPatrulha modoPatrulha;

    private SpriteRenderer spriteRenderer;
    private Inventario inventario;
    private EnemyMove enemyMove;

    
    [SerializeField] private int pontosVida;
    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = GetComponent<EnemyMove>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        inventario = GetComponent<Inventario>();
        vida = pontosVida;

    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Direction.Cima;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Direction.Baixo;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Direction.Esquerda;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Direction.Direita;
        }*/

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

    public void UseItem()
    {
        inventario.UsarItemAtual();
    }

    public override void TomarDano(int _dano, float _horizontal, float _vertical)
    {
       
        if (vida <= 0)
        { 
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(Piscar());
            KnockBack(_horizontal, _vertical);
            vida -= _dano;
        }

        
    }
    public void ChangeDirection(Direction _direction)
    {
        direction = _direction;
    }

    public override IEnumerator Piscar()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public override void KnockBack(float _horizontal, float _vertical)
    {
        enemyMove.KnockBack(_horizontal, _vertical);
        
    }
}
