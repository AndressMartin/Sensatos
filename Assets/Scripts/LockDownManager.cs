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
  public void VerPlayer(Vector2 posicaoDoPlayer)
    {
        foreach (Enemy enemy in objectManagerScript.listaInimigos)
        {
            enemy.GetComponent<IA_Enemy>().ReceberLockDown(posicaoDoPlayer);
        }
        TrancarPortas();
    }
    public void Respawn()
    {
        DesativaLockDown();
    }
    public void DesativaLockDown()
    {
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
