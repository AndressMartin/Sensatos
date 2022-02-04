using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownManager : MonoBehaviour
{
    ObjectManagerScript objectManagerScript;
    [SerializeField] float contadorTempoLockdown, contadorTempoLockdownMax;
    void Start()
    {
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();
    }

    public void ContadorLockdown()
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
        contadorTempoLockdown = contadorTempoLockdownMax;
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
        foreach (Enemy enemy in objectManagerScript.listaInimigos)
        {
            enemy.GetComponent<IA_Enemy>().DesativarLockDown();
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
