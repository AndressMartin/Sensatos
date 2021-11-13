using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public int velocity;
    public Vector3 initialPosition;
    private EnemyVision enemyVision;
    [SerializeField] private GameObject playerGameObject;
    private Rigidbody2D rb;
    private Enemy enemy;
    [SerializeField] private float horizontal = 0, vertical = 0;



    [SerializeField] private bool lastPlayerPositionChecked;
    [SerializeField] private Vector3 lastPlayerPosition;
    [SerializeField] private float time = 0.0F;
    [SerializeField] private float timeMax = 0;

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
    public float waitTime;
    public float startWaitTime;
    public List<Transform> moveSpots = new List<Transform>();
    private int lastMoveSpot;
    private int randomSpot;
    bool hearShoot;

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
        if (!knockBacking) //entrar em modo de alerta por x tempos, caso continue vendo o player executa os ifs abaixo
        {

            if (firstTimeOnLoop)
            {
                firstTimeOnLoop = false;
            }
            else
                ultimaposicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);




            lastPlayerPosition = playerGameObject.transform.position;
            if (enemyVision.playerOnAttackRange)//caso esteja dentro do range de ataque 
            {
                enemy.UseItem();
            }

            else if (enemyVision.seePlayer)
            {
                Vector3 direction = playerGameObject.transform.position - transform.position;
                direction.Normalize();
                MOVE(direction);
                //Debug.Log("seguind player");
            }
        }
    }
    public void Main()
    {

        /*if (playerGameObject != null)
        {
            pathFinding.ReceivePlayerGameObject(playerGameObject);
        }
        else
            pathFinding.ReceivePlayerGameObject(null);*/
        //Debug.Log(estado);

        if (playerGameObject != null)//caso esteja vendo o player
        {
            switch (estado)
            {
                case Estado.combate:
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
        else if (playerGameObject == null)//caso não veja o player
        {
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

                if (Mathf.Abs(lastPlayerPosition.x - transform.position.x) >= 0.1 && Mathf.Abs(lastPlayerPosition.y - transform.position.y) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
                {

                    Vector2 direction = lastPlayerPosition - transform.position;
                    direction.Normalize();
                    MOVE(direction);

                    Debug.Log("indo até a ultima posicao do  player");
                }
                else
                {                                         //inimigo começa a contar como se estivesse procurando o player por um tempo na região                  
                    Debug.Log("contador player");

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
                    Debug.Log("Cheguei a minha origme");
                }
                else
                {
                    Vector2 direction = initialPosition - transform.position;
                    direction.Normalize();
                    MOVE(direction);

                    //MoveTransform(direction);
                    Debug.Log("voltando origemr");
                }

                Debug.Log("Não vejo o player, voltando a origem");
            }
        }
        if (playerGameObject == null && estado == Estado.rotina && stance != Stances.wait)//patrulando
        {

            //Vector3 direction = moveSpots[randomSpot].position - transform.position;
            //direction.Normalize();
            //MOVE(direction);
            MoveTransform(moveSpots[randomSpot].position);

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
        if (hearEnemy && estado != Estado.combate)//caso ouça o inimigo
        {
            if (hearShoot)//caso tenha tiro
            {
                estado = Estado.alerta;
                if (Mathf.Abs(enemySoundPosition.x - transform.position.x) >= 0.1 && Mathf.Abs(enemySoundPosition.y - transform.position.y) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
                {
                    Vector3 direction = enemySoundPosition - transform.position;
                    direction.Normalize();
                    MOVE(direction);
                }
            }
            else//caso seja por passos
            {
                if (estado == Estado.alerta)
                {
                    if (Mathf.Abs(enemySoundPosition.x - transform.position.x) >= 0.1 && Mathf.Abs(enemySoundPosition.y - transform.position.y) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
                    {
                        Vector3 direction = enemySoundPosition - transform.position;
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
        ResetKnockBackCont();
        KnockBackContador();
        float x, y, z;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
        transformtemp = new Vector3(x, y, z);
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

    private void MoveTransform(Vector2 _direction)
    {
        transform.position = Vector2.MoveTowards(transform.position, _direction, velocity / 3 * Time.deltaTime);
        CollisionDirection();

    }
    private void MOVE(Vector2 _direction)
    {
        rb.MovePosition((Vector2)transform.position + (_direction * velocity * Time.deltaTime));
        CollisionDirection();
    }
    private void CollisionDirection()
    {
        float xdif = transformtemp.x - transform.position.x;
        float ydif = transformtemp.y - transform.position.y;


        if (Mathf.Abs(ydif) >= Mathf.Abs(xdif))
        {
            if (ydif < 0)
            {
                enemy.ChangeDirection(Enemy.Direcao.Cima);
            }
            else if (ydif > 0)
            {
                enemy.ChangeDirection(Enemy.Direcao.Baixo);
            }
        }

        else if (xdif > 0)
        {
            enemy.ChangeDirection(Enemy.Direcao.Esquerda);
        }
        else if (xdif < 0)
        {
            enemy.ChangeDirection(Enemy.Direcao.Direita);
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


}


