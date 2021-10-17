using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : SingletonInstance<Movement>
{
    private Player player;
    private State state;
    private Rigidbody2D rb;
    private PontaArma pontaArma;
    float horizontal;
    float vertical;
    float _horizontal;
    float _vertical;
    public float runSpeed;
    public bool canMove = true;
    [SerializeField] private float velocityMaxX;
    [SerializeField] private float velocityMaxY;



    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        pontaArma = GetComponentInChildren<PontaArma>();
        state = GetComponent<State>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
            pontaArma.direction = player.direction;
        }

        Move();

    }
    
    void Move( )
    {
        if (horizontal != 0)
        {
            rb.AddForce(new Vector2(horizontal, 0).normalized * (runSpeed));
            if(rb.velocity.x >= velocityMaxX)
            {
                rb.velocity = new Vector2(velocityMaxX,rb.velocity.y);
            }
            else if(rb.velocity.x <= -velocityMaxX)
            {
                rb.velocity = new Vector2(-velocityMaxX, rb.velocity.y);
            }
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) >= 0.1)
            {
               
                rb.velocity = new Vector2(rb.velocity.x * 0.99F, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }          
        }

        if (vertical != 0)
        {
            rb.AddForce(new Vector2(0, vertical).normalized * (runSpeed));
            if (rb.velocity.y >= velocityMaxY)
            {
                rb.velocity = new Vector2(rb.velocity.x, velocityMaxX);
            }
            else if (rb.velocity.y <= -velocityMaxX)
            {
                rb.velocity = new Vector2(rb.velocity.x, -velocityMaxX);
            }
        }
        else
        {
            if (Mathf.Abs(rb.velocity.y) >= 0.1)
            {

                rb.velocity = new Vector2(rb.velocity.x , rb.velocity.y * 0.99F);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }




        /* if (vertical != 0)
         {
             if (Mathf.Abs(rb.velocity.y) <= velocityMaxY)
             {
                 if (vertical == 1)
                 {
                     if (_vertical != vertical)
                     {
                         rb.velocity = new Vector2(rb.velocity.x, 0);
                         _vertical = vertical;
                     }

                     else
                     {
                         rb.AddForce(new Vector2(0, Mathf.Abs(vertical)).normalized * (runSpeed));
                     }
                 }

                 else if (vertical == -1)
                 {
                     if (_vertical != vertical)
                     {
                         rb.velocity = new Vector2(rb.velocity.x, 0);
                         _vertical = vertical;

                     }
                     else
                     {
                         rb.AddForce(new Vector2(0, -Mathf.Abs(vertical)).normalized * (runSpeed));
                     }
                 }
             }
         }
         else
         {
             rb.velocity = new Vector2(rb.velocity.x, 0);
         }*/


        //rb.AddForce(new Vector2(horizontal, 0).normalized * (runSpeed));
        //Debug.Log((horizontal) * (runSpeed));
        //rb.velocity = new Vector2(horizontal, vertical).normalized * (runSpeed);
    }




   public void UpdateRunSpeed(int _velocidade)
    {
        runSpeed = _velocidade * 2;
    }

    void ChageDirection()
    {

    }

 
}


