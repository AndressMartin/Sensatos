using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpcaoIdioma : Opcao
{
    //Componentes
    [SerializeField] private TMP_Text nomeOpcao;
    [SerializeField] private TMP_Text textoEscolhaIdioma;

    [SerializeField] private string nomePortugues;
    [SerializeField] private string nomeIngles;

    [SerializeField] private Animator animacao;

    //Variaveis
    private int selecao;

    [SerializeField] private IdiomaManager.Idioma[] listaIdiomas;

    public override void AtualizarInformacoes(GeneralManagerScript generalManager)
    {
        switch(IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                textoEscolhaIdioma.text = nomePortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                textoEscolhaIdioma.text = nomeIngles;
                break;
        }
    }

    public override void NaOpcao(GeneralManagerScript generalManager)
    {
        if (InputManager.Esquerda() == true)
        {
            if(selecao > 0)
            {
                selecao--;
            }
            else
            {
                selecao = listaIdiomas.Length - 1;
            }

            generalManager.IdiomaManager.SetIdioma(listaIdiomas[selecao]);

            AtualizarInformacoes(generalManager);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }

        if (InputManager.Direita() == true)
        {
            if (selecao < listaIdiomas.Length - 1)
            {
                selecao++;
            }
            else
            {
                selecao = 0;
            }

            generalManager.IdiomaManager.SetIdioma(listaIdiomas[selecao]);

            AtualizarInformacoes(generalManager);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
    }

    public override void Selecionado(bool selecionado)
    {
        for(int i = 0; i < listaIdiomas.Length; i++)
        {
            if(listaIdiomas[i] == IdiomaManager.GetIdiomaEnum)
            {
                selecao = i;
                break;
            }
        }

        if (selecionado == true)
        {
            animacao.SetBool("Selecionado", true);
        }
        else
        {
            animacao.SetBool("Selecionado", false);
        }
    }
}
