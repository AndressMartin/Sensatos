using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField]private EnemyVision enemyVision;
    private Enemy enemy;
    private EnemyMove enemyMove;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyVision = transform.parent.Find("EnemyVision").gameObject.GetComponent<EnemyVision>();
        enemyMove = enemy.GetComponent<EnemyMove>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();


        if (temp != null)
        {
            if (enemyVision.polygonCollider.enabled == false)
            {
                enemyVision.OnAttackRange(true, collision.gameObject);
                enemyMove.playerOnAttackRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && enemyVision.polygonCollider.enabled == true)
        {
            enemyVision.OnAttackRange(false, null);
            enemyMove.playerOnAttackRange = false;

        }

        if (collision.gameObject.tag == "Player" && enemyVision.polygonCollider.enabled == false)
        {
            enemyVision.OnAttackRange(false, null);
            enemyMove.playerOnAttackRange = false;

        }
    }
}
