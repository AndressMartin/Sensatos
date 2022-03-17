using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuDasRoupas : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    //[SerializeField] private SelecaoRoupa[] roupa;

    [SerializeField] private TMP_Text nomeDaRoupa;
    [SerializeField] private TMP_Text descricaoDaRoupa;

    //Variaveis
    private int selecao;
    private int scrool;

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

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Variaveis
        selecao = 0;
        scrool = 0;

        /*
        foreach (SelecaoRoupa selecaoRoupa in roupas)
        {
            selecaoRoupa.Iniciar();
        }
        */

        iniciado = true;
    }
}
