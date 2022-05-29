using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariaveisGlobais : MonoBehaviour
{
    //Variaveis
    private bool completouUmAssalto;

    //Getters e Setters
    public bool CompletouUmAssalto
    {
        get
        {
            return completouUmAssalto;
        }

        set
        {
            completouUmAssalto = value;
        }
    }

    private void Start()
    {
        completouUmAssalto = false;
    }
}
