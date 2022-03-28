using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpcaoSlot : MonoBehaviour
{
    [SerializeField] private Image imagem;
    [SerializeField] private TMP_Text texto;

    public void Selecionado(bool selecionado)
    {
        if (selecionado == true)
        {
            imagem.color = Color.white;
            texto.color = Color.red;
        }
        else
        {
            imagem.color = new Color(0.2392157f, 0.5568628f, 0.654902f);
            texto.color = Color.black;
        }
    }
}
