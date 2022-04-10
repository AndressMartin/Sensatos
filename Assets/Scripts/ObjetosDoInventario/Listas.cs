using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listas : MonoBehaviour
{
    //Instancia do singleton
    public static Listas instance = null;

    //Variaveis
    [SerializeField] private ListaDeArmas listaDeArmas;
    [SerializeField] private ListaDeItens listaDeItens;
    [SerializeField] private ListaDeRoupas listaDeRoupas;
    [SerializeField] private ListaDeMissoes listaDeMissoes;

    //Getters
    public ListaDeArmas ListaDeArmas => listaDeArmas;
    public ListaDeItens ListaDeItens => listaDeItens;
    public ListaDeRoupas ListaDeRoupas => listaDeRoupas;
    public ListaDeMissoes ListaDeMissoes => listaDeMissoes;

    private void Awake()
    {
        //Faz do script um singleton
        if (instance == null) //Confere se a instancia nao e nula
        {
            instance = this;
        }
        else if (instance != this) //Caso a instancia nao seja nula e nao seja este objeto, ele se destroi
        {
            Destroy(gameObject);
            return;
        }

        //Caso o objeto esteja sendo criado pela primeira vez, marca ela para nao ser destruido em mudancas de cenas
        DontDestroyOnLoad(transform.gameObject);

        //Inicia os dicionarios das listas
        listaDeArmas.Iniciar();
        listaDeItens.Iniciar();
        listaDeRoupas.Iniciar();
        listaDeMissoes.Iniciar();
    }
}
