using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransicaoDeTela : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform fundo;

    //Variaveis
    private string nomeDaCena;

    [SerializeField] private bool AparecerQuandoIniciarACena;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        fundo.gameObject.SetActive(false);

        //Variaveis
        nomeDaCena = string.Empty;

        if(AparecerQuandoIniciarACena == true)
        {
            Desaparecer();
        }
    }

    public void Desaparecer()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Transicao);
        generalManager.PauseManager.SetPermitirInput(false);

        animacao.Play("Desaparecendo");

        StartCoroutine(EsperarUmTempo());

        fundo.gameObject.SetActive(true);
    }

    public void TransicaoDeCena(string nomeDaCena)
    {
        GameManager.instance.VariaveisGlobais.CompletouUmAssalto = false;

        this.nomeDaCena = nomeDaCena;

        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Transicao);
        generalManager.PauseManager.SetPermitirInput(false);

        animacao.Play("TransicaoDeTela");

        fundo.gameObject.SetActive(true);
    }

    public void DesativarTela()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Nenhum);
        generalManager.PauseManager.SetPermitirInput(true);

        animacao.Play("Vazio");

        fundo.gameObject.SetActive(false);
    }

    public void TrocarMapa()
    {
        LevelLoaderScript.Instance.CarregarNivel(nomeDaCena);
    }

    public void AutoSave()
    {
        if(GameManager.instance.VariaveisGlobais.CompletouUmAssalto == true)
        {
            SaveManager.instance.AutoSave();

            GameManager.instance.VariaveisGlobais.CompletouUmAssalto = false;
        }
    }

    private IEnumerator EsperarUmTempo()
    {
        animacao.speed = 0;

        yield return new WaitForSeconds(1f);

        animacao.speed = 1;
    }
}
