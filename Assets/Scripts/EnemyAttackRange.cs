using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField]private EnemyVision enemyVision;
    // Start is called before the first frame update
    void Start()
    {
        enemyVision = transform.parent.Find("EnemyVision").gameObject.GetComponent<EnemyVision>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && enemyVision.polygonCollider.enabled == true)
        {
            enemyVision.OnAttackRange(true,collision.gameObject);
        }

        if (collision.gameObject.tag == "Player" && enemyVision.polygonCollider.enabled == false)
        {
            enemyVision.OnAttackRange(true, collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && enemyVision.polygonCollider.enabled == true)
        {
            enemyVision.OnAttackRange(false, null);
        }

        if (collision.gameObject.tag == "Player" && enemyVision.polygonCollider.enabled == false)
        {
            enemyVision.OnAttackRange(false, null);
        }
    }
}
