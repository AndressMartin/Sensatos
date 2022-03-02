using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private Enemy enemy;
    public bool vendoSubVisao;
    [SerializeField]bool vendo;
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    private void Update()
    {
        if(vendo || vendoSubVisao)
        {
            enemy.SetPlayerOnAttackRange(vendoSubVisao);
        }
    }
    

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            vendo = true;
            //enemy.playerOnAttackRange= true;         
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            vendo = false;
            //enemy.playerOnAttackRange = false;
            //enemyMovement.playerOnAttackRange = false;
        }

    }
}
