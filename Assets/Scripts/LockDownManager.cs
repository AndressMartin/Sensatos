using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownManager : MonoBehaviour
{
    ObjectManagerScript objectManagerScript;
    void Start()
    {
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VerPlayer(Vector2 posicaoDoPlayer)
    {
        foreach (Enemy enemy in objectManagerScript.listaInimigos)
        {
            enemy.GetComponent<EnemyMove>().LockDownAtivo(posicaoDoPlayer);
        }
        TrancarPortas();
    }
    private void TrancarPortas()
    {
        foreach (Porta porta in objectManagerScript.listaPortas)
        {
            porta.AtivarLockDown();
        }
    }
}