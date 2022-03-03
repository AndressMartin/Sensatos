using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    //Componentes
    private Enemy enemy;
    private Rigidbody2D rb;
    private AIPath aiPath;
    private IAEnemy ia_Enemy;

    //Variaveis
    [SerializeField] bool rotaIdaVolta;
    private float velocidade;
    [SerializeField] private List<Transform> pontosDeRota = new List<Transform>();

    private float velX;
    private float velY;
    [SerializeField] private float velocidadeModoNormal;
    [SerializeField] private float velocidadeModoLockdown;

    private Vector2 vetorKnockBack;

    bool iniciado = false;

    //Variaveis de controle

    private Vector2 ultimaPosicaoEnquantoFaziaRota;
    private int indiceListaNaoLockdown;
    private int indiceListaLockdown;

    private int indiceUltimoPontoListaNaoLockdown;
    private int indiceUltimoPontoListaLockdown;

    //Contadores
    private float timeKnockBack;
    private float timeKnockBackMax;

    //Getters
    public Vector2 GetUltimaPosicaoOrigem => ultimaPosicaoEnquantoFaziaRota;
    public Vector2 PontosDeRota => pontosDeRota[indiceListaNaoLockdown].position;
    public Vector2 PontoDeProcura => enemy.GeneralManager.EnemyManager.PontosDeProcura[indiceListaLockdown].position;
    public float GetVelocidade => velocidade;
    public float GetVelocidadeModoNormal => velocidadeModoNormal;
    public float GetVelocidadeModoLockdown => velocidadeModoLockdown;

    private void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        ia_Enemy = GetComponent<IAEnemy>();

        ultimaPosicaoEnquantoFaziaRota = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        indiceListaNaoLockdown = 0;
        indiceListaLockdown = 0;
        indiceUltimoPontoListaLockdown = 0;
        indiceUltimoPontoListaNaoLockdown = 0;

        //Variaveis de controle
        timeKnockBack = 0;
        timeKnockBackMax = 0.5f;

        VerificarPontoPatrulhaMaisPerto();
        RotaIdaVolta();

        iniciado = true;
    }

    public void Main()
    {
        if (enemy.GetEstado == Enemy.Estado.TomandoDano)
        {
            KnockBackContador();
            MoverKnockBack();
        }
    }

    private void DefinirDirecao()
    {
        velX = enemy.VetorVelocidade.x;
        velY = enemy.VetorVelocidade.y;


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

    void VerificarPontoPatrulhaMaisPerto()
    {
        int ponto0 = 0;
        float menorPonto = 5;

        for (int i = 0; i < pontosDeRota.Count; i++)
        {
            float distancia = Vector2.Distance(transform.position, pontosDeRota[i].transform.position);
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
        if (rotaIdaVolta)
        {
            int valor = pontosDeRota.Count;
            for (int i = 0; i < valor; i++)
            {
                if (i != valor + 1)
                {
                    pontosDeRota.Add(pontosDeRota[valor - i - 1]);
                }
            }
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
        GeradorDePontos(ref indiceListaNaoLockdown,ref indiceUltimoPontoListaNaoLockdown, pontosDeRota);
    }
    void GerarPontoAleatorio()
    {
        indiceListaLockdown = Random.Range(0, enemy.GeneralManager.EnemyManager.PontosDeProcura.Count);
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

        Mover(enemy.GeneralManager.EnemyManager.PontosDeProcura[indiceListaLockdown].position);
    
        //gera um novo lugar de waypoint
        if (indiceListaLockdown >= enemy.GeneralManager.EnemyManager.PontosDeProcura.Count - 1)
            indiceListaLockdown = 0;
        else
            indiceListaLockdown++;

        indiceListaLockdown = Random.Range(0, enemy.GeneralManager.EnemyManager.PontosDeProcura.Count);//para aleatorizar o proximo destino que ele vai
        if (indiceListaLockdown != indiceUltimoPontoListaLockdown)
        {
                
            indiceUltimoPontoListaLockdown = indiceListaLockdown;
            ultimaPosicaoEnquantoFaziaRota = enemy.GeneralManager.EnemyManager.PontosDeProcura[indiceUltimoPontoListaLockdown].position;
        }
        else
        {
            while (indiceListaLockdown == indiceUltimoPontoListaLockdown)
            {
                   
                indiceListaLockdown = Random.Range(0, enemy.GeneralManager.EnemyManager.PontosDeProcura.Count);
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

    public void Mover(Vector2 _alvo)
    {
        aiPath.enabled = true;
        aiPath.canMove = true;

        if (ia_Enemy.GetEstadoDeteccaoPlayer == IAEnemy.EstadoDeteccaoPlayer.PlayerDetectado)
        {
            velocidade = velocidadeModoLockdown;
        }
        else
        {
            velocidade = velocidadeModoNormal;
        }

        aiPath.maxSpeed = velocidade;

        aiPath.destination = _alvo;

        DefinirDirecao();
    }

    public void MoverKnockBack()
    {
        rb.velocity = vetorKnockBack;
    }

    public void KnockBack(float _knockBack, Vector2 _direcaoKnockBack)
    {
        ZerarVelocidade();
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

    public void ZerarVelocidade()
    {
        rb.velocity = Vector2.zero;
        aiPath.enabled = false;
        aiPath.canMove = false;
        aiPath.maxSpeed = 0;
        aiPath.destination = transform.position;
    }
    public void ResetarVariaveisDeControle()
    {
        ultimaPosicaoEnquantoFaziaRota = Vector3.zero;
        indiceListaNaoLockdown = 0;
        indiceListaLockdown = 0;
        indiceUltimoPontoListaNaoLockdown = 0;
        indiceUltimoPontoListaLockdown = 0;
        velocidade = 2;
    }

}




