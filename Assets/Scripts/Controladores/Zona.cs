using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zona : MonoBehaviour
{
    private ZoneManagerScript zoneManager;
    [SerializeField] private int zona;

    [SerializeField] private List<Transform> pontosDeProcura;

    //Getters
    public int GetZona => zona;
    public List<Transform> PontosDeProcura => pontosDeProcura;

    public void Iniciar(ZoneManagerScript zoneManager, int zona)
    {
        this.zoneManager = zoneManager;
        this.zona = zona;
    }

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if(colisao.CompareTag("Player"))
        {
            zoneManager.SetZonaAtual(zona);
        }
    }
}
