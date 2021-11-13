using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDirectionHitbox : EntityModel
{
    public Transform transformFatherObject;
    public State state;
    public Item objetectCollision;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    public bool usandoInteration;

    void Start()
    {
        state = GetComponentInParent<State>();
        transformFatherObject = state.transform;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (state.interagindo)
            ChangeColliderInteration(true);
        else
            ChangeColliderInteration(false);

        transform.position = FrenteDoPersonagem(transformFatherObject, 0.5F);
    }
    public void ChangeDirection(Direcao _direction)
    {
        direcao = _direction;
    }

    void ChangeColliderInteration(bool _OnOff)
    {
        boxCollider2D.enabled = _OnOff;
        usandoInteration = boxCollider2D.enabled;
        if (!_OnOff)
            objetectCollision = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Item>())
        {
            objetectCollision = collision.gameObject.GetComponent<Item>();
        }
    }
}