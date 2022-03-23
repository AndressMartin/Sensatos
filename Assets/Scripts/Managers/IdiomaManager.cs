using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdiomaManager : MonoBehaviour
{
    //Enuns
    public enum Idioma { Portugues, Ingles }

    //Variaveis
    private static string idioma;

    //Getters
    public static string GetIdioma => idioma;

    //Setters
    public static void SetIdioma(Idioma idioma)
    {
        switch (idioma)
        {
            case Idioma.Portugues:
                IdiomaManager.idioma = "PT";
                break;

            case Idioma.Ingles:
                IdiomaManager.idioma = "EN";
                break;
        }
    }

    private void Awake()
    {
        if(idioma == null)
        {
            SetIdioma(Idioma.Portugues);
        }
    }
}
