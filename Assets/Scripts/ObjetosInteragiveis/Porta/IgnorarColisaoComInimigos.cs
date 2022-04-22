using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorarColisaoComInimigos : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<BoxCollider2D>(), true);
        }
    }
}
