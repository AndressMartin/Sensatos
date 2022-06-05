using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TelaMissaoConcluida : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform tela;

    [SerializeField] private TMP_Text nomeDaMissao;

    //Variaveis
    [SerializeField] AudioClip somMissaoConcluida;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        tela.gameObject.SetActive(false);
    }

    private void AtualizarInformacoes(string nomeDaMissao)
    {
        this.nomeDaMissao.text = nomeDaMissao;
    }

    public void IniciarTela(string nomeDaMissao)
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.TelaMissaoConcluida);

        LiberarInput(false);

        AtualizarInformacoes(nomeDaMissao);

        animacao.Play("MissaoConcluida");

        tela.gameObject.SetActive(true);
    }

    public void DesativarTela()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Nenhum);

        LiberarInput(true);

        animacao.Play("Vazio");

        tela.gameObject.SetActive(false);
    }

    private void LiberarInput(bool liberar)
    {
        generalManager.Player.GetComponent<PlayerInput>().enabled = liberar;
    }

    public void AutoSave()
    {
        SaveManager.instance.AutoSave();
    }

    public void TocarSom()
    {
        generalManager.SoundManager.TocarSom(somMissaoConcluida);
    }
}
