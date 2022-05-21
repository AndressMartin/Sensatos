using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacao : MonoBehaviour
{
    private Animator animacao; //Animator
    private string animacaoAtual; //Guarda a animacao atual

    //Getters
    public string AnimacaoAtual => animacaoAtual;

    void Awake()
    {
        animacao = GetComponent<Animator>();
        animacaoAtual = string.Empty;
    }

    //Troca a animacao atual
    public void TrocarAnimacao(string animacao)
    {
        animacaoAtual = animacao;
        this.animacao.Play(animacaoAtual);
    }

    //Atualiza o valor dos parametros de direcao no animator, se ele os tiver
    public void AtualizarDirecao(EntityModel.Direcao direcao)
    {
        animacao.SetFloat("Direcao", (float)direcao);
    }
}
