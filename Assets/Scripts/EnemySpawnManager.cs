using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] List<Spawner> salasSeguranca;
    List<Spawner> SalasSeguranca => salasSeguranca;

    public void AddToLista(Spawner spawner)
    {
        salasSeguranca.Add(spawner);

        ObjectManagerScript objectManagerScript;
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();

        foreach (Enemy item in spawner.EnemyList)
        {
            objectManagerScript.adicionarAosInimigos(item);
        }
    }
    public void AtivarInimigos()
    {
        foreach (Spawner spawner in salasSeguranca)
        {
            spawner.AtivarDesativarInimigos(true);
        }
    }
}
