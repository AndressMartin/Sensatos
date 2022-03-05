using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAttackRange : MonoBehaviour
{
    EnemyAttackRange enemyAttackRange;
    [SerializeField] private bool vendo;
    CircleCollider2D circleCollider;

    public void Iniciar(float _raio,EnemyAttackRange pai)
    {
        enemyAttackRange = pai;
        circleCollider = GetComponent<CircleCollider2D>();
        if (_raio > 0.5)
        {
            circleCollider.radius = _raio;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            vendo = true;
            enemyAttackRange.SetarAttackRange(vendo);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            vendo = false;
            enemyAttackRange.SetarAttackRange(vendo);
        }

    }
}
