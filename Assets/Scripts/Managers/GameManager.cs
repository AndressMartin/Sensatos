using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Instancia do singleton
    public static GameManager instance = null;

    //Componentes
    private NomesDeCenas nomesDeCenas;
    private VariaveisGlobais variaveisGlobais;

    //Enuns
    public enum Modo { Cidade, Assalto }
    public enum Capitulo { Inicio, Assalto1 }

    //Variaveis
    private float tempoDeJogo;
    private int assaltosLiberados;

    private Modo modoDeJogo;
    private Capitulo capituloAtual;

    //Getters
    public NomesDeCenas NomesDeCenas => nomesDeCenas;
    public VariaveisGlobais VariaveisGlobais => variaveisGlobais;
    public float TempoDeJogo => tempoDeJogo;
    public Modo ModoDeJogo => modoDeJogo;
    public Capitulo CapituloAtual => capituloAtual;
    public int AssaltosLiberados => assaltosLiberados;

    //Setters
    public void SetTempoDeJogo(float novoTempo)
    {
        tempoDeJogo = novoTempo;
    }

    public void SetAssaltosLiberados(int novoAssaltosLiberados)
    {
        assaltosLiberados = novoAssaltosLiberados;
    }

    public void SetModoDeJogo(Modo novoModoDeJogo)
    {
        modoDeJogo = novoModoDeJogo;
    }

    public void SetCapituloAtual(Capitulo novoCapituloAtual)
    {
        capituloAtual = novoCapituloAtual;
    }

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

        //Componentes
        nomesDeCenas = GetComponent<NomesDeCenas>();
        variaveisGlobais = GetComponent<VariaveisGlobais>();

        //Inicia o contador de tempo
        StartCoroutine(contadorDeTempo());

        //Roda algumas funcoes iniciais do jogo
        FuncoesInicias();
    }

    private void FuncoesInicias()
    {
        //Variaveis
        tempoDeJogo = 0;
        assaltosLiberados = 1;
        modoDeJogo = Modo.Cidade;
        capituloAtual = Capitulo.Inicio;

        //Carrega as configuracoes do arquivo quando o jogo se inicia
        SaveConfiguracoes.CarregarConfiguracoes();

        //Atualiza o idioma
        IdiomaManager idiomaManager = gameObject.AddComponent<IdiomaManager>() as IdiomaManager;
        idiomaManager.SetIdioma(SaveConfiguracoes.configuracoes.idioma);
        Destroy(idiomaManager);
    }

    public void IniciarNovoJogo()
    {
        SaveData.ResetarSaveFile();

        ResetarVariaveisDoJogo();

        IniciarJogo();
    }

    public void IniciarJogo()
    {
        FindObjectOfType<GeneralManagerScript>().Hud.TransicaoDeTela.TransicaoDeCena(nomesDeCenas.Cidade);
    }

    private void ResetarVariaveisDoJogo()
    {
        FindObjectOfType<GeneralManagerScript>().Player.ResetarPlayer();
        FindObjectOfType<IniciadorDoPlayer>().SetarVariaveis();

        tempoDeJogo = 0;
        assaltosLiberados = 0;
        capituloAtual = Capitulo.Inicio;
        Missoes.ResetarEstadosDasMissoes();
        Flags.ResetarFlags();

        variaveisGlobais.CompletouUmAssalto = false;
    }

    private IEnumerator contadorDeTempo()
    {
        yield return new WaitForSecondsRealtime(1);

        while (true)
        {
            tempoDeJogo += 1;

            yield return new WaitForSecondsRealtime(1);
        }
    }
}
