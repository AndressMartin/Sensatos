using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{

    //Componentes
    private Player player;
    private EnemyMovement enemyMovement;

    enum InimigoEstados { AndandoAtePlayer,Patrulhar,VerificandoArea,AtacarPlayer,SomPassos,SomTiro,AndandoUltimaPosicaoPlayerConhecida};
    InimigoEstados inimigoEstados;

    bool vendoPlayer;
    bool playerAreaAtaque;
    bool emLockDown;
    bool fazerRotinaLockDown;
    bool somTiro;
    bool somPasso;
    bool controlePlayerPos;

    Vector2 posicaoPassosPlayer;
    Vector2 posicaoTiroPlayer;
    Vector2 posicaoUltimoLugarVisto;
    Vector2 posicaoAtualPlayer;
    Vector2 posicaoUltimoPontoRota;

    float tempoVerificarAreaLockdown, tempoVerificarAreaLockdownMax;

    void Start()
    {
        player = FindObjectOfType<Player>();
        enemyMovement= GetComponent<EnemyMovement>();

        vendoPlayer = false;
        playerAreaAtaque = false;
        emLockDown = false;
        fazerRotinaLockDown = false;
        somPasso = false;
        somTiro = false;
        controlePlayerPos = true;
        emLockDown = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if(!vendoPlayer && !controlePlayerPos)
        {
            controlePlayerPos = Contador(0,2);
            posicaoUltimoLugarVisto = posicaoAtualPlayer;
        }
        

        if(playerAreaAtaque)
        {
            inimigoEstados = InimigoEstados.AtacarPlayer;
        }
        else if(vendoPlayer)
        {
            controlePlayerPos = false;
            inimigoEstados = InimigoEstados.AndandoAtePlayer;
        }
        else if(somTiro || somPasso)
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
        else if (fazerRotinaLockDown)
        {
            if ( /*Distancia sua ate o local que relataram o player > 0.5*/true)
            {
                inimigoEstados = InimigoEstados.AndandoUltimaPosicaoPlayerConhecida;
            }
            else //caso chegue na area em que o jogador estava
            {

                if (VerificandoArea(tempoVerificarAreaLockdown,tempoVerificarAreaLockdownMax))
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
                FuncVerificarArea(0, 9, InimigoEstados.Patrulhar);
                break;

            case InimigoEstados.SomTiro:
                FuncaoIrAteLugarVerificarArea(posicaoTiroPlayer, 0, 1, InimigoEstados.Patrulhar);
                break;

            case InimigoEstados.AndandoUltimaPosicaoPlayerConhecida:
                FuncaoIrAteLugarVerificarArea(posicaoUltimoLugarVisto, 0, 1, InimigoEstados.Patrulhar);
                break;

            default:
                break;
        }
    }
    void Patrulhar()
    {

    }

    void Atacar()
    {

    }
    void Mover(Vector2 _alvo)
    {
        enemyMovement.Movimentar(enemyMovement.calcMovimemto(_alvo));
    }
    public void AtivarLockDown(bool ativo)
    {
        ativo = true;
        emLockDown = ativo;
        fazerRotinaLockDown = ativo;
        //mudar Variaveis conforme LockDown
    }

    #region IrAteLugarVerificarArea
    void FuncaoIrAteLugarVerificarArea(Vector2 posicaoAlvo, float tempo, float tempoMax, InimigoEstados estadoQueVaiFicarNoFim)
    {
        if (VerificarAndarAteAlvo(posicaoAlvo))
        {
            FuncVerificarArea(tempo, tempoMax, estadoQueVaiFicarNoFim);
        }
    }
    void FuncVerificarArea(float tempo, float tempoMax, InimigoEstados estadoQueVaiFicarNoFim)
    {
        if (VerificandoArea(tempo, tempoMax))
        {
            inimigoEstados = estadoQueVaiFicarNoFim;
        }
    }
    bool VerificarAndarAteAlvo(Vector2 alvo)
    {
        if (Vector2.Distance(transform.position, alvo) < 0.5)
        {
            Mover(alvo);
            return true;
        }
        return false;
    }
    bool VerificandoArea(float tempo, float tempoMax)
    {
        inimigoEstados = InimigoEstados.VerificandoArea;
        return Contador(tempo, tempoMax);
    }

    #endregion
    bool Contador(float tempo, float tempoMax)
    {
        tempo += Time.deltaTime;
        if (tempo > tempoMax)
        {
            return true;
        }
        return false;
    }
    public void FazerRespawn()
    {

    }
}
