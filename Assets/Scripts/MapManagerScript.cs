using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AtualizarPosicaoCercas());
    }

    private IEnumerator AtualizarPosicaoCercas()
    {
        yield return null; //Aguarda uma frame para todas as cercas poderem usar o metodo Start
        Cerca[] cercas = FindObjectsOfType<Cerca>(); //Pega todas as cercas na cena, e usado este metodo pois nao ha uma lista de cercas no ObjectManager
        
        foreach (Cerca cerca in cercas)
        {
            cerca.ArrumarPosicao(cercas);
        }
    }
}
