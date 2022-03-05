using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueFisico : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    //Variaveis
    [SerializeField] private int dano;
    private float width;
    private float height;
    private float knockBack;
    private float knockBackTrigger;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;

        //Variaveis
        width = 2.15f;
        height = 0.75f;
    }

    public void Atacar(EntityModel.Direcao _direcao, float knockBack, float knockBackTrigger, float _distanceH, float _distanceV, float _distanceY)
    {
        this.knockBack = knockBack;
        this.knockBackTrigger = knockBackTrigger;

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

        AtacarInimigos(_direcao);
    }

    //Passa pela lista de inimigos, confere se ha colisao com alguns deles e causa dano se houver
    private void AtacarInimigos(EntityModel.Direcao _direcao)
    {
        boxCollider2D.enabled = true;
        foreach (Enemy inimigo in generalManager.ObjectManager.ListaInimigos)
        {
            if (Colisao.HitTest(boxCollider2D, inimigo.transform.Find("HitboxDano").GetComponent<BoxCollider2D>()))
            {
                HitTarget(inimigo, _direcao);
            }
        }
        boxCollider2D.enabled = false;
    }

    void HitTarget(Enemy alvo, EntityModel.Direcao _direcao)
    {  
        alvo.TomarDanoFisico(dano, knockBack, knockBackTrigger, _direcao);
    }
}
