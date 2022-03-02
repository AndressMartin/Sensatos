using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject pontoDeSpawn;
    [SerializeField] Enemy prefabInimigo;

    [SerializeField] int quantidadeInimigos;

    [SerializeField] List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;
    private void Start()
    {
        enemyList = new List<Enemy>();
        InstanciarInimigos();
        FindObjectOfType<EnemySpawnManagerScript>().AddToLista(this);
    }
    public void InstanciarInimigos()
    {
        for (int i = 0; i < quantidadeInimigos; i++)
        {
            Enemy enemy;
            enemy = Instantiate(prefabInimigo, transform.position, Quaternion.identity);
            enemy.name = "carlos "+i;
            enemy.SerSpawnado(pontoDeSpawn.transform.position);
            enemyList.Add(enemy);
        }
    }
    public void AtivarDesativarInimigos()
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.gameObject.SetActive(true);
            enemy.SetMortoRespawn(false);
            enemy.Respawn();
        }
    }
    
}
