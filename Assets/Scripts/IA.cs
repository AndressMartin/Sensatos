using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{

    //Componentes
    private Player player;
    private EnemyMovement enemyMovement;
    private Enemy enemy;
    private EnemyVisionScript enemyVisionScript;
    private ObjectManagerScript objectManagerScript;


    //Enun
    enum InimigoEstados { AndandoAtePlayer,Patrulhar,VerificandoArea,AtacarPlayer,SomPassos,SomTiro,AndandoUltimaPosicaoPlayerConhecida,IndoAtivarLockDown};
    [SerializeField] InimigoEstados inimigoEstados;

    enum EstadoDeteccaoPlayer {playerDetectado,Detectando };
    [SerializeField] EstadoDeteccaoPlayer estadoDeteccaoPlayer;

    //Variaveis controle
    bool vendoPlayer;
    bool playerAreaAtaque;
    bool emLockDown;
    bool fazerRotinaLockDown;
    bool somTiro;
    bool somPasso;
    bool controleParaAtualizarPlayerPosicao;
    bool viuPlayerAlgumaVez;
    bool controleParaMudarEntreAlertaOuCombate;

    int indiceDoBotaoMaisPerto;

    Vector2 posicaoTiroPlayer;
    Vector2 posicaoUltimoLugarVisto;
    Vector2 posicaoAtualPlayer;

    //Controladores
    [SerializeField] float tempoVerificarAreaLockdown, tempoVerificarAreaLockdownMax;
    [SerializeField] float tempoSeguirPlayerAposPerderDeVista, tempoSeguirPlayerAposPerderDeVistaMax;
    [SerializeField] float tempoParaEntrarEmCombateComPlayer, tempoParaEntrarEmCombateComPlayerMax;
    [SerializeField] float tempoVerificandoSomPassos, tempoVerificandoSomPassosMax;
    [SerializeField] float tempoVerificandoSomTiro, tempoVerificandoSomTiroMax;
    [SerializeField] float tempoVerificandoUltimaPosicaoPlayer, tempoVerificandoUltimaPosicaoPlayerMax;


    void Start()
    {
        enemyMovement= GetComponent<EnemyMovement>();
        enemy = GetComponent<Enemy>();
        enemyVisionScript = GetComponentInChildren<EnemyVisionScript>();
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();


        vendoPlayer = false;
        playerAreaAtaque = false;
        emLockDown = false;
        fazerRotinaLockDown = false;
        somPasso = false;
        somTiro = false;
        controleParaAtualizarPlayerPosicao = true;
        viuPlayerAlgumaVez = false;
        controleParaMudarEntreAlertaOuCombate = false;

        indiceDoBotaoMaisPerto = 0;


        posicaoTiroPlayer = Vector2.zero;
        posicaoUltimoLugarVisto = Vector2.zero;
        posicaoAtualPlayer = Vector2.zero;

        ResetarContadores();
    }

    // Update is called once per frame
    public void Main()
    {
        ReferenciaVariaveisExternas();



        if(viuPlayerAlgumaVez && !emLockDown && //caso não tenha entrado em lockDown,esteja vendo o player e esteja mais perto do botão do que do player
            Vector2.Distance(transform.position, objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].transform.position) < Vector2.Distance(transform.position, enemy.GetPlayer.transform.position))
        {
            inimigoEstados = InimigoEstados.IndoAtivarLockDown;
        }

        if (!vendoPlayer && !controleParaAtualizarPlayerPosicao) //caso tenha perdido o player de vista, por quanto tempo vai atualizar sua posicao
        {
            controleParaAtualizarPlayerPosicao = Contador(ref tempoSeguirPlayerAposPerderDeVista,tempoSeguirPlayerAposPerderDeVistaMax);
            posicaoUltimoLugarVisto = posicaoAtualPlayer;
        }

        if(!vendoPlayer && controleParaMudarEntreAlertaOuCombate)//caso nao estja vendo o player mas ja tenha visto alguma vez
        {
            if(Contador(ref tempoParaEntrarEmCombateComPlayer,tempoParaEntrarEmCombateComPlayerMax)) // tempo Para Entrar em alerta // 
            {
                controleParaMudarEntreAlertaOuCombate=false;
                estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
            }
            else
            {
                estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
            }
        }
        
        if (playerAreaAtaque && estadoDeteccaoPlayer == EstadoDeteccaoPlayer.playerDetectado) // se player esta no range de ataque
        {
            inimigoEstados = InimigoEstados.AtacarPlayer;
        }
        else if(vendoPlayer) // se esta vendo o player
        {
            viuPlayerAlgumaVez = true;
            switch (estadoDeteccaoPlayer)
            {
                case EstadoDeteccaoPlayer.playerDetectado:
                    inimigoEstados = InimigoEstados.AndandoAtePlayer;
                    controleParaAtualizarPlayerPosicao = false;
                    break;
                case EstadoDeteccaoPlayer.Detectando:
                    controleParaMudarEntreAlertaOuCombate = true;

                    break;

            }
        }
        else if(somTiro || somPasso) // se ouviu algum passo ou tiro
        {
            if(somTiro)
            {
                inimigoEstados = InimigoEstados.SomTiro;
            }
            else if (somPasso)
            {
                inimigoEstados = InimigoEstados.SomPassos;
            }
        }
        else if (fazerRotinaLockDown) //se chamaram lockdown
        {
            if (Vector2.Distance(transform.position,posicaoUltimoLugarVisto) > 0.5)
            {
                inimigoEstados = InimigoEstados.AndandoUltimaPosicaoPlayerConhecida;
            }
            else //caso chegue na area em que o jogador estava
            {
                if (VerificandoArea(ref tempoVerificarAreaLockdown,tempoVerificarAreaLockdownMax))
                {
                    inimigoEstados = InimigoEstados.Patrulhar;
                }
            }
        }

        switch (inimigoEstados)
        {
            case InimigoEstados.AndandoAtePlayer:
                VerificarAndarAteAlvo(posicaoAtualPlayer);
                
                break;

            case InimigoEstados.Patrulhar:
                Patrulhar();

                break;
            case InimigoEstados.AtacarPlayer:
                Atacar();
                break;

            case InimigoEstados.SomPassos:
                FuncVerificarArea(ref tempoVerificandoSomPassos, tempoVerificandoSomPassosMax, InimigoEstados.Patrulhar);
                break;

            case InimigoEstados.SomTiro:
                FuncaoIrAteLugarVerificarArea(posicaoTiroPlayer, ref tempoVerificandoSomTiro, tempoVerificandoSomTiroMax, InimigoEstados.Patrulhar);
                break;

            case InimigoEstados.AndandoUltimaPosicaoPlayerConhecida:
                FuncaoIrAteLugarVerificarArea(posicaoUltimoLugarVisto, ref tempoVerificandoUltimaPosicaoPlayer, tempoVerificandoUltimaPosicaoPlayerMax, InimigoEstados.Patrulhar);
                break;
            case InimigoEstados.IndoAtivarLockDown:
                if(VerificarAndarAteAlvo(objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].transform.position))
                {
                    objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].AtivarLockDown();
                    FindObjectOfType<LockDownManager>().AtivarLockDown(posicaoUltimoLugarVisto);
                    fazerRotinaLockDown = true;
                }
                break;

            default:
                break;
        }
    }
    void Patrulhar()
    {
        enemyMovement.Patrulhar();
    }
    void Atacar()
    {
        enemy.Atirar();
    }
    void Mover(Vector2 _alvo)
    {
        enemyMovement.Movimentar(enemyMovement.calcMovimemto(_alvo));
    }
    void ReferenciaVariaveisExternas()
    {
        vendoPlayer = enemyVisionScript.GetVendoPlayer;
        playerAreaAtaque = enemy.playerOnAttackRange;
        posicaoAtualPlayer = enemy.GetPlayer.transform.position;
        indiceDoBotaoMaisPerto = RetornarIndiceBotaoLockDownMaisPerto();
    }


    #region IrAteLugarVerificarArea
    void FuncaoIrAteLugarVerificarArea(Vector2 posicaoAlvo, ref float tempo, float tempoMax, InimigoEstados estadoQueVaiFicarNoFim)
    {
        if (VerificarAndarAteAlvo(posicaoAlvo))
        {
            FuncVerificarArea(ref tempo, tempoMax, estadoQueVaiFicarNoFim);
        }
    }
    void FuncVerificarArea(ref float tempo, float tempoMax, InimigoEstados estadoQueVaiFicarNoFim)
    {
        if (VerificandoArea(ref tempo, tempoMax))
        {
            inimigoEstados = estadoQueVaiFicarNoFim;
        }
    }
    bool VerificarAndarAteAlvo(Vector2 alvo)
    {
        if (Vector2.Distance(transform.position, alvo) > 0.5)
        {
            Mover(alvo);
            return true;
        }
        return false;
    }
    bool VerificandoArea(ref float tempo, float tempoMax)
    {
        inimigoEstados = InimigoEstados.VerificandoArea;
        return Contador(ref tempo, tempoMax);
    }

    #endregion

    bool Contador(ref float tempo, float tempoMax)
    {
        tempo += Time.deltaTime;
        if (tempo > tempoMax)
        {
            return true;
        }
        return false;
    }
    private int RetornarIndiceBotaoLockDownMaisPerto()
    {
        List<LockDown> botoesLockDown = objectManagerScript.listaAlarmes;

        float menorDistancia = 10000;
        Vector2 botaoMaisProximo = Vector2.zero;
        int indice = 0;
        for (int i = 0; i < botoesLockDown.Count; i++)
        {
            if (Vector2.Distance(transform.position, botoesLockDown[i].transform.position) < menorDistancia)
            {
                botaoMaisProximo = botoesLockDown[i].transform.position;
                menorDistancia = Vector2.Distance(transform.position, botoesLockDown[i].transform.position);
                indice = i;
            }
        }
        return indice;
    }

    #region Receber VariaveisExternas
    public void ReceberLockDown(bool ativo)
    {
        emLockDown = ativo;
        fazerRotinaLockDown = ativo;
        //mudar Variaveis conforme LockDown
    }
    public void ReceberSom(Vector2 posicao,bool tiro)
    {
        if(tiro)
        {
            inimigoEstados = InimigoEstados.SomTiro;
            posicaoTiroPlayer = posicao;
        }
        else
        {
            inimigoEstados = InimigoEstados.SomPassos;
        }
    }

    #endregion

    #region ResetarVariaveis/Respawn
    public void Respawn()
    {
        ResetarVariaveis();
    }
    void ResetarContadores()
    {
        tempoVerificarAreaLockdown = 0;
        tempoSeguirPlayerAposPerderDeVista = 0;
        tempoParaEntrarEmCombateComPlayer = 0;
        tempoVerificandoSomPassos = 0;
        tempoVerificandoSomTiro = 0;
        tempoVerificandoUltimaPosicaoPlayer = 0;
    }
    void ResetarVariaveis()
    {
        vendoPlayer = false;
        playerAreaAtaque = false;
        emLockDown = false;
        fazerRotinaLockDown = false;
        somPasso = false;
        somTiro = false;
        viuPlayerAlgumaVez = false;
        controleParaAtualizarPlayerPosicao = false;
        controleParaMudarEntreAlertaOuCombate = false;

        indiceDoBotaoMaisPerto = 0;

        posicaoTiroPlayer = Vector2.zero;
        posicaoUltimoLugarVisto = Vector2.zero;
        posicaoAtualPlayer = Vector2.zero;

        ResetarContadores();
    }

    #endregion
}
