using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAttackRange : MonoBehaviour
{
    EnemyAttackRange enemyAttack;
    void Start()
    {
        enemyAttack = GetComponentInParent<EnemyAttackRange>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            enemyAttack.vendoSubVisao = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            enemyAttack.vendoSubVisao = false;
            //enemyMovement.playerOnAttackRange = false;
        }

    }
}


