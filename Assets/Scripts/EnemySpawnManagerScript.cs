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
    }

    public void AtivarInimigos()
    {
        foreach (Spawner spawner in salasSeguranca)
        {
            spawner.AtivarDesativarInimigos();
        }
    }
}
