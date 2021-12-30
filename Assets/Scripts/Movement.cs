using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : SingletonInstance<Movement>
{
    //Componentes
    private Player player;
    private Rigidbody2D rb;

    //Variaveis
    public float horizontal;
    public float vertical;
    public float runSpeed;
    private float velocityMax,
                  velocityMaxAndando,
                  velocityMaxAndandoSorrateiramente;

    public bool canMove = true;
    private float acelerationSpeed;

    public float knockBackHorizontal,
                 knockBackVertical;
    private float timeKnockBack;
    private float timeKnockBackMax;

    public float _tempX = 0;
    public float _tempY = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Componentes
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        //Variaveis
        velocityMaxAndando = 5;
        velocityMaxAndandoSorrateiramente = 2.5f;
        velocityMax = velocityMaxAndando;

        timeKnockBackMax = 0.5f;

        acelerationSpeed = velocityMaxAndando * 3;
    }

    //Pega os inputs e move o personagem
    public void Mover()
    {
        if (canMove)
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
            velocityMax = velocityMaxAndandoSorrateiramente;
        }
        else
        {
            velocityMax = velocityMaxAndando;
        }

        switch(player.estado)
        {
            case Player.Estado.Normal: //movimentacao padrao, caso esteja sendo empurrado nao fazer contas para se movimentar
                if (horizontal != 0)//se esta andando na vertical guarda em uma variavel a aceleracao e soma no final com addForce
                {
                    _tempX = horizontal * velocityMax;
                    /*
                    if(player.andandoSorrateiramente == false && player.strafing == false)
                    {
                        _tempX += horizontal * (acelerationSpeed) * (Time.deltaTime);
                    }
                    else
                    {
                        _tempX += horizontal * (acelerationSpeed);
                    }

                    if (_tempX >= velocityMax)
                    {
                        _tempX = velocityMax;
                        //rb.velocity = new Vector2(velocityMaxX, rb.velocity.y);
                    }
                    else if (_tempX <= -velocityMax)
                    {
                        _tempX = -velocityMax;
                        //rb.velocity = new Vector2(-velocityMaxX, rb.velocity.y);
                    }
                    */
                }
                else
                {
                    _tempX = 0;
                    /*
                    if (_tempX > 0)
                    {
                        if (player.andandoSorrateiramente == false && player.strafing == false)
                        {
                            _tempX -= (acelerationSpeed) * (Time.deltaTime);
                        }
                        else
                        {
                            _tempX = 0;
                        }

                        if (_tempX < 0)
                        {
                            _tempX = 0;
                            //rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                    }
                    else if (_tempX < 0)
                    {
                        if (player.andandoSorrateiramente == false && player.strafing == false)
                        {
                            _tempX += (acelerationSpeed) * (Time.deltaTime);
                        }
                        else
                        {
                            _tempX = 0;
                        }

                        if (_tempX > 0)
                        {
                            _tempX = 0;
                            //rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                    }
                    */
                }

                if (vertical != 0)
                {
                    _tempY = vertical * velocityMax;
                    /*
                    if (player.andandoSorrateiramente == false && player.strafing == false)
                    {
                        _tempY += vertical * (acelerationSpeed) * (Time.deltaTime);
                    }
                    else
                    {
                        _tempY += vertical * (acelerationSpeed);
                    }

                    if (_tempY >= velocityMax)
                    {
                        _tempY = velocityMax;
                        //rb.velocity = new Vector2(rb.velocity.x, velocityMaxY);
                    }
                    else if (_tempY <= -velocityMax)
                    {
                        _tempY = -velocityMax;
                        //rb.velocity = new Vector2(rb.velocity.x, -velocityMaxY);
                    }
                    */
                }
                else
                {
                    _tempY = 0;
                    /*
                    if (_tempY > 0)
                    {
                        if (player.andandoSorrateiramente == false && player.strafing == false)
                        {
                            _tempY -= (acelerationSpeed) * (Time.deltaTime);
                        }
                        else
                        {
                            _tempY = 0;
                        }

                        if (_tempY < 0)
                        {
                            _tempY = 0;
                            //rb.velocity = new Vector2(rb.velocity.x, 0);
                        }
                    }
                    else if (_tempY < 0)
                    {
                        if (player.andandoSorrateiramente == false && player.strafing == false)
                        {
                            _tempY += (acelerationSpeed) * (Time.deltaTime);
                        }
                        else
                        {
                            _tempY = 0;
                        }

                        if (_tempY > 0)
                        {
                            _tempY = 0;
                            //rb.velocity = new Vector2(rb.velocity.x, 0);
                        }
                    }
                    */
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
        canMove = false;
        timeKnockBack += Time.deltaTime;
        if (timeKnockBack > timeKnockBackMax)
        {
            player.estado = Player.Estado.Normal;
            timeKnockBack = 0;
            canMove = true;
            knockBackHorizontal = 0;
            knockBackVertical = 0;
            player.FinalizarKnockback();
        }
    }

    void Walk(float _horizontal, float _vertical)
    {
        //Debug.Log("Velocidade" + new Vector2(horizontal, vertical).normalized);
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