using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArma : EntityModel
{
     private BoxCollider2D objPai;
    private GameObject obj;
    private SpriteRenderer spriteRenderer;
    private EntityModel entity;

    void Start()
    {
        objPai = GetComponentInParent<BoxCollider2D>();
        obj = objPai.gameObject;
        entity = obj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = FrenteDoPersonagem(obj.transform,0.5F);
    }
    
}
