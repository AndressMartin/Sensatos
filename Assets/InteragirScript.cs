using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteragirScript : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private float width;
    private float height;
    private float distance;

    public float horizontal, vertical;

    private Player player;

    private ObjectManagerScript objectManager;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;

        width = 0.7f;
        height = 1f;
        distance = height / 2;

        player = GetComponentInParent<Player>();

        objectManager = FindObjectOfType<ObjectManagerScript>();
    }

    public void Interagir(EntityModel.Direcao _direcao)
    {
        switch (_direcao)
        {
            case EntityModel.Direcao.Esquerda:
                boxCollider2D.size = new Vector2(height, width);
                spriteRenderer.size = new Vector2(height, width);
                transform.position = new Vector2(transform.parent.transform.position.x - distance, transform.parent.transform.position.y);
                break;
            case EntityModel.Direcao.Direita:
                boxCollider2D.size = new Vector2(height, width);
                spriteRenderer.size = new Vector2(height, width);
                transform.position = new Vector2(transform.parent.transform.position.x + distance, transform.parent.transform.position.y);
                break;
            case EntityModel.Direcao.Cima:
                boxCollider2D.size = new Vector2(width, height);
                spriteRenderer.size = new Vector2(width, height);
                transform.position = new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.y + distance);
                break;
            case EntityModel.Direcao.Baixo:
                boxCollider2D.size = new Vector2(width, height);
                spriteRenderer.size = new Vector2(width, height);
                transform.position = new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.y - distance);
                break;
        }

        boxCollider2D.enabled = true;
        ProcurarInteragivel();
        boxCollider2D.enabled = false;
    }

    private void ProcurarInteragivel()
    {
        foreach (ObjetoInteragivel objetoInteragivel in objectManager.listaObjetosInteragiveis)
        {
            if (Colisao.HitTest(boxCollider2D, objetoInteragivel.transform.GetComponent<BoxCollider2D>()))
            {
                objetoInteragivel.Interagir(player);
                break;
            }
        }
    }
}
