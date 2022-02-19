using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEnemy : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    protected EnemyMovement enemyMovement;
    protected Enemy enemy;
    protected EnemyVisionScript enemyVisionScript;
    protected InventarioEnemy inventarioEnemy;
    protected AIPath aiPath;
    
    //Enuns
    protected enum InimigoEstados { AndandoAtePlayer, Patrulhar, AtacarPlayer, SomPassos, SomTiro, AndandoUltimaPosicaoPlayerConhecida, IndoAtivarLockDown , FicarParado , FazerRotinaLockdow , TomeiDano };
    [SerializeField] protected InimigoEstados inimigoEstados;

    protected enum EstadoDeteccaoPlayer { NaoToVendoPlayer, DetectandoPlayer, playerDetectado };
    [SerializeField] protected EstadoDeteccaoPlayer estadoDeteccaoPlayer;

    //Variaveis
    protected int municaoNoCarregador;
    protected int municaoNoCarregadorMax;

    protected bool iniciado = false;

    //Variaveis controle

    public bool vendoPlayerCircular; //se esta vendo player pela visao redonda
    public bool vendoPlayer;
    protected bool playerAreaAtaque;
    protected bool emLockDown;
    protected bool somTiro;
    protected bool somPasso;
    protected bool viuPlayerAlgumaVez;
    protected bool controlodarEsqueciPlayer; //controla o contador para esquecer o player
    protected bool vouApertarBotao;          //caso tenha visto player alguma vez e o perdeu de vista
    protected bool fazerRotinaLockDown;
    protected bool verifiqueiUltimaPosicaoJogador;
    protected bool tomeiDano;
    protected bool primeiraVezTomeiDano;
    protected int indiceDoBotaoMaisPerto;

    bool presenteNaListaDeDeteccao;
    int posicaoListaIndiceDeteccao;

    protected Vector2 posicaoTiroPlayer;
    protected Vector2 posicaoUltimoLugarVisto;
    protected Vector2 posicaoAtualPlayer;
    [SerializeField]protected Vector2 posicaoInicial;

    //Controladores
    protected float tempoEntrarEmModoAlerta;
    protected float tempoEsquecerPlayer;
    protected float tempoVerificandoUltimaPosicaoPlayer;
    protected float tempoVerificandoSomTiro;
    protected float tempoVerificandoSomPassos;
    protected float tempoVerificandoTomeiTiro;
    protected float tempoRecarregarArma;


    //Controladores Max
    [SerializeField] protected float tempoEntrarEmModoAlertaMax;
    [SerializeField] protected float tempoEsquecerPlayerMax;
    [SerializeField] protected float tempoVerificandoUltimaPosicaoPlayerMax;
    [SerializeField] protected float tempoVerificandoSomTiroMax;
    [SerializeField] protected float tempoVerificandoSomPassosMax;
    [SerializeField] protected float tempoVerificandoTomeiTiroMax;
    protected float tempoRecarregarArmaMax;

    public virtual void Start()
    {
        Iniciar();
    }

    public virtual void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        enemyMovement = GetComponent<EnemyMovement>();
        enemy = GetComponent<Enemy>();
        enemyVisionScript = GetComponentInChildren<EnemyVisionScript>();
        inventarioEnemy = GetComponent<InventarioEnemy>();
        aiPath = GetComponent<AIPath>();

        inimigoEstados = InimigoEstados.Patrulhar;
        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;

        inventarioEnemy.Iniciar();

        municaoNoCarregadorMax = inventarioEnemy.ArmaSlot.GetStatus.MunicaoMaxCartucho;
        municaoNoCarregador = municaoNoCarregadorMax;
        tempoRecarregarArmaMax = inventarioEnemy.ArmaSlot.GetStatus.TempoParaRecarregar;

        vendoPlayerCircular = false;
        vendoPlayer = false;
        playerAreaAtaque = false;
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

        presenteNaListaDeDeteccao = false;

        posicaoTiroPlayer = Vector2.zero;
        posicaoUltimoLugarVisto = Vector2.zero;
        posicaoAtualPlayer = Vector2.zero;
        posicaoInicial = transform.position;

        ResetarContadores();

        iniciado = true;
    }

    // Update is called once per frame
    public void Main()
    {
        ReferenciaVariaveisExternas();

        StateMachine();
        funcoesBasicas();
        Acoes();

    }

    protected virtual void StateMachine()
    {
        #region MaquinaDeEstadosInimigoNoraml
        switch (estadoDeteccaoPlayer)
        {

            case EstadoDeteccaoPlayer.NaoToVendoPlayer://caso não saiba Onde O player esta
                if (presenteNaListaDeDeteccao)
                {
                    enemy.GeneralManager.EnemyManager.PerdiVisaoInimigo();
                    presenteNaListaDeDeteccao = false;
                    posicaoListaIndiceDeteccao = 0;
                }

                if (vendoPlayer)//Caso tenha visto o player
                {
                    enemyMovement.ZerarVelocidade();
                    controlodarEsqueciPlayer = false;
                    estadoDeteccaoPlayer = EstadoDeteccaoPlayer.DetectandoPlayer;
                }
                else
                {
                    bool ouvindoSom = somTiro || somPasso ? true : false;

                    if (viuPlayerAlgumaVez && !emLockDown && !vouApertarBotao)    //caso tenha visto o player alguma vez, nao esteja em lockDown e esteja mais perto do botao de lockdown do que o player           
                    {
                        vouApertarBotao = true;
                        inimigoEstados = InimigoEstados.IndoAtivarLockDown;
                    }

                    else if (ouvindoSom && !vouApertarBotao)   //caso tenha ouvido algo
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

                    else if (!ouvindoSom && fazerRotinaLockDown && !vouApertarBotao) // caso receba que é pra fazer a rotina de lockdown(varrer a fase)
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
                if (emLockDown && vendoPlayer) // vendo player em lockdown
                {
                    verifiqueiUltimaPosicaoJogador = false;
                    viuPlayerAlgumaVez = true;
                    estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
                    inimigoEstados = InimigoEstados.AndandoAtePlayer;
                }

                else if (vendoPlayer && !controlodarEsqueciPlayer) //se ver o player sem estar em lockdown
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
                else if (!vendoPlayer && !controlodarEsqueciPlayer) //se estiver no estado de detecao e nao ver o player e o contador for falso diminui na detecao
                {
                    if (ContadorInverso(ref tempoEntrarEmModoAlerta, tempoEntrarEmModoAlertaMax))
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
                if (!presenteNaListaDeDeteccao) //sistema para ultimo integrante ter que apertar o botao, sempre ´e a ultimo inimigo a ver o player quem vai ativar
                {
                    posicaoListaIndiceDeteccao = enemy.GeneralManager.EnemyManager.AdicionarAlguemVendoPlayer();
                    presenteNaListaDeDeteccao = true;
                }

                vendoPlayer = vendoPlayerCircular;
                if (!controlodarEsqueciPlayer)
                {
                    if (!vendoPlayer)
                    {
                        if (Contador(ref tempoEsquecerPlayer, tempoEsquecerPlayerMax)) //caso nao o veja chamar contador para perder o inimigo de vista
                        {
                            controlodarEsqueciPlayer = true;
                            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
                            Debug.Log("Perdi o player De vista, indo na sua ultima posicao");
                        }
                    }
                    else // vendo player aumentar a detecao e verificar se esta na zona de ataque
                    {
                        posicaoUltimoLugarVisto = posicaoAtualPlayer;
                        ContadorInverso(ref tempoEsquecerPlayer, tempoEsquecerPlayerMax);

                        if(inimigoEstados == InimigoEstados.FazerRotinaLockdow)
                        {
                            Debug.LogError("o inimigo ta pra fazer lockdown mesmo vendo o player");
                        }

                        // Debug.Log("to entrando aqui "+gameObject.name+"sas");
                        if (enemy.GeneralManager.EnemyManager.VerificarUltimoVerPlayer(posicaoListaIndiceDeteccao) && !emLockDown) //o ultimo inimigo a ver o player deve ativar o lockdown isso tem prioridade sobre atacar ou mover
                        {
                            inimigoEstados = InimigoEstados.IndoAtivarLockDown;
                        }

                        else if (playerAreaAtaque)
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
        #endregion        
    }

    protected virtual void AndarAtePlayer()
    {
        enemyMovement.Mover(posicaoAtualPlayer);
    }
    protected virtual void Patrulhar()
    {
        if(VerificarChegouAteAlvo(enemyMovement.PontosDeRota))
        {
            enemyMovement.GerarNovoPonto(false);
        }

    }
    protected virtual void  TomarDano()
    {
        if (FuncaoIrAteLugarVerificarArea(posicaoUltimoLugarVisto, ref tempoVerificandoTomeiTiro, tempoVerificandoTomeiTiroMax))
        {
            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
            tomeiDano = false;
        }
    }
    protected virtual void SomPassos()
    {
        if (FuncVerificarArea(ref tempoVerificandoSomPassos, tempoVerificandoSomPassosMax))
        {
            somPasso = false;
            //Debug.Log("ouvi alguns passos");
        }
        else
        {
            enemyMovement.ZerarVelocidade();
        }
    }
    protected virtual void SomTiro()
    {
        if (FuncaoIrAteLugarVerificarArea(posicaoTiroPlayer, ref tempoVerificandoSomTiro, tempoVerificandoSomTiroMax))
        {
            somTiro = false;
            //Debug.Log("pronto ja fui e olhei o som do tiro,");
        }
    }
    protected virtual void AndandoUltimaPosicaoPlayerConhecida()
    {
        if (FuncaoIrAteLugarVerificarArea(posicaoUltimoLugarVisto, ref tempoVerificandoUltimaPosicaoPlayer, tempoVerificandoUltimaPosicaoPlayerMax))
        {
            verifiqueiUltimaPosicaoJogador = true;
            //Debug.Log("pronto ja fui e olhei,");
        }
    }
    protected virtual void IndoAtivarLockdown()
    {
        if (VerificarChegouAteAlvo(generalManager.ObjectManager.ListaAlarmes[indiceDoBotaoMaisPerto].transform.position))
        {
            AtivarLockDown();
        }
    }
    protected virtual void FicarParado()
    {
        enemyMovement.ZerarVelocidade();
    }
    protected virtual void FazerRotinaLockdown()
    {
        if (RotinaLockdow())
        {
            fazerRotinaLockDown = false;
        }
    }
    void Acoes()
    {
        #region FazerAcoesMaquinaEstados
        switch (inimigoEstados)
        {
            case InimigoEstados.AndandoAtePlayer:
                AndarAtePlayer();
                break;

            case InimigoEstados.Patrulhar:
                Patrulhar();
                break;

            case InimigoEstados.AtacarPlayer:
                Atacar();
                break;
            case InimigoEstados.TomeiDano:
                TomarDano();
                break;

            case InimigoEstados.SomPassos:
                SomPassos();
                break;

            case InimigoEstados.SomTiro:
                SomTiro();
                break;

            case InimigoEstados.AndandoUltimaPosicaoPlayerConhecida:
                AndandoUltimaPosicaoPlayerConhecida();
                break;

            case InimigoEstados.IndoAtivarLockDown:
                IndoAtivarLockdown();
                break;

            case InimigoEstados.FicarParado:
                FicarParado();
                break;

            case InimigoEstados.FazerRotinaLockdow:
                FazerRotinaLockdown();
                break;
            default:
                break;
        }
        #endregion
    }
    void funcoesBasicas()
    {

        if (municaoNoCarregador <= 0)
        {
            if (Contador(ref tempoRecarregarArma, tempoRecarregarArmaMax))
            {
                municaoNoCarregador = municaoNoCarregadorMax;
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
    }
    
    bool RotinaLockdow()
    {
        //to fazendo a rotina do lockdow
        if(emLockDown)
        {
            if (VerificarChegouAteAlvo(enemyMovement.PontoDeProcura))
            {
                enemyMovement.GerarNovoPonto(true);
            }
            return false;
        }
        return true;
        /*if(VerificarChegouAteAlvo(posicaoUltimoLugarVisto))
        {
            return true;
        }
        return false;*/
        //fazer a rotina lockdown
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
    
    void AtivarLockDown()
    {
        vouApertarBotao = false;
        generalManager.ObjectManager.ListaAlarmes[indiceDoBotaoMaisPerto].AtivarLockDown();
        enemy.GeneralManager.LockDownManager.AtivarLockDown(posicaoUltimoLugarVisto);
        inimigoEstados = InimigoEstados.AndandoUltimaPosicaoPlayerConhecida;
    }



    #region IrAteLugarVerificarArea
    protected bool FuncaoIrAteLugarVerificarArea(Vector2 posicaoAlvo,ref float tempo, float tempoMax) // func que verifica se chegou ate o ponto e retorna se ja verificou a area
    {
        if (VerificarChegouAteAlvo(posicaoAlvo)) //olhar pros lados
        {
            return FuncVerificarArea(ref tempo, tempoMax);            
        }
        return false;
    }
    protected bool FuncVerificarArea(ref float tempo, float tempoMax) // func que retorna se terminou de verificar a area
    {
        if (tempo / tempoMax < 0.25)
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

        return Contador(ref tempo, tempoMax);
    }

    protected virtual bool VerificarChegouAteAlvo(Vector2 alvo)
    {

        if (Vector2.Distance(transform.position, alvo) > 0.5)
        {
            enemyMovement.Mover(alvo);
            return false;//se chegou retorna Verdadeiro
        }
        else
        {
            return true;
        }
    }
    #endregion

    protected bool Contador(ref float tempo, float tempoMax)
    {
        tempo += Time.deltaTime;
        if (tempo > tempoMax)
        {
            tempo = 0;
            return true;
        }
        return false;
    }
    protected bool ContadorInverso(ref float tempo, float tempoMax)
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
        List<LockDown> botoesLockDown = generalManager.ObjectManager.ListaAlarmes;

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

    public void VendoOutroInimigo(Enemy _enemy)
    {
        if (_enemy.GetIA_Enemy_Basico.inimigoEstados == InimigoEstados.AndandoAtePlayer || _enemy.GetIA_Enemy_Basico.inimigoEstados == InimigoEstados.AtacarPlayer)
        {
            //Debug.Log("recebi player");
            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
        }
        else if (!emLockDown)
        {
            //caso veja um morto
            if(_enemy.morto)
            {
                //ativar lockdown e fazer algo return true
            }        
        }

    }
    protected void ReferenciaVariaveisExternas()//variaves de controle que tem que serem pegas a todo frame
    {
        vendoPlayer = enemyVisionScript.GetVendoPlayer;
        vendoPlayerCircular = enemyVisionScript.GetVendoPlayerCircular;
        playerAreaAtaque = enemy.playerOnAttackRange;
        posicaoAtualPlayer = enemy.GetPlayer.transform.position;
        indiceDoBotaoMaisPerto = RetornarIndiceBotaoLockDownMaisPerto();
    }
    public virtual void ReceberLockDown(Vector2 _posicaoPlayer)
    {
        fazerRotinaLockDown = true;
        emLockDown = true;
        posicaoUltimoLugarVisto = _posicaoPlayer;
    }
    public virtual void DesativarLockDown()
    {
        emLockDown = false;
        fazerRotinaLockDown = false;
        viuPlayerAlgumaVez = false;
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
    public virtual void Respawn()
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

        transform.position = posicaoInicial;
        enemyMovement.Mover(transform.position);

        ResetarContadores();

        indiceDoBotaoMaisPerto = 0;

        presenteNaListaDeDeteccao = false;

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

