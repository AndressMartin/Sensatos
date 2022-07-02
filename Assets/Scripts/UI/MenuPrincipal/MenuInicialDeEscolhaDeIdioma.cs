using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicialDeEscolhaDeIdioma : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private RectTransform menuOpcoesIdioma;

    [SerializeField] private PainelDeEscolha opcoesIdioma;

    //Variaveis
    private bool ativo;
    private int selecao;

    [SerializeField] private IdiomaManager.Idioma[] listaIdiomas;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        menuOpcoesIdioma.gameObject.SetActive(false);

        //Variaveis
        ativo = false;
        selecao = 0;

        ConferirSave();
    }

    private void Update()
    {
        if (ativo == false || generalManager.PauseManager.PermitirInput == false)
        {
            return;
        }

        EscolherIdioma();
    }

    private void ConferirSave()
    {
        if(SaveConfiguracoes.SaveExiste() == true)
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
        else
        {
            ativo = true;

            menuOpcoesIdioma.gameObject.SetActive(true);

            AtualizarPainelDeEscolha(opcoesIdioma, selecao);
        }
    }

    private void EscolherIdioma()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao > 0)
            {
                selecao--;

                AtualizarPainelDeEscolha(opcoesIdioma, selecao);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }

            generalManager.IdiomaManager.SetIdioma(listaIdiomas[selecao]);
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao < listaIdiomas.Length - 1)
            {
                selecao++;

                AtualizarPainelDeEscolha(opcoesIdioma, selecao);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }

            generalManager.IdiomaManager.SetIdioma(listaIdiomas[selecao]);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            //Salva as configuracoes do jogo
            SaveConfiguracoes.AtualizarConfiguracoes();
            SaveConfiguracoes.SalvarConfiguracoes();

            generalManager.Hud.TransicaoDeTela.TransicaoDeCena("MenuPrincipal");

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);

            ativo = false;
        }
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }
}
