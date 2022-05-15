using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModoDeJogoManager : MonoBehaviour
{
    //Variaveis
    [SerializeField] private GameManager.Modo modoDeJogo;

    private bool saveLiberado;

    //Getters
    public bool SaveLiberado
    {
        get
        {
            return saveLiberado;
        }

        set
        {
            saveLiberado = value;
        }
    }

    private void Start()
    {
        GameManager.instance.SetModoDeJogo(modoDeJogo);

        saveLiberado = true;
    }
}
