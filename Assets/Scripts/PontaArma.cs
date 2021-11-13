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


        if(obj.tag =="Player")
            entity = obj.GetComponent<Player>();

        if (obj.tag == "Enemy")
            entity = obj.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (direcao)
        {
            case Direcao.Direita:
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
                break;

            case Direcao.Esquerda:
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
                break;

            case Direcao.Cima:
                spriteRenderer.flipY = false;
                break;
          

            case Direcao.Baixo:
                spriteRenderer.flipY = true;
                break;

        }
        direcao = entity.direcao;

        transform.position = FrenteDoPersonagem(obj.transform,distanceFromChar);
 
    }
    
}
