using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {           
            enemy.playerOnAttackRange= true;         
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            enemy.playerOnAttackRange = false;
            //enemyMovement.playerOnAttackRange = false;
        }

    }
}
