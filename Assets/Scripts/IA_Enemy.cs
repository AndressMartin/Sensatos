using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Enemy : MonoBehaviour
{

    //Componentes
    private Player player;
    private EnemyMovement enemyMovement;
    private Enemy enemy;
    private EnemyVisionScript enemyVisionScript;
    private ObjectManagerScript objectManagerScript;


    //Enun
    enum InimigoEstados { AndandoAtePlayer, Patrulhar, VerificandoArea, AtacarPlayer, SomPassos, SomTiro, AndandoUltimaPosicaoPlayerConhecida, IndoAtivarLockDown , FicarParado };
    [SerializeField] InimigoEstados inimigoEstados;

    enum EstadoDeteccaoPlayer { NaoToVendoPlayer, DetectandoPlayer, playerDetectado };
    [SerializeField] EstadoDeteccaoPlayer estadoDeteccaoPlayer;

    //Variaveis controle
    [SerializeField] bool vendoPlayerCircular;
    [SerializeField] bool vendoPlayer;
    [SerializeField] bool playerAreaAtaque;
    [SerializeField] bool emLockDown;
    [SerializeField] bool fazerRotinaLockDown;
    [SerializeField] bool somTiro;
    [SerializeField] bool somPasso;
    [SerializeField] bool controleParaAtualizarPlayerPosicao;
    [SerializeField] bool viuPlayerAlgumaVez;
    [SerializeField] bool controleParaMudarEntreAlertaOuCombate;
    [SerializeField] bool controleParaSairEntreAlertaOuCombate;
    //------------------------
    bool controladorModoAlertaPlayerTerminou;
    [SerializeField]float tempoEntrarEmModoAlerta, tempoEntrarEmModoAlertaMax;

    bool controlodarEsqueciPlayer;
    [SerializeField] float tempoEsquecerPlayer, tempoEsquecerPlayerMax;

    bool vouApertarBotao = false;

    //-----------------------
    [SerializeField] int indiceDoBotaoMaisPerto;

    [SerializeField] Vector2 posicaoTiroPlayer;
    [SerializeField] Vector2 posicaoUltimoLugarVisto;
    [SerializeField] Vector2 posicaoAtualPlayer;

    //Controladores
     float tempoVerificarAreaLockdown, tempoVerificarAreaLockdownMax;
     float tempoSeguirPlayerAposPerderDeVista, tempoSeguirPlayerAposPerderDeVistaMax;
     float tempoParaEntrarEmCombateComPlayer, tempoParaEntrarEmCombateComPlayerMax;
     float tempoVerificandoSomPassos, tempoVerificandoSomPassosMax;
     float tempoVerificandoSomTiro, tempoVerificandoSomTiroMax;
     float tempoVerificandoUltimaPosicaoPlayer, tempoVerificandoUltimaPosicaoPlayerMax;


    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemy = GetComponent<Enemy>();
        enemyVisionScript = GetComponentInChildren<EnemyVisionScript>();
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();

        inimigoEstados = InimigoEstados.Patrulhar;

        vendoPlayer = false;
        playerAreaAtaque = false;
        emLockDown = false;
        fazerRotinaLockDown = false;
        somPasso = false;
        somTiro = false;
        controleParaAtualizarPlayerPosicao = true;
        viuPlayerAlgumaVez = false;
        controleParaMudarEntreAlertaOuCombate = false;
        controleParaSairEntreAlertaOuCombate = false;


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

        /*if (!vendoPlayer)
        {
            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
        }

        if (viuPlayerAlgumaVez && !emLockDown && //caso não tenha entrado em lockDown,esteja vendo o player e esteja mais perto do botão do que do player
            Vector2.Distance(transform.position, objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].transform.position) < Vector2.Distance(transform.position, enemy.GetPlayer.transform.position))
        {
            inimigoEstados = InimigoEstados.IndoAtivarLockDown;
        }

        if (!vendoPlayer && !controleParaAtualizarPlayerPosicao) //caso tenha perdido o player de vista, por quanto tempo vai atualizar sua posicao
        {
            controleParaAtualizarPlayerPosicao = Contador(ref tempoSeguirPlayerAposPerderDeVista, tempoSeguirPlayerAposPerderDeVistaMax);
            posicaoUltimoLugarVisto = posicaoAtualPlayer;
        }

        if (controleParaMudarEntreAlertaOuCombate && !vendoPlayer)//caso nao estja vendo o player mas ja tenha visto alguma vez
        {
            if (Contador(ref tempoParaEntrarEmCombateComPlayer, tempoParaEntrarEmCombateComPlayerMax)) // tempo Para Entrar em alerta // 
            {
                controleParaMudarEntreAlertaOuCombate = false;
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
        else if (vendoPlayer) // se esta vendo o player
        {
            viuPlayerAlgumaVez = true;
            controleParaMudarEntreAlertaOuCombate = true;
            switch (estadoDeteccaoPlayer)
            {
                case EstadoDeteccaoPlayer.playerDetectado:
                    inimigoEstados = InimigoEstados.AndandoAtePlayer;
                    controleParaAtualizarPlayerPosicao = false;
                    break;

                case EstadoDeteccaoPlayer.Detectando:
                    break;

                case EstadoDeteccaoPlayer.NaoToVendoPlayer:
                    estadoDeteccaoPlayer = EstadoDeteccaoPlayer.Detectando;
                    break;

            }
        }
        else if (somTiro || somPasso) // se ouviu algum passo ou tiro
        {
            if (somTiro)
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
            if (Vector2.Distance(transform.position, posicaoUltimoLugarVisto) > 0.5)
            {
                inimigoEstados = InimigoEstados.AndandoUltimaPosicaoPlayerConhecida;
            }
            else //caso chegue na area em que o jogador estava
            {
                if (VerificandoArea(ref tempoVerificarAreaLockdown, tempoVerificarAreaLockdownMax))
                {
                    inimigoEstados = InimigoEstados.Patrulhar;
                }
            }
        }*/

        switch (estadoDeteccaoPlayer)
        {

            case EstadoDeteccaoPlayer.NaoToVendoPlayer://caso não saiba Onde O player esta

                bool ouvindoAlgo = somTiro || somPasso ? true : false;

                if (vendoPlayer)//Caso tenha visto o player
                {
                    enemyMovement.ZerarVelocidade();
                    controladorModoAlertaPlayerTerminou = false;
                    controlodarEsqueciPlayer = false;
                    estadoDeteccaoPlayer = EstadoDeteccaoPlayer.DetectandoPlayer;
                }
                else
                {
                    if (viuPlayerAlgumaVez && !emLockDown )    //caso tenha visto o player alguma vez, nao esteja em lockDown e esteja mais perto do botao de lockdown do que o player           
                    {
                        vouApertarBotao = true;
                        inimigoEstados = InimigoEstados.IndoAtivarLockDown;
                    }

                    else if(ouvindoAlgo && !vouApertarBotao)   //caso tenha ouvido algo
                    {
                        if(somPasso)
                        {
                            inimigoEstados = InimigoEstados.SomPassos;
                        }
                        else if(somTiro)
                        {
                            inimigoEstados = InimigoEstados.SomTiro;
                        }
                    }
                    else if (!ouvindoAlgo && !vouApertarBotao)
                    {
                        inimigoEstados = InimigoEstados.Patrulhar;
                    }
                }
                break;

            case EstadoDeteccaoPlayer.DetectandoPlayer:
                if(vendoPlayer && !controlodarEsqueciPlayer) //se estiver no estado de detecao e ver o player e o contador for falso soma na detecao
                {
                    if (Contador(ref tempoEntrarEmModoAlerta, tempoEntrarEmModoAlertaMax))
                    {
                        controladorModoAlertaPlayerTerminou = true;
                        viuPlayerAlgumaVez = true;
                        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
                        Debug.Log("playerDetectado");            
                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.FicarParado;
                    }
                }
                else if(!vendoPlayer && !controlodarEsqueciPlayer) //se estiver no estado de detecao e nao ver o player e o contador for falso diminui na detecao
                {
                    if(ContadorInverso(ref tempoEntrarEmModoAlerta, tempoEntrarEmModoAlertaMax))
                    {
                        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
                        Debug.Log("Achei ter visto um player,me enganei");
                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.FicarParado;
                    }
                }
                break;

            case EstadoDeteccaoPlayer.playerDetectado://enquanto estou sabendo onde o player esta

                vendoPlayer = vendoPlayerCircular;
                if(!controlodarEsqueciPlayer)
                {
                    if(!vendoPlayer)
                    {
                        if (Contador(ref tempoEsquecerPlayer, tempoEsquecerPlayerMax)) //caso nao o veja chamar contador para perder o inimigo de vista
                        {
                            controlodarEsqueciPlayer = true;
                            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
                            Debug.Log("Perdi o player De vista indo na sua ultima posicao");
                        }
                    }
                    else // vendo player aumentar a detecao e verificar se esta na zona de ataque
                    {
                        ContadorInverso(ref tempoEsquecerPlayer, tempoEsquecerPlayerMax);

                        if (playerAreaAtaque)
                        {
                            Debug.Log("ele ta perto de min posso atirar");
                            inimigoEstados = InimigoEstados.AtacarPlayer;
                        }
                        else
                        {
                            Debug.Log("Ele ta longe de min nao posso Atacar");
                            inimigoEstados = InimigoEstados.AndandoAtePlayer;
                        }
                    }
                }
                break;
        }

        switch (inimigoEstados)
        {
            case InimigoEstados.AtacarPlayer:

                break;

            case InimigoEstados.AndandoAtePlayer:
                break;
            case InimigoEstados.Patrulhar:
                break;
            case InimigoEstados.VerificandoArea:
                break;       
            case InimigoEstados.SomPassos:
                break;
            case InimigoEstados.SomTiro:
                break;
            case InimigoEstados.AndandoUltimaPosicaoPlayerConhecida:
                break;
            case InimigoEstados.IndoAtivarLockDown:
                break;
            default:
                break;
        }

        /*switch (inimigoEstados)
        {
            case InimigoEstados.AndandoAtePlayer:
                VerificarAndarAteAlvo(posicaoAtualPlayer);

                break;

            case InimigoEstados.Patrulhar:
                if(estadoDeteccaoPlayer == EstadoDeteccaoPlayer.NaoToVendoPlayer)
                Patrulhar();

                break;
            case InimigoEstados.AtacarPlayer:
                Atacar();
                break;

            case InimigoEstados.SomPassos:
                FuncVerificarArea(ref tempoVerificandoSomPassos, tempoVerificandoSomPassosMax, InimigoEstados.Patrulhar);
                break;

            case InimigoEstados.SomTiro:
                FuncaoIrAteLugarVerificarArea(posicaoTiroPlayer, tempoVerificandoSomTiro, tempoVerificandoSomTiroMax, InimigoEstados.Patrulhar);
                break;

            case InimigoEstados.AndandoUltimaPosicaoPlayerConhecida:
                FuncaoIrAteLugarVerificarArea(posicaoUltimoLugarVisto, tempoVerificandoUltimaPosicaoPlayer, tempoVerificandoUltimaPosicaoPlayerMax, InimigoEstados.Patrulhar);
                break;
            case InimigoEstados.IndoAtivarLockDown:
                if (VerificarAndarAteAlvo(objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].transform.position))
                {
                    objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].AtivarLockDown();
                    FindObjectOfType<LockDownManager>().VerPlayer(posicaoUltimoLugarVisto);
                    fazerRotinaLockDown = true;
                }

                break;

            default:
                break;
        }*/
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
        vendoPlayerCircular = enemyVisionScript.GetVendoPlayerCircular;
        playerAreaAtaque = enemy.playerOnAttackRange;
        posicaoAtualPlayer = enemy.GetPlayer.transform.position;
        indiceDoBotaoMaisPerto = RetornarIndiceBotaoLockDownMaisPerto();
    }


    #region IrAteLugarVerificarArea
    void FuncaoIrAteLugarVerificarArea(Vector2 posicaoAlvo, float tempo, float tempoMax, InimigoEstados estadoQueVaiFicarNoFim)
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
        if(Contador(ref tempo, tempoMax))
        {
            inimigoEstados = InimigoEstados.Patrulhar;
            Debug.Log("aqu2asi");
            return Contador(ref tempo, tempoMax);
        }
        Debug.Log("Lá");

        return false;
    }

    #endregion

    bool Contador(ref float tempo, float tempoMax)
    {
        tempo += Time.deltaTime;
        if (tempo > tempoMax)
        {
            tempo = 0;
            return true;
        }
        return false;
    }
    bool ContadorInverso(ref float tempo, float tempoMax)
    {
        tempo -= Time.deltaTime;
        if (tempo < 0)
        {
            tempo = 0;
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
    public void ReceberLockDown(Vector2 _posicaoPlayer)
    {
        emLockDown = true;
        fazerRotinaLockDown = true;
        posicaoUltimoLugarVisto = _posicaoPlayer;
        //mudar Variaveis conforme LockDown
    }
    public void ReceberSom(Vector2 posicao, bool tiro)
    {
        if (tiro)
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
        inimigoEstados = InimigoEstados.Patrulhar;

        vendoPlayer = false;
        playerAreaAtaque = false;
        emLockDown = false;
        fazerRotinaLockDown = false;
        somPasso = false;
        somTiro = false;
        viuPlayerAlgumaVez = false;
        controleParaAtualizarPlayerPosicao = false;
        controleParaMudarEntreAlertaOuCombate = false;
        controleParaSairEntreAlertaOuCombate = false;


        indiceDoBotaoMaisPerto = 0;

        posicaoTiroPlayer = Vector2.zero;
        posicaoUltimoLugarVisto = Vector2.zero;
        posicaoAtualPlayer = Vector2.zero;

        ResetarContadores();
    }

    #endregion
}

