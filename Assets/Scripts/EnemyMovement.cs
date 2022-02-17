using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    //Componentes
    private Enemy enemy;
    private Rigidbody2D rb;
    private IA_Enemy_Basico iA_Enemy_Basico;

    //Variaveis
    [SerializeField] bool rotaIdaVolta;
    [SerializeField] private int velocidade;
    [SerializeField] private List<Transform> PontosRotaNaoLockdow = new List<Transform>();
    [SerializeField] private List<Transform> PontosRotaLockdow = new List<Transform>();

    private float velX;
    private float velY;

    private Vector2 vetorKnockBack;

    //Variaveis de controle

    private Vector2 ultimaPosicaoEnquantoFaziaRota;
    private int indiceListaNaoLockdown;
    private int indiceListaLockdown;

    private int indiceUltimoPontoListaNaoLockdonw;
    private int indiceUltimoPontoListaLockdonw;

    //Contadores
    private float timeKnockBack;
    private float timeKnockBackMax;

    //Getters
    public Rigidbody2D GetRb => rb;
    public Vector2 GetUltimaPosicaoOrigem => ultimaPosicaoEnquantoFaziaRota;
    public Vector2 GetPontosRotaNaoLockdow => PontosRotaNaoLockdow[indiceListaNaoLockdown].position;
    public Vector2 GetPontosRotaLockdow => PontosRotaLockdow[indiceListaLockdown].position;


    public int GetVelocidade => velocidade;

    void VerificarPontoPatrulhaMaisPerto()
    {
        int ponto0 = 0;
        float menorPonto = 5;

        for (int i = 0; i < PontosRotaNaoLockdow.Count; i++)
        {
            float distancia = Vector2.Distance(transform.position, PontosRotaNaoLockdow[i].transform.position);
            if (distancia <= menorPonto)
            {
                ponto0 = i;
                menorPonto = distancia;
            }
        }
        indiceListaNaoLockdown = ponto0;
    }
    
    void RotaIdaVolta()
    {
        if(rotaIdaVolta)
        {
            int valor = PontosRotaNaoLockdow.Count;
            for (int i = 0; i < valor; i++)
            {
                if (i != valor + 1)
                {
                    PontosRotaNaoLockdow.Add(PontosRotaNaoLockdow[valor - i - 1]);
                }
            }
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        iA_Enemy_Basico = GetComponent<IA_Enemy_Basico>();

        ultimaPosicaoEnquantoFaziaRota = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        indiceListaNaoLockdown = 0;
        indiceListaLockdown = 0;
        indiceUltimoPontoListaLockdonw =0;
        indiceUltimoPontoListaNaoLockdonw = 0;

        //Variaveis de controle
        timeKnockBack = 0;
        timeKnockBackMax = 0.5f;

        VerificarPontoPatrulhaMaisPerto();
        RotaIdaVolta();
    }


    public void Main()
    {
        if (enemy.GetEstado == Enemy.Estado.TomandoDano)
        {
            KnockBackContador();
        }
    }

    private void DefinirDirecao()
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

    
    public void GerarNovoPonto(bool pontoAleatorio)
    {
        if (pontoAleatorio)
            GerarPontoAleatorio();
        else
            GerarPontoSequencial();
    }
    void GerarPontoSequencial()
    {
        GeradorDePontos(ref indiceListaNaoLockdown,ref indiceUltimoPontoListaNaoLockdonw, PontosRotaNaoLockdow);
    }
    void GerarPontoAleatorio()
    {
        indiceListaLockdown = Random.Range(0, PontosRotaLockdow.Count);
    }
    void GeradorDePontos(ref int _pontoIndiceLista,ref int _ultimoPontoQueFui, List<Transform> _ListaDePontosUtilizada)
    {
        if (_pontoIndiceLista >= _ListaDePontosUtilizada.Count - 1)
            _pontoIndiceLista = 0;
        else
            _pontoIndiceLista++;

        if (_pontoIndiceLista != _ultimoPontoQueFui)
        {
            _ultimoPontoQueFui = _pontoIndiceLista;
            ultimaPosicaoEnquantoFaziaRota = _ListaDePontosUtilizada[_ultimoPontoQueFui].position;
        }
        else
        {
            while (_pontoIndiceLista == _ultimoPontoQueFui)
            {
                _pontoIndiceLista = Random.Range(0, _ListaDePontosUtilizada.Count);
            }
        }
    }
    public void VarrerFase()
    {

        iA_Enemy_Basico.Mover(PontosRotaLockdow[indiceListaLockdown].position);
    
            //gera um novo lugar de waypoint
            if (indiceListaLockdown >= PontosRotaLockdow.Count - 1)
                indiceListaLockdown = 0;
            else
                indiceListaLockdown++;

            indiceListaLockdown = Random.Range(0, PontosRotaLockdow.Count);//para aleatorizar o proximo destino que ele vai
            if (indiceListaLockdown != indiceUltimoPontoListaLockdonw)
            {
                
                indiceUltimoPontoListaLockdonw = indiceListaLockdown;
                ultimaPosicaoEnquantoFaziaRota = PontosRotaLockdow[indiceUltimoPontoListaLockdonw].position;
            }
            else
            {
                while (indiceListaLockdown == indiceUltimoPontoListaLockdonw)
                {
                   
                    indiceListaLockdown = Random.Range(0, PontosRotaLockdow.Count);
                }
            }
        

        //coisas
        //fim da funcao


    }
    public Vector2 CalcMovimemto(Vector2 __posicao)
    {
        Vector2 PlayerPosition = transform.position;
        Vector2 directionTemp = __posicao - PlayerPosition;
        Vector2 direction = directionTemp.normalized * velocidade;

        return direction;
    }

    public void Movimentar(Vector2 posicao)
    {
        rb.velocity = posicao;//andando ate a o posica passada
        DefinirDirecao();
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

    public void ZerarVelocidade(AILerp aILerp)
    {
        aILerp.canMove = false;
        aILerp.speed = 0;
        aILerp.destination = transform.position;
    }
    public void ReceberMoveSpots(List<Transform> _moveSpots, List<Transform> _moveSpotsLockdown)
    {
        PontosRotaNaoLockdow.Clear();
        PontosRotaLockdow.Clear();
        PontosRotaNaoLockdow = _moveSpots;
        PontosRotaLockdow = _moveSpotsLockdown;
    }
    public void ResetarVariaveisDeControle()
    {
        ultimaPosicaoEnquantoFaziaRota = Vector3.zero;
        indiceListaNaoLockdown = 0;
        indiceListaLockdown = 0;
        indiceUltimoPontoListaNaoLockdonw = 0;
        indiceUltimoPontoListaLockdonw = 0;
        velocidade = 2;
    }

}




