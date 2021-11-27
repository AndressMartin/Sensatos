using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public int velocity;
    public Vector3 initialPosition;
    private EnemyVision enemyVision;
    [SerializeField] private GameObject playerGameObject;
    public Rigidbody2D rb;
    private Enemy enemy;
    [SerializeField] private float horizontal = 0, vertical = 0;

    [SerializeField] private bool lastPlayerPositionChecked;
    [SerializeField] private Vector3 lastPlayerPosition;
    [SerializeField] private float time = 0.0F;
    [SerializeField] private float timeMax = 0;

    public float velX;
    public float velY;

    private enum state { followPlayer, attackingPlayer, searchingPlayer, BackingOriginalPosition, OriginalPosition }
    private state enemyState;

    [SerializeField] private float timePlayerResrva = 0.0F;
    [SerializeField] private float timePlayerResrvaMax = 0;
    public GameObject gameObjectPlayerReserva;
    bool firstTimeOnLoop = true;

    private PathFinding pathFinding;

    public float knockBackHorizontal, knockBackVertical;
    public bool Knock;
    [SerializeField] private float timeKnock, timeMaxKnock;
    private float timeMaxOriginalKnock;

    [SerializeField] private float timeKnockCont, timeMaxKnockCont;
    private float timeMaxOriginalKnockCont;
    bool knockBacking;
    int knockBackCont = 0;
    [SerializeField] private int contQuantosTirosParaTomarKnockBack;

    private Vector3 enemySoundPosition;
    [SerializeField] private bool hearEnemy;
    private GameObject enemySound;

    public enum Estado { rotina, alerta, combate };

    public Estado estado;
    private float timeAlert;
    private float timeMaxAlert;
    [SerializeField] private float timeMaxAlertOriginal;
    private Vector3 transformtemp;

    public enum Stances { idle, patrolling, wait };
    public Stances stance = Stances.idle;
    public enum FazesMovimentoAlerta {AndandoAte_UltimaPosicaoPlayer,chechandoUltimaPosicaoPlayer,VoltandoA_RotinaPadrao,NA }
    public FazesMovimentoAlerta fazesMovimentoAlerta = FazesMovimentoAlerta.NA;

    public float waitTime;
    public float startWaitTime;
    public List<Transform> moveSpots = new List<Transform>();
    private int lastMoveSpot;
    private int randomSpot;
    bool hearShoot;
    bool viuPlayerUmaVez;
    lockDawn lockDawn;
    float difPlayer;
    float difLockDawnButton;
    [SerializeField]bool vendoPlayer;

    [SerializeField]private Vector3 ultimaposicaoOrigem;
    private void Start()
    {
        ultimaposicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
        waitTime = startWaitTime;
        stance = Stances.patrolling;
        randomSpot = 0;

        pathFinding = GetComponent<PathFinding>();
        rb = GetComponent<Rigidbody2D>();
        initialPosition.x = transform.position.x;
        initialPosition.y = transform.position.y;
        enemy = GetComponent<Enemy>();
        timeMaxOriginalKnock = timeMaxKnock;
        timeMaxOriginalKnockCont = timeMaxKnockCont;

        timeMaxAlert = timeMaxAlertOriginal;
        lockDawn = FindObjectOfType<lockDawn>();
    }
    public void HearEnemy(Player _gameObject, float _tamanhoRaio)
    {
        enemySound = _gameObject.gameObject;
        enemySoundPosition = enemySound.transform.position;
        hearEnemy = true;
        if (_tamanhoRaio > 2)
            hearShoot = true;
    }
    void counterAlert()
    {
        if (estado == Estado.alerta)
        {
            timeAlert += Time.deltaTime;
            if (timeAlert > timeMaxAlert)
            {
                estado = Estado.combate;
            }
        }
    }

    public void seguirAtacarPlayer()
    {
        if (!knockBacking) 
        {
            float difPlayer = Vector2.Distance(playerGameObject.transform.position, transform.position);
            

            if (firstTimeOnLoop)
            {
                firstTimeOnLoop = false;
            }
            else
                ultimaposicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);



            if (!lockDawn.ativo) //caso não esteja em lockdawn
            {
                if (difPlayer < difLockDawnButton)//caso o player estja mais perto que o alarme vai pra cima do player
                {
                    lastPlayerPosition = playerGameObject.transform.position;
                    fazesMovimentoAlerta = FazesMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
                    if (enemyVision.playerOnAttackRange)//caso esteja dentro do range de ataque 
                    {
                        enemy.UseItem();
                    }
                    else if (enemyVision.seePlayer)
                    {
                        Vector2 direction = playerGameObject.transform.position - transform.position;
                        direction.Normalize();
                        MOVE(direction);
                        //Debug.Log("seguind player");
                    }
                }
                else if (difPlayer > difLockDawnButton)//caso alarme estje mais perto ativa o alarme
                {
                    if (difLockDawnButton > 0.05)
                    {
                        Vector2 direction = lockDawn.transform.position - transform.position;
                        direction.Normalize();
                        MOVE(direction);
                    }
                    else
                    {
                        lockDawn.ativo = true;
                    }
                }
            }
            else // vai pra cima do player
            {
                lastPlayerPosition = playerGameObject.transform.position;
                if (enemyVision.playerOnAttackRange)//caso esteja dentro do range de ataque 
                {
                    enemy.UseItem();
                }
                else if (enemyVision.seePlayer)
                {
                    Vector2 direction = playerGameObject.transform.position - transform.position;
                    direction.Normalize();
                    MOVE(direction);
                    //Debug.Log("seguind player");
                }
            }
        }
    }
    public void Main()
    {
        vendoPlayer = playerGameObject;
        difLockDawnButton = Vector2.Distance(lockDawn.transform.position, transform.position);


        if (vendoPlayer)//caso esteja vendo o player
        {
            switch (estado)
            {
                case Estado.combate:
                    viuPlayerUmaVez = true;
                    seguirAtacarPlayer();
                    break;
                case Estado.alerta:
                    counterAlert();
                    break;
                case Estado.rotina:
                    estado = Estado.alerta;
                    break;

            }
            if (gameObjectPlayerReserva == null)
                gameObjectPlayerReserva = playerGameObject;
        }
        else //caso não veja o player
        {
            switch (estado)
            {

                case Estado.alerta://ver player
                    switch (fazesMovimentoAlerta)
                    {
                        case FazesMovimentoAlerta.NA:
                            break;
                        case FazesMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer:
                            MovimentarUltimaPosicaoPlayer(lastPlayerPosition);
                            break;
                        case FazesMovimentoAlerta.chechandoUltimaPosicaoPlayer:
                            VerificarRegiao();
                            break;
                        case FazesMovimentoAlerta.VoltandoA_RotinaPadrao:
                            MovimentarVoltarRotinaPadrao(new Vector2(0, 0));
                            break;
                        
                    }
                    break;


                case Estado.rotina: //fazendo rotina
                    if (hearEnemy)
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

    private void VerificarRegiao()
    {
        rb.velocity = Vector2.zero;
    }
    private void OuvindoInimigo()
    {

    }
    private void Movimentar(Vector2 posicao)
    {
        rb.velocity = posicao;//andando ate a o posica passada
        CollisionDirection();
    }
    private void MovimentarUltimaPosicaoPlayer(Vector2 _posicao)
    {
        if (Vector2.Distance(transform.position,_posicao) > 0.5f)
        {
            Vector2 PlayerPosition = transform.position;
            Vector2 directionTemp = _posicao - PlayerPosition;
            Vector2 direction = directionTemp.normalized * velocity;

            Debug.Log("to indo pra la");
            Movimentar(direction);
        }
        else
        {
            Debug.Log("chegued");
            fazesMovimentoAlerta = FazesMovimentoAlerta.chechandoUltimaPosicaoPlayer;
        }
    }
    private void MovimentarVoltarRotinaPadrao(Vector2 _posicao)
    {
        Movimentar(_posicao);
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
    private void MoveTransform(Vector2 _direction)
    {
        transform.position = Vector2.MoveTowards(transform.position, _direction, velocity / 3 * Time.deltaTime);
        CollisionDirection();
    }
    private void MOVE(Vector2 _direction)
    {
        //rb.MovePosition((Vector2)transform.position + (_direction * velocity * Time.deltaTime));
        _direction.Normalize();
        rb.velocity = new Vector2((_direction.x) * velocity * Time.deltaTime, (_direction.y) * velocity * Time.deltaTime);
        CollisionDirection();
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

    public void SawEnemy(GameObject _whoEnemySaw)
    {
        lastPlayerPositionChecked = false;
        playerGameObject = _whoEnemySaw;
        timeMax = 3;
        time = 0;
        timePlayerResrvaMax = 1.5F;
        timePlayerResrva = 0;
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


}
/*Moviment~ção old
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



