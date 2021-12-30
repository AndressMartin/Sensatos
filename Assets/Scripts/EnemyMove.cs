using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //Componentes
    private EnemyVision enemyVision;
    private PathFinding pathFinding;

    public int velocity;
    public Vector3 initialPosition;
    [SerializeField] private GameObject playerGameObject;
    public Rigidbody2D rb;
    private Enemy enemy;

    public bool playerOnAttackRange;
    [SerializeField] private bool lastPlayerPositionChecked;
    [SerializeField] private Vector3 lastPlayerPosition;

    private float time = 0.0F;
    
    private float timePlayerReserva = 0.0F;


    [SerializeField] private float timePlayerReservaMax;
    [SerializeField] private float timeMaxAlert;
    [SerializeField] private float timeMax;
    [SerializeField] private float timeMaxAlertOriginal;

    public float velX;
    public float velY;

    bool firstTimeOnLoop = true;

    public float knockBackHorizontal, knockBackVertical;
    public bool Knock;
    [SerializeField] private float timeKnock, timeMaxKnock;
    private float timeMaxOriginalKnock;

    [SerializeField] private float timeKnockCont, timeMaxKnockCont;
    private float timeMaxOriginalKnockCont;
    bool knockBacking;
    int knockBackCont = 0;
    [SerializeField] private int contQuantosTirosParaTomarKnockBack;

    private Vector3 playerSoundPosition;
    [SerializeField] public bool hearPlayer;

    private enum state { FollowPlayer, AttackingPlayer, SearchingPlayer, BackingOriginalPosition, OriginalPosition }
    private state enemyState;
    public enum Estado { Rotina, Alerta, Combate , Lockdown };
    public Estado estado;
    public enum Stances { Idle, Patrolling, Wait , CorrerAteUltimaPosicaoPlayer, VarrerFase };
    public Stances stance = Stances.Idle;
    public enum FazesMovimentoAlerta { NA, AndandoAte_UltimaPosicaoPlayer, chechandoUltimaPosicaoPlayer,VoltandoA_RotinaPadrao }
    public FazesMovimentoAlerta fazerMovimentoAlerta = FazesMovimentoAlerta.NA;
    public enum ModoPatrulha {destraido, atento };
    public ModoPatrulha modoPatrulha;


    public float waitTime;
    public float startWaitTime;
    public List<Transform> moveSpots = new List<Transform>();
    private int lastMoveSpot;
    private int randomSpot;
    public bool hearShoot;
    bool viuPlayerUmaVez;
    LockDown lockDown;
    float difPlayer;
    float difLockDownButton;

    

    [SerializeField]bool vendoPlayer;
    [SerializeField]private Vector3 ultimaPosicaoOrigem;
    [SerializeField]private GameObject gameObjectPlayerReservaAlt;
    bool enemyPlayerReserva = false;

    //CONTADORES
     float contadorAlertaTempo;
    [SerializeField] float contadorAlertaTempoMax;
     private float timeAlert = 0.0F;

    bool noContadorAlert;
    private void Start()
    {
        //componentes
        pathFinding = GetComponent<PathFinding>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        lockDown = FindObjectOfType<LockDown>();

        ultimaPosicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
        stance = Stances.Patrolling;
        randomSpot = 0;

        initialPosition.x = transform.position.x;
        initialPosition.y = transform.position.y;

        //controles de tempo
        waitTime = startWaitTime;
        timeMaxOriginalKnock = timeMaxKnock;
        timeMaxOriginalKnockCont = timeMaxKnockCont;
        timeMaxAlert = timeMaxAlertOriginal;
        contadorAlertaTempo = contadorAlertaTempoMax;
    }
    public void EscutarSom(Player player, bool somTiro)
    {
        playerSoundPosition = player.transform.position;
        hearPlayer = true;
        if (somTiro == true)
        {
            hearShoot = true;
        }
    }

    void counterAlert()
    {
        rb.velocity = Vector2.zero;
        if (estado == Estado.Alerta)
        {
            noContadorAlert = true;
            timeAlert += Time.deltaTime;
            if (timeAlert > timeMaxAlert)
            {
                estado = Estado.Combate;
                noContadorAlert = false;
            }
        }
    }

    void CounterAlertTimer()
    {
        if (noContadorAlert && !vendoPlayer)
        {
            contadorAlertaTempo += Time.deltaTime;
            if (contadorAlertaTempo > contadorAlertaTempoMax)
            {
                timeAlert = 0;
                contadorAlertaTempo = 0;
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

        fazerMovimentoAlerta = FazesMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
        lastPlayerPosition = gameObjectPlayerReservaAlt.transform.position;
        difPlayer = Vector2.Distance(playerGameObject.transform.position, transform.position);


        if (!knockBacking) 
        { 
            if (firstTimeOnLoop)
            {
                firstTimeOnLoop = false;
                EntrarModoPatrulha();
            }
            else
                ultimaPosicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);


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
            enemy.UseItem();//ataque
        }
        else if (enemyVision.seePlayer)
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
        CounterAlertTimer();
        vendoPlayer = playerGameObject;
        difLockDownButton = Vector2.Distance(lockDown.transform.position, transform.position);

        if (vendoPlayer)//caso esteja vendo o player
        {
            switch (estado)
            {
                case Estado.Combate:
                    viuPlayerUmaVez = true;
                    seguirAtacarPlayer();
                    break;
                case Estado.Alerta:
                    counterAlert();//inicia o contador pra ir entrar em estado combate 
                    break;
                case Estado.Rotina:
                    estado = Estado.Alerta;
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
                        case FazesMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer:
                            MovimentarUltimaPosicaoPlayer(lastPlayerPosition);
                            break;
                        case FazesMovimentoAlerta.chechandoUltimaPosicaoPlayer:
                            VerificarRegiao();
                            break;
                        case FazesMovimentoAlerta.VoltandoA_RotinaPadrao:
                            MovimentarVoltarRotinaPadrao(ultimaPosicaoOrigem);
                            break;
                        case FazesMovimentoAlerta.NA:
                            estado = Estado.Rotina;
                            break;
                    }
                    break;


                case Estado.Rotina: //fazendo rotina
                    if (false)//hearPlayer
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
        ResetKnockBackCont();
        KnockBackContador();
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
            timePlayerReserva += Time.deltaTime;
            if (timePlayerReserva > timePlayerReservaMax)//pega a ultima posicao do player conhecida e passa a pra variavel, zera as outras coisas
            {
                timePlayerReserva = 0;//depois daquele tempo em que segue o jogador voltar pro alerta
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
        time += Time.deltaTime;
        if (time > timeMax)
        {
            fazerMovimentoAlerta = FazesMovimentoAlerta.VoltandoA_RotinaPadrao;
            lastPlayerPositionChecked = true;
            time = 0;
        }

    }
    private void OuvindoInimigo()
    {

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
        fazerMovimentoAlerta = FazesMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
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
            Debug.Log("to indo pra ultima posicao do player");
            Movimentar(calcMovimemto(lastPlayerPosition));
        }
        else
        {
            Debug.Log("chegued no last player");
            fazerMovimentoAlerta = FazesMovimentoAlerta.chechandoUltimaPosicaoPlayer;
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
            fazerMovimentoAlerta = FazesMovimentoAlerta.NA;
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
                ultimaPosicaoOrigem = moveSpots[lastMoveSpot].position;
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
    public void KnockBack(float _horizontal, float _vertical, float _knockBack)
    {
        knockBackCont++;
        timeKnockCont = 0;
        if (knockBackCont == contQuantosTirosParaTomarKnockBack)
            KnockBackComplex(_horizontal, _vertical, _knockBack);
        else
            KnockBackSimple(_horizontal, _vertical, _knockBack);
    }
    void ResetKnockBackCont()
    {
        if (knockBackCont != 0)
        {
            //inicia contador de tempo para levar o knockback forte
            timeKnockCont += Time.deltaTime;
            if (timeKnockCont > timeMaxKnockCont)
            {
                knockBackCont = 0;
                timeMaxKnockCont = timeMaxOriginalKnockCont;
                timeKnockCont = 0;
            }
        }
    }
    void KnockBackSimple(float _horizontal, float _vertical, float _knockBack)
    {
        transform.position = new Vector3(transform.position.x + _horizontal * _knockBack, transform.position.y + _vertical * _knockBack, transform.position.z);
    }
    void KnockBackComplex(float _horizontal, float _vertical, float _knockBack)
    {
        transform.position = new Vector3(transform.position.x + _horizontal * _knockBack, transform.position.y + _vertical * _knockBack, transform.position.z);
        knockBacking = true;
        Knock = true;
        knockBackCont = 0;
    }

    public void EnemyVissionReference(EnemyVision _enemyVision)
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
            lastPlayerPositionChecked = false;
            time = 0;
            enemyPlayerReserva = false;
            timePlayerReserva = 0;
            playerGameObject = null;
        }
        else
        {
            playerGameObject = _whoEnemySaw;
            time = 0;
            timePlayerReserva = 0;
        }

    }
    void KnockBackContador()
    {
        if (Knock)
        {
            timeKnock += Time.deltaTime;
            if (timeKnock > timeMaxKnock)
            {
                Knock = false;
                timeMaxKnock = 0.0F;
                timeKnock = 0;
            }
        }
        else
        {
            timeMaxKnock = timeMaxOriginalKnock;
            knockBacking = false;
        }
    }

    public void ZerarVelocidade()
    {
        rb.velocity = new Vector2(0, 0);
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



