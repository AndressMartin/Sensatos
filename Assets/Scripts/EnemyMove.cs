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
    [SerializeField]private float horizontal=0, vertical=0;
    private bool hearEnemy;

    [SerializeField] private bool lastPlayerPositionChecked;
    [SerializeField] private Vector3 lastPlayerPosition;
    [SerializeField] private float time = 0.0F;
    [SerializeField] private float timeMax = 0;

    private enum state {followPlayer,attackingPlayer,searchingPlayer,BackingOriginalPosition,OriginalPosition }
    private state enemyState;

    [SerializeField] private float timePlayerResrva = 0.0F;
    [SerializeField] private float timePlayerResrvaMax = 0;
    public GameObject gameObjectPlayerReserva;
    bool firstTimeOnLoop=true;

    [SerializeField] private PathFinding pathFinding;

    public float knockBackHorizontal, knockBackVertical;
    public bool Knock;
    [SerializeField] private float timeKnock, timeMaxKnock;
    private float timeMaxOriginalKnock;

    [SerializeField] private float timeKnockCont, timeMaxKnockCont;
    private float timeMaxOriginalKnockCont;
    bool knockBacking;
    int knockBackCont=0;
    [SerializeField]private int contQuantosTirosParaTomarKnockBack;


    private void Start()
    {
        pathFinding = GetComponent<PathFinding>();
        rb = GetComponent<Rigidbody2D>();
        initialPosition.x = transform.position.x;
        initialPosition.y = transform.position.y;
        enemy = GetComponent<Enemy>();
        timeMaxOriginalKnock = timeMaxKnock;
        timeMaxOriginalKnockCont = timeMaxKnockCont;
    }

    public void Main()
    {
        /*if (playerGameObject != null)
        {
            pathFinding.ReceivePlayerGameObject(playerGameObject);
        }
        else
            pathFinding.ReceivePlayerGameObject(null);*/
        
        
        if (playerGameObject != null && !knockBacking)//caso esteja vendo o player
        {
            if (firstTimeOnLoop)
            {
                firstTimeOnLoop = false;
            }
            if(gameObjectPlayerReserva == null)
                gameObjectPlayerReserva = playerGameObject;


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
                Debug.Log("seguind player");
            }

            
        }
        else if(playerGameObject == null && gameObjectPlayerReserva != null)
        {
            lastPlayerPosition = gameObjectPlayerReserva.transform.position;
            timePlayerResrva += Time.deltaTime;
            if (timePlayerResrva > timePlayerResrvaMax)
            {
                timePlayerResrvaMax = 0.0F;
                timePlayerResrva = 0;
                gameObjectPlayerReserva=null;
            }
        }
        /*if()
        {

        }*/

        if(!firstTimeOnLoop && playerGameObject == null)
        {
            if (!lastPlayerPositionChecked)//se o inimigo ja checou a ulitma posicao conehceida
            {
                if (Mathf.Abs( lastPlayerPosition.x - transform.position.x) >= 0.1 && Mathf.Abs( lastPlayerPosition.y - transform.position.y) >= 0.1)//caso o inimigo não tenha chego na ultima posicao do player
                {
                    Vector3 direction = lastPlayerPosition - transform.position;
                    direction.Normalize();                                         
                    MOVE(direction);
                    Debug.Log("ultima posicao do  player");
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
                if (Mathf.Abs(initialPosition.x - transform.position.x) <= 0.1 && Mathf.Abs(initialPosition.y - transform.position.y) <= 0.1) // voltar para origem
                {
                    Debug.Log("Cheguei a minha origme");
                }
                else
                {
                     Vector3 direction = initialPosition - transform.position;
                     direction.Normalize();
                     MOVE(direction);
                     Debug.Log("voltando origemr");
                }
                if (transform.position.x < initialPosition.x)//Inimigo A esquerda origem a direita
                    horizontal = 1;

                else if (transform.position.x > initialPosition.x)//Inimigo a direita origem A esquerda 
                    horizontal = -1;

                else if (transform.position.y < initialPosition.y)//Iniimgo a baixo do origem
                    vertical = 1;

                else if (transform.position.y > initialPosition.y)//origem a baixo do inimigo
                    vertical = -1;

                Debug.Log("Não vejo o player, voltando a origem");
            }
        }
        ResetKnockBackCont();
        KnockBackContador();
    }
    public void HearEnemy()
    {

    }
    public void KnockBack(float _horizontal, float _vertical,float _knockBack)
    {
        knockBackCont++;
        timeKnockCont = 0;
        if (knockBackCont == contQuantosTirosParaTomarKnockBack)
            KnockBackComplex(_horizontal, _vertical, _knockBack);
        else
            KnockBackSimple(_horizontal,_vertical,_knockBack);
    }
    void ResetKnockBackCont()
    {
        if(knockBackCont !=0)
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
    void KnockBackSimple (float _horizontal, float _vertical, float _knockBack)
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

    private void MOVE(Vector2 _direction)
    {
        rb.MovePosition((Vector2)transform.position + (_direction * velocity * Time.deltaTime));
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


