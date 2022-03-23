using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdiomaManager : MonoBehaviour
{
    //Enuns
    public enum Idioma { Portugues, Ingles }

    //Variaveis
    private static string idioma;

    private UnityEvent eventoTrocarIdioma = new UnityEvent();

    //Getters
    public static string GetIdioma => idioma;
    public UnityEvent EventoTrocarIdioma => eventoTrocarIdioma;

    //Setters
    public void SetIdioma(Idioma idioma)
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

        if(eventoTrocarIdioma != null)
        {
            eventoTrocarIdioma.Invoke();
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
