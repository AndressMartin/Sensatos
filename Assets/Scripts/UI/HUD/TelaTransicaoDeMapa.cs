using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaTransicaoDeMapa : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform fundo;

    private TransicaoDeMapa transicaoAtual;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        fundo.gameObject.SetActive(false);
    }

    public void IniciarTransicao(TransicaoDeMapa transicaoDeMapa)
    {
        transicaoAtual = transicaoDeMapa;

        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Transicao);
        generalManager.PauseManager.SetPermitirInput(false);

        animacao.Play("Transicao");

        fundo.gameObject.SetActive(true);
    }

    public void DesativarTela()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Nenhum);
        generalManager.PauseManager.SetPermitirInput(true);

        animacao.Play("Vazio");
        fundo.gameObject.SetActive(false);
    }

    public void FazerTransicao()
    {
        transicaoAtual.FazerTransicao();
    }
}
