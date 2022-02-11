using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject pontoDeSpawn;
    [SerializeField] Enemy prefabInimigo;
    [SerializeField] List<Transform> moveSpots;
    [SerializeField] int quantidadeInimigos;

    [SerializeField] List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;
    private void Start()
    {
        enemyList = new List<Enemy>();
        InstanciarInimigos();
        FindObjectOfType<EnemySpawnManager>().AddToLista(this);
    }
    public void InstanciarInimigos()
    {
        for (int i = 0; i < quantidadeInimigos; i++)
        {
            Enemy enemy;
            enemy = Instantiate(prefabInimigo);
            enemy.name = "carlos "+i;
            enemy.SerSpawnado(moveSpots,pontoDeSpawn.transform.position);
            enemyList.Add(enemy);
           //enemy.transform.position = pontoDeSpawn.transform.position;
            enemy.gameObject.SetActive(false);
        }
    }
    public void AtivarDesativarInimigos(bool Valor)
    {
        foreach (Enemy enemy in enemyList)
        {
            enemy.gameObject.SetActive(true);
        }
    }
    
}
