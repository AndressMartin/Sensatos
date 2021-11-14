using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : SingletonInstance<Movement>
{
    private Player player;
    private State state;
    private Rigidbody2D rb;
    float horizontal;
    float vertical;
    public float runSpeed;
    public bool canMove = true;
    private float velocityMax,
                  velocityMaxAndando,
                  velocityMaxAndandoSorrateiramente;

    private float acelerationSpeed;

    public float _tempX = 0;
    public float _tempY = 0;
    bool knockBacking;
    public float knockBackHorizontal, knockBackVertical;
    public bool Knock;
    float time, timeMax;
    [SerializeField]private float timeMaxOriginal;
    Sound sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponentInChildren<Sound>();
        player = GetComponent<Player>();
        state = GetComponent<State>();
        rb = GetComponent<Rigidbody2D>();

        velocityMaxAndando = 5;
        velocityMaxAndandoSorrateiramente = 2.5f;
        velocityMax = velocityMaxAndando;

        acelerationSpeed = velocityMaxAndando * 3;
    }

    //Pega os inputs e move o personagem
    public void Mover()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        if (canMove)
        {
            if(!player.strafing)
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
        KnockBackContador();
    }
    public void KnockBack(float _horizontal, float _vertical, float _knockBack)
    {
        knockBacking = true; 
        knockBackHorizontal = _horizontal;
        knockBackVertical = _vertical;

        Knock = true;
        timeMax = timeMaxOriginal;
        time = 0;
    }


    void Move()
    {
        if(player.andandoSorrateiramente == true)
        {
            velocityMax = velocityMaxAndandoSorrateiramente;
        }
        else
        {
            velocityMax = velocityMaxAndando;
        }

        if (!knockBacking)//movimenta��o padr�o, caso esteja sendo empurado n�o fazer contas para se movimentar
        {
            if (horizontal != 0)//se esta andando na vertical guarda em uma variavel a acelera��o e soma no final com addForce
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
            //rb.AddForce(new Vector2(_tempX, _tempY), ForceMode2D.Impulse);

        }

        else
        {
            _tempX = knockBackHorizontal;
            _tempY = knockBackVertical;
            //abc();
        }

        walk(_tempX, _tempY);
        //rb.velocity = new Vector2(_tempX, _tempY).normalized;
    }
    void KnockBackContador()
    {
        if (Knock)
        {
            time += Time.deltaTime;
            if (time > timeMax)
            {
                Knock = false;
                timeMax = 0.0F;
                time = 0;
            }
        }
        else
        {
            knockBackHorizontal = 0;
            knockBackVertical = 0;
            knockBacking = false;
        }
    }


    void walk(float _horizontal, float _vertical)
    {

        if (!Knock)
        {
            //Debug.Log("Velocidade" + new Vector2(horizontal, vertical).normalized);
            if(!(horizontal != 0 && vertical != 0))
            {
                rb.velocity = new Vector2(_tempX, _tempY);
            }
            else
            {
                rb.velocity = new Vector2(_tempX * 0.7f, _tempY * 0.7f);
            }
            //rb.AddForce(new Vector2(_horizontal, _vertical), ForceMode2D.Impulse);
            sound.changeColliderRadius(runSpeed);
            
        }

        else
            rb.AddForce(new Vector2(_horizontal, _vertical));
    }


        void abc()
    {
        transform.position = new Vector3(transform.position.x + knockBackHorizontal, transform.position.y + knockBackVertical, transform.position.z);
        rb.AddForce(new Vector2(knockBackHorizontal, knockBackVertical), ForceMode2D.Impulse);
    }

    void ChageDirection()
    {

    }

 
}