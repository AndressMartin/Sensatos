using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManagerScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private List<Spawner> salasSeguranca;

    //Getters
    public List<Spawner> SalasSeguranca => salasSeguranca;

    private void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void AddToLista(Spawner spawner)
    {
        salasSeguranca.Add(spawner);
    }

    public void AtivarInimigos()
    {
        foreach (Spawner spawner in salasSeguranca)
        {
            if(spawner.Zona == generalManager.ZoneManager.ZonaAtual)
            {
                spawner.AtivarInimigos();
            }
        }
    }
}
