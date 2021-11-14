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
    [SerializeField] private float velocityMaxX;
    [SerializeField] private float velocityMaxY;

    [SerializeField] private float acelerationSpeed;

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
    }

    //Pega os inputs e move o personagem
    public void Mover()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        if (!state.strafing && canMove)
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

        Move();
        KnockBackContador();

        if(rb.velocity.x > 0)
        {
            player.ChangeDirectionMovement("Direita");
        }
        else if (rb.velocity.x < 0)
        {
            player.ChangeDirectionMovement("Esquerda");
        }

        if (rb.velocity.y > 0)
        {
            player.ChangeDirectionMovement("Cima");
        }
        else if (rb.velocity.y < 0)
        {
            player.ChangeDirectionMovement("Baixo");
        }
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
        float _tempX = 0;
        float _tempY = 0;

        if (!knockBacking)//movimenta��o padr�o, caso esteja sendo empurado n�o fazer contas para se movimentar
        {
            if (horizontal != 0)//se esta andando na vertical guarda em uma variavel a acelera��o e soma no final com addForce
            {

                _tempX = horizontal * (acelerationSpeed) * (Time.deltaTime);
                if (rb.velocity.x >= velocityMaxX)
                {
                    rb.velocity = new Vector2(velocityMaxX, rb.velocity.y);
                }
                else if (rb.velocity.x <= -velocityMaxX)
                {
                    rb.velocity = new Vector2(-velocityMaxX, rb.velocity.y);
                }
            }
            else
            {
                if (rb.velocity.x > 0)
                {

                    _tempX = -1 * (acelerationSpeed) * (Time.deltaTime);

                    if (rb.velocity.x < 0)
                        rb.velocity = new Vector2(0, rb.velocity.y);


                }
                else if (rb.velocity.x < 0)
                {
                    _tempX = 1 * (acelerationSpeed) * (Time.deltaTime);

                    if (rb.velocity.x > 0)
                        rb.velocity = new Vector2(0, rb.velocity.y);
                }
                if (Mathf.Abs(rb.velocity.x) > 0 && Mathf.Abs(rb.velocity.x) < 1)
                {
                    _tempX = 0 * (acelerationSpeed) * (Time.deltaTime);
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }

            if (vertical != 0)
            {

                _tempY = vertical * (acelerationSpeed) * (Time.deltaTime);
                if (rb.velocity.y >= velocityMaxY)
                {
                    rb.velocity = new Vector2(rb.velocity.x, velocityMaxY);
                }
                else if (rb.velocity.y <= -velocityMaxY)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -velocityMaxY);
                }
            }
            else
            {
                if (rb.velocity.y > 0)
                {
                    _tempY = -1 * (acelerationSpeed) * (Time.deltaTime);
                    if (rb.velocity.y < 0)
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                }
                else if (rb.velocity.y < 0)
                {
                    _tempY = 1 * (acelerationSpeed) * (Time.deltaTime);
                    if (rb.velocity.y > 0)
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                }
                if (Mathf.Abs(rb.velocity.y) > 0 && Mathf.Abs(rb.velocity.y) < 1)
                {
                    _tempY = 0 * (acelerationSpeed) * (Time.deltaTime);
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                }
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

            rb.AddForce(new Vector2(_horizontal, _vertical), ForceMode2D.Impulse);
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

    public void UpdateRunSpeed(int _velocidade)
    {
        runSpeed = _velocidade;
        if(runSpeed == 1)
        {
            player.andandoSorrateiramente = true;
        }
        else
        {
            player.andandoSorrateiramente = false;
        }
    }

    void ChageDirection()
    {

    }

 
}


