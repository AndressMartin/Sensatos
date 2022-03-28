using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainelDeEscolha : MonoBehaviour
{
    [SerializeField] private OpcaoSlot[] opcoes;
    
    public OpcaoSlot[] Opcoes => opcoes;

    public void Selecionar(int escolha)
    {
        foreach(OpcaoSlot opcao in opcoes)
        {
            opcao.Selecionado(false);
        }

        opcoes[escolha].Selecionado(true);
    }
}
