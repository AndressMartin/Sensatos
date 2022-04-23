using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacaoPorta : MonoBehaviour
{
    private Animator porta; //Animator da porta
    private SpriteRenderer portaSprite; //SpriteRenderer da porta
    private string animacaoAtual; //Guarda a animacao atual

    //Getters
    public string AnimacaoAtual => animacaoAtual;

    void Awake()
    {
        porta = GetComponent<Animator>();
        portaSprite = GetComponent<SpriteRenderer>();
        animacaoAtual = "Fechada";
    }

    //Troca a animacao atual
    public void TrocarAnimacao(string animacao, Porta.Direcao direcao)
    {
        animacaoAtual = animacao + "_" + direcao.ToString();
        porta.Play(animacaoAtual);
    }
}
