using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{

    //Variaveis
    private Enemy enemy;
    private EnemyMovementRange movementRange;
    [SerializeField] private bool vendo;
    [SerializeField] private bool colidiuMovimento;
    [SerializeField] private float raioMovementRange;
    
    

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        movementRange = GetComponentInChildren<EnemyMovementRange>();

        movementRange.Iniciar(raioMovementRange, this);
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
        enemy.SetarAttackRange(vendo, colidiuMovimento);
    }
    public void SetarAttackRange(bool _vendo)
    {
        colidiuMovimento = _vendo;
    }
}
