using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownManagerScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    [SerializeField] bool emLockdown;
    public bool EmLockdow => emLockdown;

    [SerializeField] float contadorTempoLockdown, contadorTempoLockdownMax;

    void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void Contador()
    {
        contadorTempoLockdown = contadorTempoLockdownMax;
    }

    public void ContadorLockdownInverso()
    {
        contadorTempoLockdown -= Time.deltaTime;
        if (contadorTempoLockdown <= 0)
        {
            contadorTempoLockdown = 0;
            DesativaLockDown();
        }
    }

    public void AtivarLockDown(Vector2 posicaoDoPlayer)
    {
        emLockdown = true;
        contadorTempoLockdown = contadorTempoLockdownMax;
        generalManager.EnemySpawnManager.AtivarInimigos();

        foreach (Enemy enemy in generalManager.ObjectManager.ListaInimigos)
        {
            enemy.GetIAEnemy.ReceberLockDown(posicaoDoPlayer);
        }

        TrancarPortas();
    }
    public void Respawn()
    {
        DesativaLockDown();
    }
    public void DesativaLockDown()
    {
        contadorTempoLockdown = 0;
        emLockdown = false;
        foreach (LockDown lockDown in generalManager.ObjectManager.ListaAlarmes)
        {
            lockDown.DesativarLockDown();
        }

        foreach (Enemy enemy in generalManager.ObjectManager.ListaInimigos)
        {
            enemy.GetComponent<IAEnemy>().DesativarLockDown();
        }
        DestrancarPortas();
    }

    private void TrancarPortas()
    {
        foreach (Porta porta in generalManager.ObjectManager.ListaPortas)
        {
            porta.AtivarLockDown();
        }     
    }
    private void DestrancarPortas()
    {
        foreach (Porta porta in generalManager.ObjectManager.ListaPortas)
        {
            porta.DesativarLockDown();
        }  
    }
}
