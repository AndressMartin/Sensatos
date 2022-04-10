using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdiomaManager : MonoBehaviour
{
    //Enuns
    public enum Idioma { Portugues, Ingles }

    //Variaveis
    private static Idioma idiomaEnum;
    private static string idioma;

    private UnityEvent eventoTrocarIdioma = new UnityEvent();

    //Getters
    public static string GetIdioma => idioma;
    public static Idioma GetIdiomaEnum => idiomaEnum;
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

        IdiomaManager.idiomaEnum = idioma;

        if (eventoTrocarIdioma != null)
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
