using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //Componentes
    private GameObject gameObjectPlayerReservaAlt;
    private GameObject playerGameObject;
    private lockDown lockDown;
    private Enemy enemy;
    private EnemyVision enemyVision;
    private PathFinding pathFinding;
    public Rigidbody2D rb;

    //Variaveis
    public int velocity;
    public List<Transform> moveSpots = new List<Transform>();

    float difPlayer;
    float difLockDownButton;

    private float velX;
    private float velY;

    [SerializeField] private Vector3 lastPlayerPosition;

    

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
    bool Knock;
    bool knockBacking;
    bool noContadorAlert;
    bool enemyPlayerReserva = false;


    //CONTADORES
    private float timePlayerReserva = 0.0F;
    private float timeAlert = 0.0F;
    private float time = 0.0F;
    private float contadorAlertaTempo = 0.0F;

    private float timeKnock = 0.0F;
    private float timeKnockCont = 0.0F;
    private int knockBackCont = 0;

    //Tempos
    [SerializeField] private float timePlayerReservaMax;
    [SerializeField] private float timeMaxAlert;
    [SerializeField] private float timeMax;
    [SerializeField] float contadorAlertaTempoMax;
    [SerializeField] private float timeMaxKnock;
    [SerializeField] private float timeMaxKnockCont;

    //Enuns
    public enum Estado { rotina, alerta, combate, lockdown };
    public Estado estado;
    public enum Stances { idle, patrolling, wait, correrAteUltimaPosicaoPlayer, VarrerFase };
    public Stances stance = Stances.idle;
    public enum FazesMovimentoAlerta
    {        AndandoAte_UltimaPosicaoPlayer, chechandoUltimaPosicaoPlayer, VoltandoA_RotinaPadrao, NA,
        ouviuTiro, AndandoAte_UltimaPosicaoSomPlayer, ouviuPassos
    }
    public FazesMovimentoAlerta fazesMovimentoAlerta = FazesMovimentoAlerta.NA;
    public enum ModoPatrulha { destraido, atento };
    public ModoPatrulha modoPatrulha;

    void getComponent()
    {
        pathFinding = GetComponent<PathFinding>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        lockDown = FindObjectOfType<lockDown>();
    }

    void zerarVariaveisDeControle()
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
        Knock = false;
        knockBacking = false;
        noContadorAlert = false;
        enemyPlayerReserva = false;
        timePlayerReserva = 0.0F;
        timeAlert = 0.0F;
        time = 0.0F;
        contadorAlertaTempo = 0.0F;
        timeKnock = 0.0F;
        timeKnockCont = 0.0F;
        knockBackCont = 0;
    }

    private void Start()
    {
        //componentes
        getComponent();

        ultimaposicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);        
        stance = Stances.patrolling;
        randomSpot = 0;
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
        if (estado == Estado.alerta)
        {
            noContadorAlert = true;
            timeAlert += Time.deltaTime;
            if (timeAlert > timeMaxAlert)
            {
                estado = Estado.combate;
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

        if(hearPlayer || hearPlayer)
        {
            hearPlayer = false;
            hearShoot = false;  
        }

        fazesMovimentoAlerta = FazesMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
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
                ultimaposicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);


            if (!lockDown.ativo) //caso n�o esteja em lockdawn
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
                        lockDown.ativo = true;  //ativa o lockdown
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
                case Estado.combate:
                    seguirAtacarPlayer();
                    break;
                case Estado.alerta:
                    counterAlert();//inicia o contador pra ir entrar em estado combate 
                    break;
                case Estado.rotina:
                    estado = Estado.alerta;
                    break;

            }

        }
        else //caso n�o veja o player
        {
            switch (estado)
            {
                case Estado.combate:
                    estado = Estado.alerta;
                    break;

                case Estado.lockdown:
                    switch (stance)
                    {
                        case Stances.correrAteUltimaPosicaoPlayer:
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

                case Estado.alerta://ver player
                    switch (fazesMovimentoAlerta)
                    {
                        case FazesMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer:
                            MovimentarUltimaPosicaoPlayer(lastPlayerPosition);
                            break;
                        case FazesMovimentoAlerta.chechandoUltimaPosicaoPlayer:
                            VerificarRegiao();
                            break;
                        case FazesMovimentoAlerta.VoltandoA_RotinaPadrao:
                            MovimentarVoltarRotinaPadrao(ultimaposicaoOrigem);
                            break;
                        case FazesMovimentoAlerta.ouviuTiro:
                            OuviuTiro();
                            break;
                        case FazesMovimentoAlerta.AndandoAte_UltimaPosicaoSomPlayer:
                            MovimentarAteOSom();
                            break;
                        case FazesMovimentoAlerta.ouviuPassos:
                            OlharDirecaoSom();
                            break;

                        case FazesMovimentoAlerta.NA:
                            estado = Estado.rotina;
                            break;
                    }
                    break;


                case Estado.rotina: //fazendo rotina
                    if (hearPlayer || hearShoot)//hearPlayer
                    {
                        OuvindoInimigo();
                    }
                    else
                    {
                        switch (stance)
                        {
                            case Stances.patrolling://patrulhando
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
            estado = Estado.lockdown;
            stance = Stances.correrAteUltimaPosicaoPlayer;
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
            else        //enquanto o contador n�o chegar no numero fique atualizando a posicao do player e retornando pra onde o inimigo deve andar
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
            fazesMovimentoAlerta = FazesMovimentoAlerta.VoltandoA_RotinaPadrao;
            time = 0;
        }

    }

    private void OuviuTiro()
    {
        Debug.Log("To no tiro");
        if (Vector2.Distance(playerSoundPosition, transform.position) >= 0.1)//caso o inimigo n�o tenha chego na ultima posicao do player
        {
            MoveGeneric(playerSoundPosition);
            Debug.Log("To andando ate tiro");
        }
        else
        {
            fazesMovimentoAlerta = FazesMovimentoAlerta.AndandoAte_UltimaPosicaoSomPlayer;
            hearShoot = false;
            hearPlayer = hearShoot;
            Debug.Log("� ture"+ fazesMovimentoAlerta);
        }
        
    }
    private void OuvindoInimigo()
    {
        
        Debug.Log("to ouvindo algo Shoot: "+hearShoot+" Player: "+hearPlayer);
        if (hearShoot)
        {
            estado = Estado.alerta;
            fazesMovimentoAlerta = FazesMovimentoAlerta.ouviuTiro;
        }
        else if(hearPlayer)
        {
            estado = Estado.alerta;
            fazesMovimentoAlerta = FazesMovimentoAlerta.ouviuPassos;
        }
        //else
        //fazesMovimentoAlerta = FazesMovimentoAlerta.NA;

        /*if (Vector2.Distance(playerSoundPosition, transform.position) >= 0.1)//caso o inimigo n�o tenha chego na ultima posicao do player
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
        fazesMovimentoAlerta = FazesMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
        estado = Estado.alerta;

        //coisas
        //fim da funcao


    }
    private void MovimentarUltimaPosicaoPlayer(Vector2 _posicao)
    {
        
        if(!enemyPlayerReserva)//so atualiza a posicao do jogador se o contador n�o deu o tempo
            _posicao = InimigoSeguindoAposPerderVisaoDoPlayerContador(_posicao);

        if (Vector2.Distance(transform.position, _posicao) > 0.5f)
        {
            Movimentar(calcMovimemto(lastPlayerPosition));
        }
        else
        {
            fazesMovimentoAlerta = FazesMovimentoAlerta.chechandoUltimaPosicaoPlayer;
        }
        
    }
    void OlharDirecaoSom()
    {
        float difX = transform.position.x - playerSoundPosition.x;
        float difY = transform.position.y - playerSoundPosition.y;


        float dif= Vector2.Distance(transform.position, playerSoundPosition);

        if (difX > 0)
            enemy.direcao = EntityModel.Direcao.Esquerda;
        else
            enemy.direcao = EntityModel.Direcao.Direita;
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
            fazesMovimentoAlerta = FazesMovimentoAlerta.chechandoUltimaPosicaoPlayer;
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
            fazesMovimentoAlerta = FazesMovimentoAlerta.NA;
            estado = Estado.rotina;
            stance = Stances.patrolling;
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
            stance = Stances.patrolling;
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
                timeKnock = 0;
            }
        }
        else
        {
            knockBacking = false;
        }
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

                if (Vector2.Distance(lastPlayerPosition,transform.position) >= 0.1)//caso o inimigo n�o tenha chego na ultima posicao do player
                {

                    Vector2 direction = lastPlayerPosition - transform.position;
                    direction.Normalize();
                    MOVE(direction);

                    //Debug.Log("indo at� a ultima posicao do  player");
                }
                else
                {                                         //inimigo come�a a contar como se estivesse procurando o player por um tempo na regi�o                  
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

                //Debug.Log("N�o vejo o player, voltando a origem");
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
        else if (hearEnemy && estado != Estado.combate)//caso ou�a o inimigo
        {
            if (hearShoot)//caso tenha tiro
            {
                estado = Estado.alerta;
                if (Vector2.Distance(enemySoundPosition,transform.position) >= 0.1)//caso o inimigo n�o tenha chego na ultima posicao do player
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
                    if (Vector2.Distance(enemySoundPosition, transform.position) >= 0.1)//caso o inimigo n�o tenha chego na ultima posicao do player
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



