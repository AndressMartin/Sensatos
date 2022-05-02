using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LojaDeFerramentas : MonoBehaviour
{
    private bool iniciado = false;

    void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        iniciado = true;
    }
}
