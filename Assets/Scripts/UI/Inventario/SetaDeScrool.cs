using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetaDeScrool : MonoBehaviour
{
    //Componentes
    [SerializeField] private Animator animacao;

    public void Ativa(bool ativa)
    {
        if (ativa == true)
        {
            animacao.SetBool("Selecionado", true);
        }
        else
        {
            animacao.SetBool("Selecionado", false);
        }
    }
}
