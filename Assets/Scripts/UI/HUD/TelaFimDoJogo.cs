using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaFimDoJogo : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform tela;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        tela.gameObject.SetActive(false);
    }

    public void IniciarFimDoJogo()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Transicao);

        animacao.Play("InicioFimDoJogo");

        tela.gameObject.SetActive(true);
    }

    public void IniciarCreditos()
    {
        animacao.Play("InicioCreditos");
    }

    public void Creditos()
    {
        animacao.Play("Creditos");
    }

    public void VoltarAoMenuPrincipal()
    {
        generalManager.Hud.TransicaoDeTela.TransicaoDeCena("MenuPrincipal");
    }

    public void FazerTransicao()
    {
        generalManager.EventoManager.PosicaoFimDoJogo();
    }

    public void MostrarDialogoFimDoJogo()
    {
        generalManager.EventoManager.MostrarDialogoFimDoJogo();
    }
}
