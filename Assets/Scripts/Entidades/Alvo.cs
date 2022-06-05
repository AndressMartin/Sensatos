using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Alvo : EntityModel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private TMP_Text texto;

    private Animator animacao;

    //Variaveis
    [SerializeField] private Ponto[] pontos;
    [SerializeField] private AudioClip somPontos;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        animacao.Play("Vazio");
    }

    public override void TomarDano(int _dano, float _knockBack, float _knockBackTrigger, Vector2 _direcaoKnockBack)
    {
        AtualizarTexto();
        animacao.Play("PontoSubindo", 0, 0);

        generalManager.SoundManager.TocarSom(somPontos);
    }

    private void AtualizarTexto()
    {
        Ponto ponto = pontos[Random.Range(0, pontos.Length)];

        texto.text = ponto.Valor.ToString();
        texto.colorGradientPreset = ponto.Cor;
    }

    [System.Serializable]
    public struct Ponto
    {
        //Variaveis
        [SerializeField] private int valor;
        [SerializeField] private TMP_ColorGradient cor;

        //Getters
        public int Valor => valor;
        public TMP_ColorGradient Cor => cor;
    }
}
