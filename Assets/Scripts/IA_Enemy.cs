using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Enemy : MonoBehaviour
{

    //Componentes
    private EnemyMovement enemyMovement;
    private Enemy enemy;
    private EnemyVisionScript enemyVisionScript;
    private ObjectManagerScript objectManagerScript;
    private InventarioEnemy inventarioEnemy;

    //Enun
    enum InimigoEstados { AndandoAtePlayer, Patrulhar, AtacarPlayer, SomPassos, SomTiro, AndandoUltimaPosicaoPlayerConhecida, IndoAtivarLockDown , FicarParado , FazerRotinaLockdow , TomeiDano };
    [SerializeField] InimigoEstados inimigoEstados;

    enum EstadoDeteccaoPlayer { NaoToVendoPlayer, DetectandoPlayer, playerDetectado };
    [SerializeField] EstadoDeteccaoPlayer estadoDeteccaoPlayer;

    enum TipoInimigo {Normal,DeLockdown };
    [SerializeField] TipoInimigo tipoInimigo;

    //Variaveis
    [SerializeField] int municaoNoCarregador;
    [SerializeField] int municaoNoCarregadorMax;

    //Variaveis controle
    
    bool vendoPlayerCircular; //se esta vendo player pela visao redonda
    bool vendoPlayer;
    bool playerAreaAtaque;
    bool emLockDown;
    bool somTiro;
    bool somPasso;
    bool viuPlayerAlgumaVez;
    bool controlodarEsqueciPlayer; //controla o contador para esquecer o player
    bool vouApertarBotao;          //caso tenha visto player alguma vez e o perdeu de vista
    bool fazerRotinaLockDown;
    bool verifiqueiUltimaPosicaoJogador;
    bool tomeiDano;
    bool primeiraVezTomeiDano;
    int indiceDoBotaoMaisPerto;

    Vector2 posicaoTiroPlayer;
    Vector2 posicaoUltimoLugarVisto;
    Vector2 posicaoAtualPlayer;

    //Controladores
    float tempoEntrarEmModoAlerta;
    float tempoEsquecerPlayer;
    float tempoVerificandoUltimaPosicaoPlayer;
    float tempoVerificandoSomTiro;
    float tempoVerificandoSomPassos;
    float tempoVerificandoTomeiTiro;
    float tempoRecarregarArma;


    //Controladores Max
    [SerializeField] float tempoEntrarEmModoAlertaMax;
    [SerializeField] float tempoEsquecerPlayerMax;
    [SerializeField] float tempoVerificandoUltimaPosicaoPlayerMax;
    [SerializeField] float tempoVerificandoSomTiroMax;
    [SerializeField] float tempoVerificandoSomPassosMax;
    [SerializeField] float tempoVerificandoTomeiTiroMax;
    [SerializeField] float tempoRecarregarArmaMax;


    public void SerSpawnado(int municao)
    {
        municaoNoCarregadorMax = municao;
    }
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemy = GetComponent<Enemy>();
        enemyVisionScript = GetComponentInChildren<EnemyVisionScript>();
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();
        inventarioEnemy = GetComponent<InventarioEnemy>();

        inimigoEstados = InimigoEstados.Patrulhar;
        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;

        if(municaoNoCarregadorMax == 0) 
            municaoNoCarregadorMax = inventarioEnemy.ArmaSlot.GetStatus.MunicaoMaxCartucho;

        municaoNoCarregador = municaoNoCarregadorMax;

        vendoPlayerCircular = false; 
        vendoPlayer = false;
        playerAreaAtaque = false;
        emLockDown = false;
        somTiro = false;
        somPasso = false;
        viuPlayerAlgumaVez = false;
        controlodarEsqueciPlayer = false;
        vouApertarBotao = false;
        fazerRotinaLockDown = false;
        verifiqueiUltimaPosicaoJogador = false;
        tomeiDano = false;
        primeiraVezTomeiDano = false;
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

        
        
        #region MaquinaDeEstadosInimigoNoraml
        switch (estadoDeteccaoPlayer)
        {

            case EstadoDeteccaoPlayer.NaoToVendoPlayer://caso não saiba Onde O player esta
                enemy.GetEnemyManager.RemoveToLista(enemy.GetEnemyManager.GetEnemiesQueVemPlayer, enemy);

                if (vendoPlayer)//Caso tenha visto o player
                {
                    enemyMovement.ZerarVelocidade();
                    controlodarEsqueciPlayer = false;
                    estadoDeteccaoPlayer = EstadoDeteccaoPlayer.DetectandoPlayer;
                }
                else
                {
                    bool ouvindoSom = somTiro || somPasso ? true : false;

                    if (viuPlayerAlgumaVez && !emLockDown && !vouApertarBotao )    //caso tenha visto o player alguma vez, nao esteja em lockDown e esteja mais perto do botao de lockdown do que o player           
                    {
                        vouApertarBotao = true;
                        inimigoEstados = InimigoEstados.IndoAtivarLockDown;
                    }

                    else if(ouvindoSom && !vouApertarBotao)   //caso tenha ouvido algo
                    {
                        if(somTiro)
                        {
                            inimigoEstados = InimigoEstados.SomTiro;
                        }
                        else if(somPasso)
                        {
                            inimigoEstados = InimigoEstados.SomPassos;
                        }
                    }

                    else if (!ouvindoSom && fazerRotinaLockDown) // caso receba que é pra fazer a rotina de lockdown
                    {
                        inimigoEstados = InimigoEstados.FazerRotinaLockdow;
                    }

                    else if (!ouvindoSom && !verifiqueiUltimaPosicaoJogador && viuPlayerAlgumaVez && !vouApertarBotao)
                    {
                        inimigoEstados = InimigoEstados.AndandoUltimaPosicaoPlayerConhecida;
                    }

                    else if (!ouvindoSom && !vouApertarBotao) // caso nao receba nada
                    {
                        inimigoEstados = InimigoEstados.Patrulhar;
                    }
                }
                break;

            case EstadoDeteccaoPlayer.DetectandoPlayer: 
                if(emLockDown && vendoPlayer)
                {
                    verifiqueiUltimaPosicaoJogador = false;
                    viuPlayerAlgumaVez = true;
                    estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
                }

                else if(vendoPlayer && !controlodarEsqueciPlayer) //se estiver no estado de detecao e ver o player e o contador for falso soma na detecao
                {
                    if (Contador(ref tempoEntrarEmModoAlerta, tempoEntrarEmModoAlertaMax))
                    {
                        verifiqueiUltimaPosicaoJogador = false;
                        viuPlayerAlgumaVez = true;
                        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
                        //Debug.Log("playerDetectado");            
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
                        //Debug.Log("Achei ter visto um player,me enganei");
                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.FicarParado;
                    }
                }
                break;

            case EstadoDeteccaoPlayer.playerDetectado://enquanto estou sabendo onde o player esta
                enemy.GetEnemyManager.AddToLista(enemy.GetEnemyManager.GetEnemiesQueVemPlayer, enemy);

                vendoPlayer = vendoPlayerCircular;
                if(!controlodarEsqueciPlayer)
                {
                    if(!vendoPlayer)
                    {
                        if (Contador(ref tempoEsquecerPlayer, tempoEsquecerPlayerMax)) //caso nao o veja chamar contador para perder o inimigo de vista
                        {
                            controlodarEsqueciPlayer = true;
                            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
                            //Debug.Log("Perdi o player De vista, indo na sua ultima posicao");
                        }
                    }
                    else // vendo player aumentar a detecao e verificar se esta na zona de ataque
                    {
                        posicaoUltimoLugarVisto = posicaoAtualPlayer;
                        ContadorInverso(ref tempoEsquecerPlayer, tempoEsquecerPlayerMax);

                        if (enemy.GetEnemyManager.VerficiarSeUltimoIntegranteDaLista(enemy.GetEnemyManager.GetEnemiesQueVemPlayer,enemy) && !emLockDown) //terPrioridade para AtivarAlarme
                        {
                            inimigoEstados = InimigoEstados.IndoAtivarLockDown;
                        }
                        
                        else if(playerAreaAtaque)
                        {
                            //Debug.Log("ele ta perto de min posso atirar");
                            inimigoEstados = InimigoEstados.AtacarPlayer;
                        }
                        else
                        {
                            //Debug.Log("Ele ta longe de min nao posso Atacar");
                            inimigoEstados = InimigoEstados.AndandoAtePlayer;
                        }
                        
                    }
                }
                break;
        }
        if (tipoInimigo == TipoInimigo.DeLockdown)
        {
            if (playerAreaAtaque && vendoPlayer)
            {
                Atacar();
                Debug.Log("Atacar");
            }
            else if (vendoPlayer)
            {
                Mover(posicaoAtualPlayer);
                Debug.Log("Mover");
            }
            else
            {
                Patrulhar();
                Debug.Log("Patrulhando");
            }

        }
        #endregion


        if (municaoNoCarregador <= 0)
        {
            if (Contador(ref tempoRecarregarArma, tempoRecarregarArmaMax))
            {
                municaoNoCarregador = municaoNoCarregadorMax;
            }
        }

        if (emLockDown)
        {
            
            if(!vendoPlayer)
            {
                enemy.GetLockDownManager.ContadorLockdownInverso();
            }
            else 
            {
                enemy.GetLockDownManager.ContadorLockdown();
            }
        }

        if (tomeiDano)
        {
            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;

            if (primeiraVezTomeiDano)
            {
                posicaoUltimoLugarVisto = posicaoAtualPlayer;
                primeiraVezTomeiDano = false;
            }
        }

        switch (inimigoEstados)
        {
            case InimigoEstados.AndandoAtePlayer:
                Mover(posicaoAtualPlayer);
                break;

            case InimigoEstados.Patrulhar:
                enemyMovement.Patrulhar();
                break;

            case InimigoEstados.AtacarPlayer:
                Atacar();
                break;
            case InimigoEstados.TomeiDano:
                if (FuncaoIrAteLugarVerificarArea(posicaoUltimoLugarVisto,ref tempoVerificandoTomeiTiro,tempoVerificandoTomeiTiroMax))
                {
                    estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
                    tomeiDano = false;
                }

                break;

            case InimigoEstados.SomPassos:
                if(FuncVerificarArea(ref tempoVerificandoSomPassos,tempoVerificandoSomPassosMax))
                {
                    somPasso = false;
                    //Debug.Log("ouvi alguns passos");
                }
                else
                {
                    enemyMovement.ZerarVelocidade();
                }
                break;

            case InimigoEstados.SomTiro:
                if (FuncaoIrAteLugarVerificarArea(posicaoTiroPlayer, ref tempoVerificandoSomTiro, tempoVerificandoSomTiroMax))
                {
                    somTiro = false;
                    //Debug.Log("pronto ja fui e olhei o som do tiro,");
                }
                break;

            case InimigoEstados.AndandoUltimaPosicaoPlayerConhecida:
                if(FuncaoIrAteLugarVerificarArea(posicaoUltimoLugarVisto,ref tempoVerificandoUltimaPosicaoPlayer, tempoVerificandoUltimaPosicaoPlayerMax))
                {
                    verifiqueiUltimaPosicaoJogador = true;
                    //Debug.Log("pronto ja fui e olhei,");
                }
                break;

            case InimigoEstados.IndoAtivarLockDown:
                if(VerificarChegouAteAlvo(objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].transform.position))
                {
                    AtivarLockDown();
                }
                break;

            case InimigoEstados.FicarParado:
                enemyMovement.ZerarVelocidade();
                break;

            case InimigoEstados.FazerRotinaLockdow:
                if(RotinaLockdow())
                {
                    fazerRotinaLockDown = false;
                }
                break;

            default:
                break;
        }
    }
    bool RotinaLockdow()
    {
        if( VerificarChegouAteAlvo(posicaoUltimoLugarVisto))
        {
            return true;
        }
        return false;
        //fazer a rotina lockdown
    }
    void Patrulhar()
    {
        enemyMovement.Patrulhar();
    }
    void Atacar()
    {
        enemyMovement.ZerarVelocidade();
        if (municaoNoCarregador > 0)
        {
            if (enemy.Atirar()) //Reload
            {
                municaoNoCarregador--;
            }
        }
    }
    void Mover(Vector2 _alvo)
    {
        enemyMovement.Movimentar(enemyMovement.calcMovimemto(_alvo));
    }
    void AtivarLockDown()
    {
        vouApertarBotao = false;
        objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].AtivarLockDown();
        enemy.GetLockDownManager.AtivarLockDown(posicaoUltimoLugarVisto);
    }

    

    #region IrAteLugarVerificarArea
    bool FuncaoIrAteLugarVerificarArea(Vector2 posicaoAlvo,ref float tempo, float tempoMax) // func que verifica se chegou ate o ponto e retorna se ja verificou a area
    {
        if (VerificarChegouAteAlvo(posicaoAlvo)) //olhar pros lados
        {
            if(tempo / tempoMax < 0.25)
            {
                enemy.ChangeDirection(EntityModel.Direcao.Esquerda);
            }
            else if (tempo / tempoMax < 0.50)
            {
                enemy.ChangeDirection(EntityModel.Direcao.Cima);
            }
            else if (tempo / tempoMax < 0.75)
            {
                enemy.ChangeDirection(EntityModel.Direcao.Direita);
            }
            else
            {
                enemy.ChangeDirection(EntityModel.Direcao.Baixo);
            }
            enemyMovement.ZerarVelocidade();

            return FuncVerificarArea(ref tempo, tempoMax);            
        }
        return false;
    }
    bool FuncVerificarArea(ref float tempo, float tempoMax) // func que retorna se terminou de verificar a area
    {
        //Realmente Verificar a area
        //enemyMovement.ZerarVelocidade();
        return Contador(ref tempo, tempoMax);
    }

    bool VerificarChegouAteAlvo(Vector2 alvo)
    {
        if (Vector2.Distance(transform.position, alvo) > 0.5)
        {
            Mover(alvo);
            return false;//se chegou retorna Verdadeiro
        }
        else
        {
            return true;
        }
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
    void ReferenciaVariaveisExternas()//variaves de controle que tem que serem pegas a todo frame
    {
        vendoPlayer = enemyVisionScript.GetVendoPlayer;
        vendoPlayerCircular = enemyVisionScript.GetVendoPlayerCircular;
        playerAreaAtaque = enemy.playerOnAttackRange;
        posicaoAtualPlayer = enemy.GetPlayer.transform.position;
        indiceDoBotaoMaisPerto = RetornarIndiceBotaoLockDownMaisPerto();
    }
    public void ReceberLockDown(Vector2 _posicaoPlayer)
    {
        Debug.Log("recebi o lockdown " + transform.position);
        fazerRotinaLockDown = true;
        emLockDown = true;
        posicaoUltimoLugarVisto = _posicaoPlayer;
    }
    public void DesativarLockDown()
    {
        emLockDown = false;
    }
    public void ReceberSom(Vector2 posicao, bool tiro)
    {
        if (tiro)
        {
            somTiro = true;
            posicaoTiroPlayer = posicao;
        }
        else
        {
            somPasso = true;
        }
    }

    public void ReceberDano()
    {
        primeiraVezTomeiDano = true;
        tomeiDano = true;
    }
    #endregion

    #region ResetarVariaveis/Respawn
    public void Respawn()
    {
        ResetarVariaveis();
    }
    void ResetarVariaveis()
    {
        inimigoEstados = InimigoEstados.Patrulhar;
        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;

        vendoPlayerCircular = false;
        vendoPlayer = false;
        playerAreaAtaque = false;
        emLockDown = false;
        somTiro = false;
        somPasso = false;
        viuPlayerAlgumaVez = false;
        controlodarEsqueciPlayer = false;
        vouApertarBotao = false;
        fazerRotinaLockDown = false;
        verifiqueiUltimaPosicaoJogador = false;
        tomeiDano = false;
        primeiraVezTomeiDano = false;

        municaoNoCarregador = municaoNoCarregadorMax; 
        indiceDoBotaoMaisPerto = 0;

        posicaoTiroPlayer = Vector2.zero;
        posicaoUltimoLugarVisto = Vector2.zero;
        posicaoAtualPlayer = Vector2.zero;

        ResetarContadores();
        enemy.GetEnemyManager.RemoveToLista(enemy.GetEnemyManager.GetEnemiesQueVemPlayer, enemy);

    }
    void ResetarContadores()
    {
        tempoEntrarEmModoAlerta = 0;
        tempoEsquecerPlayer = 0;
        tempoVerificandoUltimaPosicaoPlayer = 0;
        tempoVerificandoSomTiro = 0;
        tempoVerificandoSomPassos = 0;
        tempoVerificandoTomeiTiro =0;
    }

    #endregion
}

