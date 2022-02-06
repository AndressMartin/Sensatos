using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Componentes
    private Enemy enemy;
    private Rigidbody2D rb;
    private IA_Enemy_Basico iA_Enemy;

    //Variaveis
    [SerializeField] private int velocidade;
    [SerializeField] private List<Transform> moveSpots = new List<Transform>();

    private float velX;
    private float velY;

    private Vector2 vetorKnockBack;

    //Variaveis de controle

    private Vector2 ultimaPosicaoEnquantoFaziaRota;
    private int randomSpot;
    private int lastMoveSpot;
   
    //Contadores
    private float timeKnockBack;
    private float timeKnockBackMax;

    //Getters
    public Rigidbody2D GetRb => rb;
    public Vector2 GetUltimaPosicaoOrigem => ultimaPosicaoEnquantoFaziaRota;


    public void ResetarVariaveisDeControle()
    {
        ultimaPosicaoEnquantoFaziaRota = Vector3.zero;
        randomSpot = 0;
        lastMoveSpot = 0;
        velocidade = 2;
    }

    private void Start()
    {
        iA_Enemy = GetComponent<IA_Enemy_Basico>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();

        ultimaPosicaoEnquantoFaziaRota = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        randomSpot = 0;

        //Variaveis de controle
        timeKnockBack = 0;
        timeKnockBackMax = 0.5f;
    }


    public void Main()
    {
        if (enemy.GetEstado == Enemy.Estado.TomandoDano)
        {
            KnockBackContador();
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
    public void Patrulhar()
    {
        Vector2 directionTemp = moveSpots[randomSpot].position - transform.position;
        Vector2 direction = directionTemp.normalized * velocidade;
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
                ultimaPosicaoEnquantoFaziaRota = moveSpots[lastMoveSpot].position;
            }
            else
            {
                while (randomSpot == lastMoveSpot)
                {
                    randomSpot = Random.Range(0, moveSpots.Count);
                }
            }
        }

    }
    public void VarrerFase()
    {


        //coisas
        //fim da funcao


    }
    public Vector2 calcMovimemto(Vector2 __posicao)
    {
        Vector2 PlayerPosition = transform.position;
        Vector2 directionTemp = __posicao - PlayerPosition;
        Vector2 direction = directionTemp.normalized * velocidade;

        return direction;
    }

    public void Movimentar(Vector2 posicao)
    {
        rb.velocity = posicao;//andando ate a o posica passada
        CollisionDirection();
    }
    public void ZerarVelocidade()
    {
        rb.velocity = Vector2.zero;
    }
    public void ReceberMoveSpots(List<Transform> _moveSpots)
    {
        moveSpots.Clear();
        moveSpots = _moveSpots;
    }

}




