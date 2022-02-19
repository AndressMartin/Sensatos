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

    void Start()
    {
        porta = GetComponent<Animator>();
        portaSprite = GetComponent<SpriteRenderer>();
        animacaoAtual = "Fechada";
    }

    //Troca a animacao atual
    public void TrocarAnimacao(string animacao)
    {
        animacaoAtual = animacao;
        porta.Play(animacaoAtual);
    }
}
