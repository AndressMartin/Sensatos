using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{

    //Variaveis
    private Enemy enemy;
    private SubAttackRange subAttackRange;
    [SerializeField] private bool vendo;
    [SerializeField] private bool vendoSub;
    [SerializeField] private float raioSubAttack;
    
    

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        subAttackRange = GetComponentInChildren<SubAttackRange>();

        subAttackRange.Iniciar(raioSubAttack, this);
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            vendo = true;
            PassarVisao();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player temp = collision.gameObject.GetComponent<Player>();

        if (temp != null)
        {
            vendo = false;
            PassarVisao();
        }

    }
    void PassarVisao()
    {
        enemy.SetarAttackRange(vendo, vendoSub);
    }
    public void SetarAttackRange(bool _vendo)
    {
        vendoSub = _vendo;
    }
}
