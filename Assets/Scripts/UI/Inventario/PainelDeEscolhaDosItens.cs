using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainelDeEscolhaDosItens : MonoBehaviour
{
    [SerializeField] private Image[] opcoes;
    
    public Image[] Opcoes => opcoes;

    public void Selecionar(int escolha)
    {
        foreach(Image image in opcoes)
        {
            image.color = new Color(0.2392157f, 0.5568628f, 0.654902f);
        }

        opcoes[escolha].color = Color.white;
    }
}
