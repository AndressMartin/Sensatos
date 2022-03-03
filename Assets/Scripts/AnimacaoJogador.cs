using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EntityModel;

public class AnimacaoJogador : MonoBehaviour
{
    //Componentes
    private Animator corpo; //Animator do corpo
    private Animator braco; //Animator dos bracos
    private SpriteRenderer corpoSprite; //SpriteRenderer do corpo
    private SpriteRenderer bracoSprite; //SpriteRenderer dos bracos

    //Variaveis
    private string armaEquipadaVisual; //Guarda a arma equipada
    private string animacaoAtual; //Guarda a animacao atual

    //Getters
    public SpriteRenderer CorpoSprite => corpoSprite;
    public string AnimacaoAtual => animacaoAtual;

    void Start()
    {
        corpo = transform.Find("Corpo").GetComponent<Animator>();
        braco = transform.Find("Bracos").GetComponent<Animator>();
        corpoSprite = transform.Find("Corpo").GetComponent<SpriteRenderer>();
        bracoSprite = transform.Find("Bracos").GetComponent<SpriteRenderer>();
        armaEquipadaVisual = "";
        animacaoAtual = "Idle";
    }

    //Atualiza o valor dos parametros de direcao nos animators
    public void AtualizarDirecao(Direcao direcao, Direcao direcaoMovimento)
    {
        corpo.SetFloat("Direcao", (float)direcao);
        corpo.SetFloat("DirecaoMovimento", (float)direcaoMovimento);
        braco.SetFloat("Direcao", (float)direcao);
        braco.SetFloat("DirecaoMovimento", (float)direcaoMovimento);
    }

    //Troca a animacao atual
    public void TrocarAnimacao(string animacao)
    {
        animacaoAtual = animacao;
        corpo.Play(animacaoAtual);
        braco.Play(armaEquipadaVisual + animacaoAtual);
    }

    //Troca apenas a animacao dos bracos e sincroniza o tempo dela com a do corpo, para quando arma equipada for trocada
    public void AtualizarArmaBracos(string armaEquipada)
    {
        armaEquipadaVisual = armaEquipada;
        braco.Play(armaEquipadaVisual + animacaoAtual, 0, corpo.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    public void Piscar()
    {
        corpoSprite.enabled = !corpoSprite.enabled;
        bracoSprite.enabled = !bracoSprite.enabled;
    }

    public void SetarVisibilidade(bool visivel)
    {
        corpoSprite.enabled = visivel;
        bracoSprite.enabled = visivel;
    }

    public void SetVelocidade(float velocidade)
    {
        corpo.speed = velocidade;
        braco.speed = velocidade;
    }
}