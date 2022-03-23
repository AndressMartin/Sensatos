using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteIdioma : MonoBehaviour
{
    private GeneralManagerScript generalManager;

    [SerializeField] private string idioma;

    private void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();
        idioma = IdiomaManager.GetIdioma;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            IdiomaManager.SetIdioma(IdiomaManager.Idioma.Portugues);
            idioma = IdiomaManager.GetIdioma;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            IdiomaManager.SetIdioma(IdiomaManager.Idioma.Ingles);
            idioma = IdiomaManager.GetIdioma;
        }
    }
}
