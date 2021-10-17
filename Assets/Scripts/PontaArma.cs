using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArma : EntityModel
{
     private BoxCollider2D objPai;
    private GameObject obj;
    private SpriteRenderer spriteRenderer;
    private EntityModel entity;
    [SerializeField]private float distanceFromChar;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        objPai = GetComponentInParent<BoxCollider2D>();
        obj = objPai.gameObject;
        entity = obj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case Direction.Direita:
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
                break;

            case Direction.Esquerda:
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
                break;

            case Direction.Cima:
                spriteRenderer.flipY = false;
                break;
          

            case Direction.Baixo:
                spriteRenderer.flipY = true;
                break;

        }

        transform.position = FrenteDoPersonagem(obj.transform,distanceFromChar);

        
    
       
    }
    
}
