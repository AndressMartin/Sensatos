using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    void Start()
    {
        AtualizarPosicaoCercas();
        CriarParedesQueBloqueiamVisao();
    }

    private void AtualizarPosicaoCercas()
    {
        Cerca[] cercas = FindObjectsOfType<Cerca>(); //Pega todas as cercas na cena, e usado este metodo pois nao ha uma lista de cercas no ObjectManager
        
        foreach (Cerca cerca in cercas)
        {
            cerca.ArrumarPosicao(cercas);
        }
    }

    private void CriarParedesQueBloqueiamVisao()
    {
        DuplicadorDeTileMapsQueBloqueiamVisao[] novasParedes = FindObjectsOfType<DuplicadorDeTileMapsQueBloqueiamVisao>();

        foreach (DuplicadorDeTileMapsQueBloqueiamVisao novasParede in novasParedes)
        {
            novasParede.Duplicar();
        }
    }
}