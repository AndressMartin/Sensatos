using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaltoManager : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private MudarIdiomaMissao mudarIdiomaMissao;

    //Variaveis
    private static Assalto assaltoAtual;
    [SerializeField] private List<Assalto> assaltos;

    //Getters
    public Assalto GetAssaltoAtual => assaltoAtual;
    public List<Assalto> Assaltos => assaltos;

    //Setters
    static public void SetAssaltoAtual(Assalto novoAssaltoAtual)
    {
        assaltoAtual = novoAssaltoAtual;
    }

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        mudarIdiomaMissao = GetComponent<MudarIdiomaMissao>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Setar o Assalto Atual, se ele nao for nulo
        if(assaltoAtual != null)
        {
            SetarAssalto(assaltoAtual);
        }

        //Trocar o idioma uma vez para iniciar os objetos com o idioma correto
        TrocarIdioma();
    }

    public void SetarAssalto(Assalto _assalto)
    {
        assaltoAtual = _assalto;
        generalManager.NpcManager.PassarAssalto(_assalto);
        VerificarAssaltoMissao.SetarAssalto(assaltoAtual,generalManager.Player);
    }

    private void TrocarIdioma()
    {
        foreach (Assalto assalto in assaltos)
        {
            assalto.TrocarIdioma();

            foreach (Missao missao in assalto.GetMissoesPrincipais)
            {
                mudarIdiomaMissao.AtualizarIdioma(missao);
            }

            foreach (Missao missao in assalto.GetMissoesSecundarias)
            {
                mudarIdiomaMissao.AtualizarIdioma(missao);
            }
        }
    }
}
