using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionHitbox : EntityModel
{
    public Transform transformFatherObject;
    public State state;
    public ParedeModel objetectCollision;

    private SpriteRenderer spriteRenderer; 
    private BoxCollider2D boxCollider2D;

    public bool usandoItem;

    void Start()
    {
        state = GetComponentInParent<State>();
        transformFatherObject = state.transform;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (state.usandoItem)
            ChangeColliderItem(true);
        else
            ChangeColliderItem(false);

       transform.position = FrenteDoPersonagem(transformFatherObject, 0.5F);
    }
    public void ChangeDirection(Direction _direction)
    {
        direction = _direction;
    }

    void ChangeColliderItem(bool _OnOff)
    {
        boxCollider2D.enabled = _OnOff;
        usandoItem = boxCollider2D.enabled;
        if (!_OnOff)
            objetectCollision = null;
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ParedeModel>())
        {

            objetectCollision = collision.gameObject.GetComponent<ParedeModel>();
        }
    }
}
