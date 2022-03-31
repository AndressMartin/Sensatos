using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MudarIdiomaTexto : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private TMP_Text texto;

    //Variaveis
    [SerializeField] [TextArea(3, 6)] private string textoPortugues;
    [SerializeField] [TextArea(3, 6)] private string textoIngles;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Componentes
        texto = GetComponent<TMP_Text>();

        //Trocar o idioma uma vez para iniciar o objeto com o idioma correto
        TrocarIdioma();
    }

    private void TrocarIdioma()
    {
        switch(IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                texto.text = textoPortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                texto.text = textoIngles;
                break;
        }
    }
}