using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    private Enemy enemy;
    private bool vendoSubVisao;
    [SerializeField] private bool vendo;

    //Setters
    public void SetVendoSubVisao(bool ativo)
    {
        vendoSubVisao = ativo;
    }

    void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();
        enemy = GetComponentInParent<Enemy>();
    }
    private void Update()
    {
        if(generalManager.PauseManager.JogoPausado == false)
        {
            if (vendo || vendoSubVisao)
            {
                enemy.SetPlayerOnAttackRange(vendoSubVisao);
            }
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
