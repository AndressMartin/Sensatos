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

    private float velocity,
                  velocityAndando,
                  velocityAndandoSorrateiramente;

    private float knockBackHorizontal,
                 knockBackVertical;

    private float timeKnockBack;
    private float timeKnockBackMax;

    private float _tempX = 0;
    private float _tempY = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Componentes
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        //Variaveis
        velocityAndando = 5;
        velocityAndandoSorrateiramente = 2.5f;
        velocity = velocityAndando;

        timeKnockBackMax = 0.5f;

    }

    //Pega os inputs e move o personagem
    public void Mover()
    {
        if (player.estado == Player.Estado.Normal)
        {
            if(player.modoMovimento != Player.ModoMovimento.Strafing)
            {
                switch (horizontal)
                {
                    case 1:
                        player.ChangeDirection("Direita");
                        break;
                    case -1:
                        player.ChangeDirection("Esquerda");
                        break;
                }

                switch (vertical)
                {
                    case -1:
                        player.ChangeDirection("Baixo");
                        break;
                    case 1:
                        player.ChangeDirection("Cima");
                        break;
                }
            }

            switch (horizontal)
            {
                case 1:
                    player.ChangeDirectionMovement("Direita");
                    break;
                case -1:
                    player.ChangeDirectionMovement("Esquerda");
                    break;
            }

            switch (vertical)
            {
                case -1:
                    player.ChangeDirectionMovement("Baixo");
                    break;
                case 1:
                    player.ChangeDirectionMovement("Cima");
                    break;
            }
        }

        Move();
        if(player.estado == Player.Estado.TomandoDano)
        {
            KnockBackContador();
        }
    }

    public void KnockBack(float _horizontal, float _vertical, float _knockBack)
    {
        knockBackHorizontal = _horizontal * _knockBack;
        knockBackVertical = _vertical * _knockBack;

        timeKnockBack = 0;
    }

    void Move()
    {
        if(player.modoMovimento == Player.ModoMovimento.AndandoSorrateiramente)
        {
            velocity = velocityAndandoSorrateiramente;
        }
        else
        {
            velocity = velocityAndando;
        }

        switch(player.estado)
        {
            case Player.Estado.Normal: //movimentacao padrao, caso esteja sendo empurrado nao fazer contas para se movimentar
                if (horizontal != 0)//se esta andando na vertical guarda em uma variavel a aceleracao e soma no final com addForce
                {
                    _tempX = horizontal * velocity;                   
                }
                else
                {
                    _tempX = 0;
                }

                if (vertical != 0)
                {
                    _tempY = vertical * velocity;
                }
                else
                {
                    _tempY = 0;
                }
                break;

            case Player.Estado.TomandoDano:
                _tempX = knockBackHorizontal;
                _tempY = knockBackVertical;
                break;

            case Player.Estado.Atacando:
                _tempX = 0;
                _tempY = 0;
                break;

            case Player.Estado.UsandoItem:
                _tempX = 0;
                _tempY = 0;
                break;
        }

        Walk(_tempX, _tempY);
    }

    void KnockBackContador()
    {
        timeKnockBack += Time.deltaTime;
        if (timeKnockBack > timeKnockBackMax)
        {
            player.estado = Player.Estado.Normal;
            timeKnockBack = 0;
            knockBackHorizontal = 0;
            knockBackVertical = 0;
            player.FinalizarKnockback();
        }
    }

    void Walk(float _horizontal, float _vertical)
    {
        if (!(horizontal != 0 && vertical != 0))
        {
            rb.velocity = new Vector2(_tempX, _tempY);
        }
        else
        {
            rb.velocity = new Vector2(_tempX * 0.7f, _tempY * 0.7f);
        }

        if(!(_tempX == 0 && _tempY == 0))
        {
            if (player.estado == Player.Estado.Normal && player.modoMovimento != Player.ModoMovimento.AndandoSorrateiramente)
            {
                player.GerarSom(0f, false);
            }
        }
    }

    public void ZerarVelocidade()
    {
        rb.velocity = new Vector2(0, 0);
    }
}