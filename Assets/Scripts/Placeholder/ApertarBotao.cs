using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApertarBotao : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LevelLoaderScript.Instance.CarregarNivel("Mapa_Cidade");
        }
    }
}
