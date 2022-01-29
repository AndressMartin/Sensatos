using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Enemy : MonoBehaviour
{
    //Componentes
    private Enemy enemy;
    private EnemyMovement enemyMovement;
    private EnemyVisionScript enemyVisionScript;
    private ObjectManagerScript objectManagerScript;
    private Player player;

    //Variaveis Controle
    Vector2 playerSoundPosition;
    Vector2 posicaoPlayerAposPerderDeVista;

    bool vendoPlayer;
    bool vendoPlayerCircular;//vendo player com visao circular pensando em tirar

    bool viuPlayerAlgumaVez;
    bool ativeiLockDown;
    bool perderPlayerDeVista;
    bool hearPlayer;
    bool hearShoot;
    bool emLockDown;

    int indiceDoBotaoMaisPerto = 0;
    //Contadores
    private float countEntrarModoAtaque;
    private float countVerificarRegiao;
    private float countPosicaoPlayerPerderVista;
    private float contadorOlharEmDirecaoAoSom;

    [SerializeField] private float contadorpraPoderEntrarModoAtaquePlayerMax;
    [SerializeField] private float timerEmQueVaiVerificarRegiaoMax;
    [SerializeField] private float contadorPosicaoPlayerPerderDeVistaMax;
    [SerializeField] private float contadorOlharEmDirecaoAoSomMax;


    //Enum
    public enum Estado { Rotina, Alerta, Combate, Lockdown };
    public Estado estado;

    public enum Stances { Idle, Patrolling, Wait, CorrerAteUltimaPosicaoPlayer, VarrerFase };
    public Stances stance;
    public enum FazerMovimentoAlerta
    {
        NA, AndandoAte_UltimaPosicaoPlayer, ChechandoUltimaPosicaoPlayer, VoltandoA_RotinaPadrao,
        OuviuTiro, AndandoAte_UltimaPosicaoSomPlayer, OuviuPassos
    }
    public FazerMovimentoAlerta fazerMovimentoAlerta = FazerMovimentoAlerta.NA;
    public enum ModoPatrulha { destraido, atento };
    public ModoPatrulha modoPatrulha;


    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyVisionScript = GetComponentInChildren<EnemyVisionScript>();
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();
        player = FindObjectOfType<Player>();

        emLockDown = false;
        perderPlayerDeVista=false;
        viuPlayerAlgumaVez = false;
        ativeiLockDown = false;
    }
   
    public void Main()
    {
        VariaveisAtualizamTodoFrame();

        if (vendoPlayer)//caso esteja vendo o player
        {
            switch (estado)
            {
                case Estado.Lockdown:
                    seguirAtacarPlayer();
                    break;
                case Estado.Combate:
                    seguirAtacarPlayer();
                    break;
                case Estado.Alerta:
                    ContadorEntrarEmModoAtaque();//inicia o contador pra ir entrar em estado combate 
                    break;
                case Estado.Rotina:
                    estado = Estado.Alerta;
                    break;
            }
        }


        else //caso não veja o player
        {
            indiceDoBotaoMaisPerto=RetornarIndiceBotaoLockDownMaisPerto();
            ContadorSeguirPLayer();

            if (viuPlayerAlgumaVez && !ativeiLockDown &&
                Vector2.Distance(transform.position,objectManagerScript.listaAlarmes[indiceDoBotaoMaisPerto].transform.position) < Vector2.Distance(transform.position,player.transform.position))
            {
                
                AtivarOLockDown(indiceDoBotaoMaisPerto);
                
            }
            else
            {
                switch (estado)
                {
                    case Estado.Combate:
                        estado = Estado.Alerta;
                        break;

                    case Estado.Lockdown:
                        switch (stance)
                        {
                            case Stances.CorrerAteUltimaPosicaoPlayer:
                                MovimentarUltimaPosicaoPLayerLockDown(posicaoPlayerAposPerderDeVista);
                                break;
                            case Stances.VarrerFase:
                                enemyMovement.VarrerFase();
                                break;

                            default:
                                Debug.Log("ERROR");
                                break;
                        }
                        break;

                    case Estado.Alerta://ver player
                        switch (fazerMovimentoAlerta)
                        {
                            case FazerMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer:
                                MovimentarUltimaPosicaoPLayer(posicaoPlayerAposPerderDeVista);
                                break;
                            case FazerMovimentoAlerta.ChechandoUltimaPosicaoPlayer:
                                VerificarRegiao();
                                break;
                            case FazerMovimentoAlerta.VoltandoA_RotinaPadrao:
                                MovimentarVoltarRotinaPadrao(enemyMovement.GetUltimaPosicaoOrigem);
                                break;
                            case FazerMovimentoAlerta.OuviuTiro:
                                OuviuTiro();
                                break;
                            case FazerMovimentoAlerta.AndandoAte_UltimaPosicaoSomPlayer:
                                MovimentarAteOSom();
                                break;
                            case FazerMovimentoAlerta.OuviuPassos:
                                if (emLockDown)
                                    OlharDirecaoSom();
                                else
                                    OuviuTiro();
                                break;

                            case FazerMovimentoAlerta.NA:
                                estado = Estado.Rotina;
                                break;
                        }
                        break;

                    case Estado.Rotina:
                        if (hearPlayer || hearShoot)
                        {
                            OuvindoInimigo();
                        }
                        else
                        {
                            switch (stance)
                            {
                                case Stances.Patrolling:
                                    enemyMovement.Patrulhar();
                                    break;
                            }
                        }
                        break;
                }
            }
        }
    }
    private int RetornarIndiceBotaoLockDownMaisPerto()
    {
        List<LockDown> botoesLockDown = objectManagerScript.listaAlarmes;

        float menorDistancia = 100;
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
    void AtivarOLockDown(int indice)
    {
        if (Vector2.Distance(transform.position, objectManagerScript.listaAlarmes[indice].transform.position) > 0.5f)
        {

            SeMovimentarAteAlvo(objectManagerScript.listaAlarmes[indice].transform.position);
        }
        else
        {
            ativeiLockDown = true;            //ou alguma boleano GetLockDownAtivo
            objectManagerScript.listaAlarmes[indice].AtivarLockDown();
            FindObjectOfType<LockDownManager>().VerPlayer(posicaoPlayerAposPerderDeVista);

            enemyMovement.ZerarVelocidade();
        }
        
    }

    void SeMovimentarAteAlvo(Vector2 _alvo)
    {
        enemyMovement.Movimentar(enemyMovement.calcMovimemto(_alvo));
    }
    private void seguirAtacarPlayer()
    {
        if (!enemy.playerOnAttackRange)
        {
            SeMovimentarAteAlvo(enemy.GetPlayer.transform.position);
        }
        else
        {
            Atacar();
        }

    }
    private void MovimentarUltimaPosicaoPLayerLockDown(Vector2 _posicao)
    {
        if (Vector2.Distance(transform.position, _posicao) > 0.5f)
        {

            SeMovimentarAteAlvo((_posicao));
        }
        else
        {
            stance = Stances.VarrerFase;
        }
    }
    private void MovimentarUltimaPosicaoPLayer(Vector2 _posicao)
    {
        if (Vector2.Distance(transform.position, _posicao) > 0.5f)
        {

            SeMovimentarAteAlvo((_posicao));
        }
        else
        {
            fazerMovimentoAlerta = FazerMovimentoAlerta.ChechandoUltimaPosicaoPlayer;
        }
    }
   
    private void MovimentarVoltarRotinaPadrao(Vector2 _posicao)
    {
        if (Vector2.Distance(transform.position, _posicao) > 0.5f)
        {

            SeMovimentarAteAlvo((_posicao));
        }
        else
        {
            fazerMovimentoAlerta = FazerMovimentoAlerta.NA;
            estado = Estado.Rotina;
            Debug.Log("aqui");
            stance = Stances.Patrolling;
        }
    }
    void MovimentarAteOSom()
    {
        if (Vector2.Distance(transform.position, playerSoundPosition) > 0.5f)
        {
            SeMovimentarAteAlvo(enemyMovement.calcMovimemto(playerSoundPosition));
        }
        else
        {
            fazerMovimentoAlerta = FazerMovimentoAlerta.ChechandoUltimaPosicaoPlayer;
        }
    }
    void OuviuTiro()
    {
        if (Vector2.Distance(playerSoundPosition, transform.position) >= 0.1)
        {
            SeMovimentarAteAlvo(playerSoundPosition);
        }
        else
        {
            fazerMovimentoAlerta = FazerMovimentoAlerta.AndandoAte_UltimaPosicaoSomPlayer;
            hearShoot = false;
            hearPlayer = false;
        }

    }

    void ContadorEntrarEmModoAtaque()
    {
        countEntrarModoAtaque += Time.deltaTime;
        if(countEntrarModoAtaque >= contadorpraPoderEntrarModoAtaquePlayerMax)
        {
            countEntrarModoAtaque = 0;
            estado = Estado.Combate;
            viuPlayerAlgumaVez = true;//caso tenha sido detectado pelo player
            perderPlayerDeVista = true;
        }
    }
    void OuvindoInimigo()
    {
        if (hearShoot)
        {
            estado = Estado.Alerta;
            fazerMovimentoAlerta = FazerMovimentoAlerta.OuviuTiro;
        }
        else if (hearPlayer)
        {
            Debug.Log("sendoChamado");
            estado = Estado.Alerta;
            fazerMovimentoAlerta = FazerMovimentoAlerta.OuviuPassos;
        }

    }

    private void VerificarRegiao()
    {
        enemyMovement.ZerarVelocidade();

        countVerificarRegiao += Time.deltaTime;
        if (countVerificarRegiao > timerEmQueVaiVerificarRegiaoMax)
        {
            fazerMovimentoAlerta = FazerMovimentoAlerta.VoltandoA_RotinaPadrao;
            countVerificarRegiao = 0;
        }

    }
    void OlharDirecaoSom()
    {
        enemyMovement.ZerarVelocidade();

        float difX = transform.position.x - playerSoundPosition.x;
        float difY = transform.position.y - playerSoundPosition.y;


        float dif = Vector2.Distance(transform.position, playerSoundPosition);

        if (difX > 0)
            enemy.ChangeDirection(EntityModel.Direcao.Esquerda);
        else
            enemy.ChangeDirection(EntityModel.Direcao.Direita);
        contadorOlharDirecao();
    }
    
    public void LockDownAtivo(Vector2 posicaoPlayer)
    {
        emLockDown = true;
        modoPatrulha = ModoPatrulha.atento;
        estado = Estado.Lockdown;
        stance = Stances.CorrerAteUltimaPosicaoPlayer;

        enemyVisionScript.EntrarModoPatrulha();

        posicaoPlayerAposPerderDeVista = posicaoPlayer;
        
    }
    public void EscutarSom(Player player, bool somTiro)
    {
        playerSoundPosition = player.transform.position;
        if (somTiro == true)
        {
            hearShoot = true;
        }
        else
        {
            hearPlayer = true;
        }
    }
    public void Respawn()
    {
        emLockDown = false;
        vendoPlayer = false;
        viuPlayerAlgumaVez = false;
        ativeiLockDown = false;
        perderPlayerDeVista = false;
        hearPlayer = false;
        hearShoot = false;
        playerSoundPosition = Vector2.zero;
        posicaoPlayerAposPerderDeVista = Vector2.zero;

        modoPatrulha = ModoPatrulha.destraido;
        estado = Estado.Rotina;
        stance = Stances.Patrolling;
        fazerMovimentoAlerta = FazerMovimentoAlerta.NA;
        
    }
    void VariaveisAtualizamTodoFrame()
    {
        vendoPlayer = enemyVisionScript.GetVendoPlayer;
        vendoPlayerCircular = enemyVisionScript.GetVendoPlayerCircular;
    }
    void Atacar()
    {
        enemyMovement.ZerarVelocidade();
        enemy.Atirar();
    }
    void contadorOlharDirecao()
    {
        contadorOlharEmDirecaoAoSom += Time.deltaTime;
        if (contadorOlharEmDirecaoAoSom >= contadorOlharEmDirecaoAoSomMax)
        {
            contadorOlharEmDirecaoAoSom = 0;
            fazerMovimentoAlerta = FazerMovimentoAlerta.VoltandoA_RotinaPadrao;
        }
    }
    void ContadorSeguirPLayer()
    {
        if (perderPlayerDeVista)
        {
            countPosicaoPlayerPerderVista += Time.deltaTime;
            if (countPosicaoPlayerPerderVista >= contadorPosicaoPlayerPerderDeVistaMax)
            {
                countPosicaoPlayerPerderVista = 0;
                perderPlayerDeVista = false;
            }
            else
            {
                posicaoPlayerAposPerderDeVista = new Vector2(enemy.GetPlayer.transform.position.x, enemy.GetPlayer.transform.position.y);
                fazerMovimentoAlerta = FazerMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
            }
        }
    }
}
