using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteragirScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    //Variaveis
    private float width;
    private float height;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;

        //Variaveis
        width = 0.7f;
        height = 1f;
        distance = height / 2;
    }

    public void AtualizarHitBox(EntityModel.Direcao _direcao)
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
    }

    public void Interagir(Player player, EntityModel.Direcao _direcao)
    {
        AtualizarHitBox(_direcao);
        ProcurarInteragivel(player);
    }

    private void ProcurarInteragivel(Player player)
    {
        boxCollider2D.enabled = true;
        foreach (ObjetoInteragivel objetoInteragivel in generalManager.ObjectManager.ListaObjetosInteragiveis)
        {
            if (Colisao.HitTest(boxCollider2D, objetoInteragivel.transform.GetComponent<BoxCollider2D>()))
            {
                if(objetoInteragivel.Ativo == true)
                {
                    objetoInteragivel.Interagir(player);
                    break;
                }
            }
        }
        boxCollider2D.enabled = false;
    }

    public BoxCollider2D GetBoxCollider2D()
    {
        return boxCollider2D;
    }
}
