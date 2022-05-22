using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpcaoSlot : MonoBehaviour
{
    [SerializeField] private Animator animacao;
    [SerializeField] private TMP_Text texto;

    public void Selecionado(bool selecionado)
    {
        if (selecionado == true)
        {
            animacao.SetBool("Selecionado", true);
        }
        else
        {
            animacao.SetBool("Selecionado", false);
        }
    }
}
