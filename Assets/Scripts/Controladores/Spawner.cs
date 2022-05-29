using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Variaveis
    [SerializeField] LayerMask layerDasZonas;

    [SerializeField] Enemy prefabInimigo;
    [SerializeField] int quantidadeInimigos;

    private int zona;
    List<Enemy> enemyList;
    //Getters
    public List<Enemy> EnemyList => enemyList;
    public int Zona => zona;

    private void Start()
    {
        enemyList = new List<Enemy>();
        InstanciarInimigos();
        FindObjectOfType<EnemySpawnManagerScript>().AddToLista(this);

        SetarZona();
    }

    private void SetarZona()
    {
        BoxCollider2D colisao = GetComponent<BoxCollider2D>();

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(colisao.bounds.center, colisao.bounds.extents, 0, layerDasZonas);
        foreach (Collider2D objeto in hitColliders)
        {
            if (objeto.GetComponent<Zona>())
            {
                zona = objeto.GetComponent<Zona>().GetZona;
                break;
            }
        }

        colisao.enabled = false;
    }

    public void InstanciarInimigos()
    {
        for (int i = 0; i < quantidadeInimigos; i++)
        {
            Enemy enemy;
            enemy = Instantiate(prefabInimigo, transform.position, Quaternion.identity);
            enemy.name = "carlos " + i;
            enemy.SerSpawnado(transform.position);
            enemyList.Add(enemy);
        }
    }

    public void AtivarInimigos()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (!enemy.gameObject.activeSelf || enemy.Morto == true)
            {
                enemy.gameObject.SetActive(true);
                enemy.SetMortoRespawn(false);
                enemy.Respawn();
            }
        }
    }
    
}
