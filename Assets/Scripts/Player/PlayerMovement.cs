using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Componentes
    private Player player;
    private Rigidbody2D rb;

    //Variaveis
    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;

    private float velocidade,
                  velocidadeAndando,
                  velocidadeAndandoSorrateiramente;

    private float movimentoX = 0;
    private float movimentoY = 0;
    private Vector2 vetorKnockBack;

    //Variaveis de controle
    private float timeKnockBack;
    private float timeKnockBackMax;

    // Start is called before the first frame update
    void Start()
    {
        //Componentes
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        //Variaveis
        velocidadeAndando = 5;
        velocidadeAndandoSorrateiramente = 3f;
        velocidade = velocidadeAndando;

        //Variaveis de controle
        timeKnockBack = 0;
        timeKnockBackMax = 0.5f;

    }

    public void ResetarVariaveisDeControle()
    {
        timeKnockBack = 0;
        vetorKnockBack = Vector2.zero;
        ZerarVelocidade();
    }

    //Pega os inputs e move o personagem
    public void Mover()
    {
        if (player.GetEstado == Player.Estado.Normal)
        {
            if(player.modoMovimento != Player.ModoMovimento.Strafing)
            {
                switch (horizontal)
                {
                    case 1:
                        player.ChangeDirection(EntityModel.Direcao.Direita);
                        break;
                    case -1:
                        player.ChangeDirection(EntityModel.Direcao.Esquerda);
                        break;
                }

                switch (vertical)
                {
                    case -1:
                        player.ChangeDirection(EntityModel.Direcao.Baixo);
                        break;
                    case 1:
                        player.ChangeDirection(EntityModel.Direcao.Cima);
                        break;
                }
            }

            switch (horizontal)
            {
                case 1:
                    player.ChangeDirectionMovement(EntityModel.Direcao.Direita);
                    break;
                case -1:
                    player.ChangeDirectionMovement(EntityModel.Direcao.Esquerda);
                    break;
            }

            switch (vertical)
            {
                case -1:
                    player.ChangeDirectionMovement(EntityModel.Direcao.Baixo);
                    break;
                case 1:
                    player.ChangeDirectionMovement(EntityModel.Direcao.Cima);
                    break;
            }
        }

        DefinirMovimento();

        if(player.GetEstado == Player.Estado.TomandoDano)
        {
            KnockBackContador();
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
            player.FinalizarAnimacao();
        }
    }

    void DefinirMovimento()
    {
        if(player.modoMovimento == Player.ModoMovimento.AndandoSorrateiramente)
        {
            velocidade = velocidadeAndandoSorrateiramente;
        }
        else
        {
            velocidade = velocidadeAndando;
        }

        if(player.GetEstado == Player.Estado.Normal)
        {
            if (horizontal != 0)
            {
                movimentoX = horizontal * velocidade;
            }
            else
            {
                movimentoX = 0;
            }

            if (vertical != 0)
            {
                movimentoY = vertical * velocidade;
            }
            else
            {
                movimentoY = 0;
            }
        }

        MoverObjeto(movimentoX, movimentoY);
    }

    void MoverObjeto(float _horizontal, float _vertical)
    {
        switch(player.GetEstado)
        {
            case Player.Estado.Normal:
                if (horizontal == 0 || vertical == 0)
                {
                    rb.velocity = new Vector2(movimentoX, movimentoY);
                }
                else
                {
                    rb.velocity = new Vector2(movimentoX * 0.7f, movimentoY * 0.7f);
                }

                if (!(movimentoX == 0 && movimentoY == 0))
                {
                    if (player.modoMovimento != Player.ModoMovimento.AndandoSorrateiramente)
                    {
                        player.GerarSom(0f, false);
                    }
                }
                break;

            case Player.Estado.TomandoDano:
                rb.velocity = vetorKnockBack;
                break;

            default:
                ZerarVelocidade();
                break;
        }
    }

    public void ZerarVelocidade()
    {
        rb.velocity = new Vector2(0, 0);
    }
}