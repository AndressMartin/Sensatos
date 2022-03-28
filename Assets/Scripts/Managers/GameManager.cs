using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Instancia do singleton
    public static GameManager instance = null;

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

        //Roda algumas funcoes iniciais do jogo
        FuncoesInicias();
    }

    private void FuncoesInicias()
    {
        //Carrega as configuracoes do arquivo quando o jogo se inicia
        SaveConfiguracoes.CarregarConfiguracoes();

        //Atualiza o idioma
        IdiomaManager idiomaManager = gameObject.AddComponent<IdiomaManager>() as IdiomaManager;
        idiomaManager.SetIdioma(SaveConfiguracoes.configuracoes.idioma);
        Destroy(idiomaManager);
    }
}
