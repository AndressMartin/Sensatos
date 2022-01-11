using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private EnemyVisionScript enemyVision;
    private Enemy enemy;
    private EnemyMovement enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyVision = transform.parent.GetComponentInChildren<EnemyVisionScript>();
        enemyMovement = enemy.GetComponent<EnemyMovement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            if (enemyVision.polygonCollider.enabled == false)
            {
                enemyVision.OnAttackRange(true, collision.gameObject);
                enemyMovement.playerOnAttackRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && enemyVision.polygonCollider.enabled == true)
        {
            enemyVision.OnAttackRange(false, null);
            enemyMovement.playerOnAttackRange = false;

        }

        if (collision.gameObject.tag == "Player" && enemyVision.polygonCollider.enabled == false)
        {
            enemyVision.OnAttackRange(false, null);
            enemyMovement.playerOnAttackRange = false;

        }
    }
}
