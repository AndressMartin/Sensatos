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
        //mets();
    }

    public void VerPlayer(Vector2 posicaoDoPlayer)
    {
        foreach (Enemy enemy in objectManagerScript.listaInimigos)
        {
            enemy.GetComponent<EnemyMovement>().LockDownAtivo(posicaoDoPlayer);
        }
        TrancarPortas();
       

    }
    public void DesativaLockDown()
    {
        DestrancarPortas();
    }
    void mets()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            VerPlayer(FindObjectOfType<Player>().gameObject.transform.position);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            DestrancarPortas();
        }
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
