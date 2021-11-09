using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField]private EnemyVision enemyVision;
    private Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyVision = transform.parent.Find("EnemyVision").gameObject.GetComponent<EnemyVision>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();


        if (temp != null)
        {
            if (enemyVision.polygonCollider.enabled == false)
            {
                enemyVision.OnAttackRange(true, collision.gameObject);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();
        if (temp != null)
        {
            Debug.Log("Ta me colididno");

            if (enemyVision.polygonCollider.enabled == true)
            {
                if (Input.GetKeyDown(KeyCode.Q))//Botão de interação
                {
                    enemy.stealthKill();
                }
            }
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
