using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownManager : MonoBehaviour
{
    ObjectManagerScript objectManagerScript;
    EnemySpawnManager enemySpawnManager;


    [SerializeField]bool emLockdow;
    public bool EmLockdow => emLockdow;

    [SerializeField] float contadorTempoLockdown, contadorTempoLockdownMax;
    void Start()
    {
        objectManagerScript = transform.parent.GetComponentInChildren<ObjectManagerScript>();
        enemySpawnManager = transform.parent.GetComponentInChildren<EnemySpawnManager>();
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
        emLockdow = true;
        contadorTempoLockdown = contadorTempoLockdownMax;
        enemySpawnManager.AtivarInimigos();

        foreach (Enemy enemy in objectManagerScript.listaInimigos)
        {
            enemy.GetComponent<IA_Enemy_Basico>().ReceberLockDown(posicaoDoPlayer);
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
        emLockdow = false;
        foreach (LockDown lockDown in objectManagerScript.listaAlarmes)
        {
            lockDown.DesativarLockDown();
        }

        foreach (Enemy enemy in objectManagerScript.listaInimigos)
        {
            enemy.GetComponent<IA_Enemy_Basico>().DesativarLockDown();
        }
        DestrancarPortas();
    }

    private void TrancarPortas()
    {
        foreach (Porta porta in objectManagerScript.listaPortas)
        {
            porta.AtivarLockDown();
        }     
    }
    private void DestrancarPortas()
    {
        foreach (Porta porta in objectManagerScript.listaPortas)
        {
            porta.DesativarLockDown();
        }  
    }
}
