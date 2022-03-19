using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetaDeScrool : MonoBehaviour
{
    //Componentes
    [SerializeField] private Image imagem;

    public void Ativa(bool ativa)
    {
        if (ativa == true)
        {
            imagem.color = Color.white;
        }
        else
        {
            imagem.color = Color.red;
        }
    }
}
