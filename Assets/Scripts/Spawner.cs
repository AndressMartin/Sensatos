using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject pontoDeSpawn;
    [SerializeField] Enemy prefabInimigo;
    [SerializeField] List<Transform> moveSpots;
    [SerializeField] bool i = false;
    [SerializeField] ArmaDeFogo armaDeFogo;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(i)
        {
            i=false;
            Enemy enemy;
            enemy = Instantiate(prefabInimigo);
            enemy.name = "carlos";
            enemy.SerSpawnado(moveSpots,armaDeFogo);
        }
    }
}
