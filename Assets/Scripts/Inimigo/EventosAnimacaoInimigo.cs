using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosAnimacaoInimigo : MonoBehaviour
{
    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
    }

    // Chama a funcao para o inimigo desaparecer
    public void Desaparecer()
    {
        enemy.Desaparecer();
    }
}
