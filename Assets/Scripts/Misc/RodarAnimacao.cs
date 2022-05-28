using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodarAnimacao : MonoBehaviour
{
    //Componentes
    private Animator animacao;

    //Variaveis
    [SerializeField] private string nomeDaAnimacao;

    void Start()
    {
        TocarAnimacao();
    }

    public void TocarAnimacao()
    {
        if(animacao == null)
        {
            animacao = GetComponent<Animator>();
        }

        animacao.Play(nomeDaAnimacao);
    }
}
