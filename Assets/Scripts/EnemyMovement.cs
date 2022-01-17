using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Componentes
    private GameObject gameObjectPlayerReservaAlt;
    private GameObject playerGameObject;
    private LockDown lockDown;
    private Enemy enemy;
    private EnemyVisionScript enemyVision;
    private PathFinding pathFinding;
    public Rigidbody2D rb;
    private ModoLockDown modoLockDown;
    private DetecSystem detecSystem;

    //Variaveis
    public int velocity;
    public List<Transform> moveSpots = new List<Transform>();

    float difPlayer;
    float difLockDownButton;

    private float velX;
    private float velY;

    [SerializeField] private Vector3 lastPlayerPosition;

    private Vector2 vetorKnockBack;

    //Variaveis de controle
    [SerializeField] private int contQuantosTirosParaTomarKnockBack;

    [SerializeField] private bool vendoPlayer;
    [SerializeField] private bool hearPlayer;
    [SerializeField] private bool hearShoot;

    public bool playerOnAttackRange;
    private Vector3 ultimaposicaoOrigem;
    private Vector3 playerSoundPosition;
    private int randomSpot;
    private int lastMoveSpot;

    bool firstTimeOnLoop = true;
    bool noContadorAlert;
    bool enemyPlayerReserva = false;

    private float timeKnockBack;
    private float timeKnockBackMax;


    //Contadores
    private float timerSeguirPlayerAposPerderEleDeVista = 0.0F;
    private float timerPraEntarEmModoCombate = 0.0F;
    private float timerEmQueVaiVerificarRegiao = 0.0F;
    private float timerPraPresetarModoAlerta = 0.0F;

    private float timerKnockback = 0.0F;
    private float timerResetKnockBack = 0.0F;
    private int knockBackCont = 0;

    //Tempos
    [SerializeField] private float timerSeguirPlayerAposPerderEleDeVistaMax;
    [SerializeField] private float timerPraEntarEmModoCombateMax;
    [SerializeField] private float timerEmQueVaiVerificarRegiaoMax;
    [SerializeField] private float timerResetarModoEmAlertaMax;
    [SerializeField] private float timerKnockbackMax;
    [SerializeField] private float timerResetKnockBackMax;

    //Enuns
    public enum Estado { Rotina, Alerta, Combate, Lockdown };
    public Estado estado;
    public enum Stances { Idle, Patrolling, Wait, CorrerAteUltimaPosicaoPlayer, VarrerFase };
    public Stances stance = Stances.Idle;
    public enum FazerMovimentoAlerta
{
        NA, AndandoAte_UltimaPosicaoPlayer, ChechandoUltimaPosicaoPlayer, VoltandoA_RotinaPadrao,
        OuviuTiro, AndandoAte_UltimaPosicaoSomPlayer, OuviuPassos
    }
    public FazerMovimentoAlerta fazerMovimentoAlerta = FazerMovimentoAlerta.NA;
    public enum ModoPatrulha { destraido, atento };
    public ModoPatrulha modoPatrulha;

    void getComponent()
    {
        detecSystem = GetComponentInChildren<DetecSystem>();
        modoLockDown = FindObjectOfType<ModoLockDown>();
        pathFinding = GetComponent<PathFinding>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        lockDown = FindObjectOfType<LockDown>();
    }

    public void ResetarVariaveisDeControle()
    {
        vendoPlayer = false;
        hearPlayer = false;
        hearShoot = false;
        contQuantosTirosParaTomarKnockBack = 10;
        playerOnAttackRange = false;
        ultimaposicaoOrigem = Vector3.zero;
        playerSoundPosition = Vector3.zero;
        randomSpot = 0;
        lastMoveSpot = 0;
        firstTimeOnLoop = true;
        noContadorAlert = false;
        enemyPlayerReserva = false;
        timerSeguirPlayerAposPerderEleDeVista = 0.0F;
        timerPraEntarEmModoCombate = 0.0F;
        timerEmQueVaiVerificarRegiao = 0.0F;
        timerPraPresetarModoAlerta = 0.0F;
        timerKnockback = 0.0F;
        timerResetKnockBack = 0.0F;
        knockBackCont = 0;
        velocity = 2;
    }

    private void Start()
    {
        //Componentes
        getComponent();
        modoLockDown.AddtoLista(this, timerPraEntarEmModoCombateMax, timerResetarModoEmAlertaMax);

        ultimaposicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);        
        stance = Stances.Patrolling;
        randomSpot = 0;

        //Variaveis de controle
        timeKnockBack = 0;
        timeKnockBackMax = 0.5f;
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

    void counterAlert()
    {
        rb.velocity = Vector2.zero;
        if (estado == Estado.Alerta)
        {
            noContadorAlert = true;
            timerPraEntarEmModoCombate += Time.deltaTime;
            if (timerPraEntarEmModoCombate > timerPraEntarEmModoCombateMax)
            {
                estado = Estado.Combate;
                noContadorAlert = false;
            }
        }
    }

    void CounterAlertTimer()//contador pra perder a barra de alerta
    {
        if (noContadorAlert && !vendoPlayer)
        {
            timerPraPresetarModoAlerta += Time.deltaTime;
            if (timerPraPresetarModoAlerta > timerResetarModoEmAlertaMax)
            {
                timerPraEntarEmModoCombate = 0;
                timerPraPresetarModoAlerta = 0;
                noContadorAlert = false;
            }
        }
        
    }

    void EntrarModoPatrulha()
    {
        velocity += 2;
        enemyVision.EntrarModoPatrulha();
        modoPatrulha = ModoPatrulha.atento;

    }
    public void seguirAtacarPlayer()
    {
        if (gameObjectPlayerReservaAlt == null)
            gameObjectPlayerReservaAlt = playerGameObject;

        /*if(timerPraEntarEmModoCombate != timerPraEntarEmModoCombateMax)//ficar contando que a detecção é instantanea
        timerPraEntarEmModoCombate = timerPraEntarEmModoCombateMax;*/

        if (hearPlayer || hearPlayer)
        {
            hearPlayer = false;
            hearShoot = false;  
        }

        fazerMovimentoAlerta = FazerMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
        lastPlayerPosition = gameObjectPlayerReservaAlt.transform.position;
        difPlayer = Vector2.Distance(playerGameObject.transform.position, transform.position);


        if (enemy.GetEstado == Enemy.Estado.Normal) 
        { 
            if (firstTimeOnLoop)
            {
                firstTimeOnLoop = false;
                EntrarModoPatrulha();
            }
            else
                ultimaposicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);


            if (!lockDown.lockDownAtivo) //caso não esteja em lockdawn
            {
                if (difPlayer < difLockDownButton)//caso o player estja mais perto que o alarme vai pra cima do player
                {
                    MoveEnemyToPlayer();
                }
                else if (difPlayer > difLockDownButton)//caso alarme estje mais perto ativa o alarme
                {
                    Debug.Log("2");

                    if (difLockDownButton > 0.05)
                    {
                        if (Vector2.Distance(transform.position, lockDown.transform.position) > 0.2f)
                        {
                            MoveGeneric(lockDown.transform.position);
                        }
                    }
                    else
                    {
                        lockDown.lockDownAtivo = true;  //ativa o lockdown
                        lockDown.AtivarLockDown(playerGameObject.transform.position);

                    }
                }
            }
            else // vai pra cima do player
            {
                Debug.Log("3");
                MoveEnemyToPlayer();
            }
            if (playerGameObject != null) {
                if (Vector2.Distance(transform.position, playerGameObject.transform.position) < 2.0)
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }
    void MoveEnemyToPlayer()
    {
        if (playerOnAttackRange)//caso esteja dentro do range de ataque 
        {           
            enemy.Atirar();//ataque
        }
        else if (enemyVision.vendoPlayer)
        {
            if (Vector2.Distance(transform.position, playerGameObject.transform.position) > 1.0f)//Esse num tem que ser o valor do raio+-
            {
                Debug.Log("To entrando aqui");
                MoveGeneric(playerGameObject.transform.position);//mover     
            }
        }
    }
    public void Main()
    {
        detecSystem.Main();
        CounterAlertTimer();
        vendoPlayer = enemyVision.vendoPlayer;
        difLockDownButton = Vector2.Distance(lockDown.transform.position, transform.position);

        if (vendoPlayer)//caso esteja vendo o player
        {
            switch (estado)
            {
                case Estado.Combate:
                    seguirAtacarPlayer();
                    break;
                case Estado.Alerta:
                    counterAlert();//inicia o contador pra ir entrar em estado combate 
                    break;
                case Estado.Rotina:
                    estado = Estado.Alerta;
                    break;
                case Estado.Lockdown:
                     seguirAtacarPlayer();
                    break;

            }

        }
        else //caso não veja o player
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
                            MoveLockDown(lastPlayerPosition);
                            break;
                        case Stances.VarrerFase:
                            VarrerFase();
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
                            MovimentarUltimaPosicaoPlayer(lastPlayerPosition);
                            break;
                        case FazerMovimentoAlerta.ChechandoUltimaPosicaoPlayer:
                            VerificarRegiao();
                            break;
                        case FazerMovimentoAlerta.VoltandoA_RotinaPadrao:
                            MovimentarVoltarRotinaPadrao(ultimaposicaoOrigem);
                            break;
                        case FazerMovimentoAlerta.OuviuTiro:
                            OuviuTiro();
                            break;
                        case FazerMovimentoAlerta.AndandoAte_UltimaPosicaoSomPlayer:
                            MovimentarAteOSom();
                            break;
                        case FazerMovimentoAlerta.OuviuPassos:
                            OlharDirecaoSom();
                            break;

                        case FazerMovimentoAlerta.NA:
                            estado = Estado.Rotina;
                            break;
                    }
                    break;


                case Estado.Rotina: //fazendo rotina
                    if (hearPlayer || hearShoot)//hearPlayer
                    {
                        OuvindoInimigo();
                    }
                    else
                    {
                        switch (stance)
                        {
                            case Stances.Patrolling://patrulhando
                                Patrulhar();
                                break;
                        }
                    }
                    break;
                
            }
        }

        if (enemy.GetEstado == Enemy.Estado.TomandoDano)
        {
            KnockBackContador();
        }
    }
    public void LockDownAtivo(Vector2 posicaoPlayer)
    {
        if (!vendoPlayer)
        {
            estado = Estado.Lockdown;
            stance = Stances.CorrerAteUltimaPosicaoPlayer;
            lastPlayerPosition = posicaoPlayer;

        }
    }
    Vector2 InimigoSeguindoAposPerderVisaoDoPlayerContador(Vector2 vector2)
    {     
        if (gameObjectPlayerReservaAlt != null)
        {
            timerSeguirPlayerAposPerderEleDeVista += Time.deltaTime;
            if (timerSeguirPlayerAposPerderEleDeVista > timerSeguirPlayerAposPerderEleDeVistaMax)//pega a ultima posicao do player conhecida e passa a pra variavel, zera as outras coisas
            {
                timerSeguirPlayerAposPerderEleDeVista = 0;//depois daquele tempo em que segue o jogador voltar pro alerta
                gameObjectPlayerReservaAlt = null;
                enemyPlayerReserva = true;
            }
            else        //enquanto o contador não chegar no numero fique atualizando a posicao do player e retornando pra onde o inimigo deve andar
            {
                Debug.Log("SendoAtualizado");
                lastPlayerPosition = gameObjectPlayerReservaAlt.transform.position;
                return lastPlayerPosition;
            }
            return vector2;
        }
        return vector2;
        
    }
    private void VerificarRegiao()
    {
        rb.velocity = Vector2.zero;
        timerEmQueVaiVerificarRegiao += Time.deltaTime;
        if (timerEmQueVaiVerificarRegiao > timerEmQueVaiVerificarRegiaoMax)
        {
            fazerMovimentoAlerta = FazerMovimentoAlerta.VoltandoA_RotinaPadrao;
            timerEmQueVaiVerificarRegiao = 0;
        }

    }

    private void OuviuTiro()
    {
        //Debug.Log("To no tiro");
        if (Vector2.Distance(playerSoundPosition, transform.position) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
        {
            MoveGeneric(playerSoundPosition);
            //Debug.Log("To andando ate tiro");
        }
        else
        {
            fazerMovimentoAlerta = FazerMovimentoAlerta.AndandoAte_UltimaPosicaoSomPlayer;
            hearShoot = false;
            hearPlayer = hearShoot;
            //Debug.Log("é ture"+ fazerMovimentoAlerta);
        }
        
    }
    private void OuvindoInimigo()
    {
        
        //Debug.Log("to ouvindo algo Shoot: "+hearShoot+" Player: "+hearPlayer);
        if (hearShoot)
        {
            estado = Estado.Alerta;
            fazerMovimentoAlerta = FazerMovimentoAlerta.OuviuTiro;
        }
        else if(hearPlayer)
        {
            estado = Estado.Alerta;
            fazerMovimentoAlerta = FazerMovimentoAlerta.OuviuPassos;
        }
        //else
        //fazesMovimentoAlerta = FazesMovimentoAlerta.NA;

        /*if (Vector2.Distance(playerSoundPosition, transform.position) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
        {
            MoveGeneric(playerSoundPosition);
        }*/
        
    }
    private void Movimentar(Vector2 posicao)
    {
        rb.velocity = posicao;//andando ate a o posica passada
        CollisionDirection();
    }

    private void MoveGeneric(Vector2 _posicao)
    {
        rb.velocity = Vector2.zero;
        Movimentar(calcMovimemto(_posicao));
        
    }
    private void MoveLockDown(Vector2 _posicao)
    {
        if (Vector2.Distance(_posicao,transform.position) >= 4)
        {
            MoveGeneric(_posicao);
            Debug.Log("estou em lockDawn");
        }
        else
        {
            stance = Stances.VarrerFase;
            //stance = Stances.patrolling;
        }
    }
    private void VarrerFase()
    {
        fazerMovimentoAlerta = FazerMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
        estado = Estado.Alerta;

        //coisas
        //fim da funcao


    }
    private void MovimentarUltimaPosicaoPlayer(Vector2 _posicao)
    {
        
        if(!enemyPlayerReserva)//so atualiza a posicao do jogador se o contador não deu o tempo
            _posicao = InimigoSeguindoAposPerderVisaoDoPlayerContador(_posicao);

        if (Vector2.Distance(transform.position, _posicao) > 0.5f)
        {
            Movimentar(calcMovimemto(lastPlayerPosition));
        }
        else
        {
            fazerMovimentoAlerta = FazerMovimentoAlerta.ChechandoUltimaPosicaoPlayer;
        }
        
    }
    void OlharDirecaoSom()
    {
        float difX = transform.position.x - playerSoundPosition.x;
        float difY = transform.position.y - playerSoundPosition.y;


        float dif= Vector2.Distance(transform.position, playerSoundPosition);

        if (difX > 0)
            enemy.ChangeDirection(EntityModel.Direcao.Esquerda);
        else
            enemy.ChangeDirection(EntityModel.Direcao.Direita);
        contadorOlharDirecao();
    }
    void contadorOlharDirecao()
    {

    }

    void MovimentarAteOSom()
    {
        if (Vector2.Distance(transform.position, playerSoundPosition) > 0.5f)
        {
            Debug.Log("to indo pra ultima posicao do player");
            Movimentar(calcMovimemto(playerSoundPosition));
        }
        else
        {
            Debug.Log("chegued no last player");
            fazerMovimentoAlerta = FazerMovimentoAlerta.ChechandoUltimaPosicaoPlayer;
        }
    }
    private void MovimentarVoltarRotinaPadrao(Vector2 _posicao)
    {
        if (Vector2.Distance(transform.position, _posicao) > 0.5f)
        {
            Debug.Log("to indo pra origem");
            Movimentar(calcMovimemto(_posicao));
        }
        else
        {
            Debug.Log("chegued na origem");
            fazerMovimentoAlerta = FazerMovimentoAlerta.NA;
            estado = Estado.Rotina;
            stance = Stances.Patrolling;
        }
    }
 
    private void Patrulhar()
    {
        Vector2 directionTemp = moveSpots[randomSpot].position - transform.position;
        Vector2 direction = directionTemp.normalized * velocity;
        Movimentar(direction);

        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            //gera um novo lugar de waypoint
            if (randomSpot >= moveSpots.Count - 1)
                randomSpot = 0;
            else
                randomSpot++;

            if (randomSpot != lastMoveSpot)
            {
                lastMoveSpot = randomSpot;
                ultimaposicaoOrigem = moveSpots[lastMoveSpot].position;
            }
            else
            {
                while (randomSpot == lastMoveSpot)
                {
                    randomSpot = Random.Range(0, moveSpots.Count);
                }
            }
        }
        else
        {
            stance = Stances.Patrolling;
        }
    }
    private void CollisionDirection()
    {
        velX = rb.velocity.x;
        velY = rb.velocity.y;


        if (Mathf.Abs(velY) >= Mathf.Abs(velX))
        {
            if (velY < 0)
            {
                enemy.ChangeDirection(Enemy.Direcao.Baixo);
            }
            else if (velY > 0)
            {
                enemy.ChangeDirection(Enemy.Direcao.Cima);
            }
        }

        else if (velX > 0)
        {
            enemy.ChangeDirection(Enemy.Direcao.Direita);
        }
        else if (velX < 0)
        {
            enemy.ChangeDirection(Enemy.Direcao.Esquerda);
        }
    }
    public void KnockBack(float _knockBack, Vector2 _direcaoKnockBack)
    {
        vetorKnockBack = _direcaoKnockBack * _knockBack;
        timeKnockBack = 0;
    }

    void KnockBackContador()
    {
        timeKnockBack += Time.deltaTime;
        if (timeKnockBack > timeKnockBackMax)
        {
            timeKnockBack = 0;
            vetorKnockBack = Vector2.zero;
            enemy.FinalizarAnimacao();
        }
    }

    public void EnemyVissionReference(EnemyVisionScript _enemyVision)
    {
        enemyVision = _enemyVision;
    }

    Vector2 calcMovimemto(Vector2 __posicao)
    {
        Vector2 PlayerPosition = transform.position;
        Vector2 directionTemp = __posicao - PlayerPosition;
        Vector2 direction = directionTemp.normalized * velocity;

        return direction;
    }

    public void SawEnemy(GameObject _whoEnemySaw)
    {
        if (_whoEnemySaw == null)
        {
            timerEmQueVaiVerificarRegiao = 0;
            enemyPlayerReserva = false;
            timerSeguirPlayerAposPerderEleDeVista = 0;
            playerGameObject = null;
        }
        else
        {
            playerGameObject = _whoEnemySaw;
            timerEmQueVaiVerificarRegiao = 0;
            timerSeguirPlayerAposPerderEleDeVista = 0;
        }

    }

    public void ZerarVelocidade()
    {
        rb.velocity = new Vector2(0, 0);
    }
    public float RetornarTimerDeAlerta()
    {
        return timerPraEntarEmModoCombate;
    }
    public bool RetornarVendoPlayer()
    {
        return vendoPlayer;
    }
    public float RetornarTimeResetandoAlerta()
    {
        return timerPraPresetarModoAlerta;
    }
    public bool RetornarMorto()
    {
        return gameObject.GetComponent<Enemy>().morto;

    }


}
/*Movimentacao old
 * 
            if (!lastPlayerPositionChecked)//se o inimigo ja checou a ulitma posicao conehceida
            {
                if (gameObjectPlayerReserva != null)
                {
                    estado = Estado.alerta;
                    hearEnemy = false;
                    lastPlayerPosition = gameObjectPlayerReserva.transform.position;
                    timePlayerResrva += Time.deltaTime;
                    if (timePlayerResrva > timePlayerResrvaMax)//pega a ultima posicao do player conhecida e passa a pra variavel, zera as outras coisas
                    {
                        timePlayerResrvaMax = 0.0F;
                        timePlayerResrva = 0;
                        gameObjectPlayerReserva = null;//depois daquele tempo em que segue o jogador voltar pro alerta
                    }
                }

                if (Vector2.Distance(lastPlayerPosition,transform.position) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
                {

                    Vector2 direction = lastPlayerPosition - transform.position;
                    direction.Normalize();
                    MOVE(direction);

                    //Debug.Log("indo até a ultima posicao do  player");
                }
                else
                {                                         //inimigo começa a contar como se estivesse procurando o player por um tempo na região                  
                    //Debug.Log("contador player");

                    time += Time.deltaTime;
                    if (time > timeMax)
                    {
                        lastPlayerPositionChecked = true;
                        timeMax = 0.0F;
                        time = 0;
                    }
                }
            }
            else // caso o inimigo ja tenha checado a ultima posicao do player ele volta para sua origem, ou rotina;
            {
                timeAlert = 0;
                if (ultimaposicaoOrigem != null)
                {
                    initialPosition = ultimaposicaoOrigem;
                }
                if (Mathf.Abs(initialPosition.x - transform.position.x) <= 0.1 && Mathf.Abs(initialPosition.y - transform.position.y) <= 0.1) // voltar para origem
                {
                    estado = Estado.rotina;
                    hearShoot=false;
                    //Debug.Log("Cheguei a minha origme");
                }
                else
                {
                    Vector2 direction = initialPosition - transform.position;
                    direction.Normalize();
                    MOVE(direction);

                    //MoveTransform(direction);
                    //Debug.Log("voltando origemr");
                }

                //Debug.Log("Não vejo o player, voltando a origem");
            }
        }
        if(viuPlayerUmaVez && !vendoPlayer  && !lockDawn.ativo)
        {
            if (difLockDawnButton > 0.05)
            {
                Vector3 direction = lockDawn.transform.position - transform.position;
                direction.Normalize();
                MOVE(direction);
            }
            else
            {
                lockDawn.ativo = true;
            }
        }
        else if (hearEnemy && estado != Estado.combate)//caso ouça o inimigo
        {
            if (hearShoot)//caso tenha tiro
            {
                estado = Estado.alerta;
                if (Vector2.Distance(enemySoundPosition,transform.position) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
                {
                    Vector2 direction = enemySoundPosition - transform.position;
                    direction.Normalize();
                    MOVE(direction);
                }
            }
            else//caso seja por passos
            {
                if (estado == Estado.alerta)
                {
                    if (Vector2.Distance(enemySoundPosition, transform.position) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
                    {
                        Vector2 direction = enemySoundPosition - transform.position;
                        direction.Normalize();
                        MOVE(direction);
                    }
                    else
                    {
                        hearEnemy = false;
                    }
                }
                else
                {
                    estado = Estado.alerta;
                }
            }
        }
 */



