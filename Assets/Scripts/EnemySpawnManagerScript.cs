using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManagerScript : MonoBehaviour
{
    [SerializeField] private List<Spawner> salasSeguranca;
    public List<Spawner> SalasSeguranca => salasSeguranca;

    public void AddToLista(Spawner spawner)
    {
        salasSeguranca.Add(spawner);

        GeneralManagerScript generalManager = FindObjectOfType<GeneralManagerScript>();

        foreach (Enemy item in spawner.EnemyList)
        {
            generalManager.ObjectManager.AdicionarAosInimigos(item);
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
