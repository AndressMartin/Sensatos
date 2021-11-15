using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueFisico : MonoBehaviour
{
    public float widthTemp;
    public float heightTemp;
    public int dano;

    private bool atacando;
    private float tempo;


    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private float width;
    private float height;

    public float horizontal, vertical;
    float knockBack;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        width = 2.15f;
        height = 0.75f;

        atacando = false;
        tempo = 0;
    }

    private void FixedUpdate()
    {
        if(atacando == true)
        {
            if(tempo > 0)
            {
                atacando = false;
                boxCollider2D.enabled = false;
            }
            tempo += Time.deltaTime;
        }
    }

    public void Atacar(EntityModel.Direcao _direcao, float knockBack, float _distanceH, float _distanceV, float _distanceY)
    {
        this.knockBack = knockBack;
        switch (_direcao)
        {
            case EntityModel.Direcao.Esquerda:
                boxCollider2D.size = new Vector2(height, width);
                spriteRenderer.size = new Vector2(height, width);
                transform.position = new Vector2(transform.parent.transform.position.x - _distanceH, transform.parent.transform.position.y + _distanceY);
                break;
            case EntityModel.Direcao.Direita:
                boxCollider2D.size = new Vector2(height, width);
                spriteRenderer.size = new Vector2(height, width);
                transform.position = new Vector2(transform.parent.transform.position.x + _distanceH, transform.parent.transform.position.y + _distanceY);
                break;
            case EntityModel.Direcao.Cima:
                boxCollider2D.size = new Vector2(width, height);
                spriteRenderer.size = new Vector2(width, height);
                transform.position = new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.y + _distanceV + _distanceY);
                break;
            case EntityModel.Direcao.Baixo:
                boxCollider2D.size = new Vector2(width, height);
                spriteRenderer.size = new Vector2(width, height);
                transform.position = new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.y - _distanceV + _distanceY);
                break;

        }

        tempo = 0;
        boxCollider2D.enabled = true;
        atacando = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject alvo = collision.gameObject;

        if (alvo.GetComponent<Player>() != null || alvo.GetComponent<Enemy>() != null)
        {
            if (alvo.transform.parent != transform.parent)
                HitTarget(alvo);
        }
    }

    void HitTarget(GameObject alvo)
    {
        //Debug.Log("Entrou no trigger");
        EntityModel temp;

        temp = alvo.GetComponent<EntityModel>();
        temp.TomarDano(dano, horizontal, vertical, knockBack);

        
        if (temp.GetComponentInChildren<EnemyVision>() != null)
        {
            if (temp.GetComponentInChildren<EnemyVision>().polygonCollider.enabled)
            {
                Enemy tempEnemy = temp.GetComponent<Enemy>();
                tempEnemy.stealthKill();
            }
        }
    }
}
