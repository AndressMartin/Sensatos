using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]private List<Enemy> enemies = new List<Enemy>();
    [SerializeField]private List<Enemy> enemiesQueVemPlayer = new List<Enemy>();
    public List<Enemy> GetEnemies => enemies;
    public List<Enemy> GetEnemiesQueVemPlayer => enemiesQueVemPlayer;

    public void AddToLista(List<Enemy> lista,Enemy valor)
    {
        if(!lista.Contains(valor))
        {
            lista.Add(valor);
        }
    }
    public void RemoveToLista(List<Enemy> lista, Enemy valor)
    {
        if (lista.Contains(valor))
        {
            lista.Remove(valor);
        }
    }
    public bool VerficiarSeUltimoIntegranteDaLista(List<Enemy> lista,Enemy enemy)
    {
        if(lista[lista.Count -1] == enemy && lista.Count > 1)
        {
            return true;
        }
        return false;
    }

}
