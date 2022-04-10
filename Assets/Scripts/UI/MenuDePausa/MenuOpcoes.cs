using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpcoes : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private Opcao[] opcoes;

    //Variaveis
    private int selecao;

    private bool iniciado = false;

    void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Variaveis
        selecao = 0;

        iniciado = true;
    }

    public void IniciarScrool()
    {
        selecao = 0;

        foreach(Opcao opcao in opcoes)
        {
            opcao.AtualizarInformacoes(generalManager);
        }

        AtualizarScrool();
    }

    private void AtualizarScrool()
    {
        foreach (Opcao opcao in opcoes)
        {
            opcao.Selecionado(false);
        }

        opcoes[selecao].Selecionado(true);
    }

    public void SelecionandoOpcoes()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao > 0)
            {
                selecao--;

                AtualizarScrool();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao < opcoes.Length - 1)
            {
                selecao++;

                AtualizarScrool();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Rodar as funcoes da opcao
        opcoes[selecao].NaOpcao(generalManager);
    }
}
