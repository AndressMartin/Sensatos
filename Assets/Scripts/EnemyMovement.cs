using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Componentes
    private LockDown lockDown;
    private Enemy enemy;
    private Rigidbody2D rb;
    private IA_Enemy iA_Enemy;

    //Variaveis
    [SerializeField] private int velocidade;
    [SerializeField] private List<Transform> moveSpots = new List<Transform>();

    private float velX;
    private float velY;

    private Vector2 vetorKnockBack;

    //Variaveis de controle

    [SerializeField] private Vector2 ultimaposicaoOrigem;
    private int randomSpot;
    private int lastMoveSpot;
   
    //Contadores
    private float timeKnockBack;
    private float timeKnockBackMax;

    //Getters
    public Rigidbody2D GetRb => rb;
    public Vector2 GetUltimaPosicaoOrigem => ultimaposicaoOrigem;


    public void ResetarVariaveisDeControle()
    {
        ultimaposicaoOrigem = Vector3.zero;
        randomSpot = 0;
        lastMoveSpot = 0;
        velocidade = 2;
    }

    private void Start()
    {
        iA_Enemy = GetComponent<IA_Enemy>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        lockDown = FindObjectOfType<LockDown>();

        ultimaposicaoOrigem = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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





    ///-----------------------------------------------------------------------
    ///tem uso mas verificar se pode melhorar%
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
            iA_Enemy.stance = IA_Enemy.Stances.Patrolling;
        }
    }
    public void VarrerFase()
    {
        iA_Enemy.fazerMovimentoAlerta = IA_Enemy.FazerMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer;
        iA_Enemy.estado = IA_Enemy.Estado.Alerta;
        iA_Enemy.stance = IA_Enemy.Stances.Patrolling;

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

}




