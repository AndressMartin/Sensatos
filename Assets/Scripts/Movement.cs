using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : SingletonInstance<Movement>
{
    private State state;
    private Rigidbody2D rb;
    private PontaArma pontaArma;
    float horizontal;
    float vertical;
    public float runSpeed;
    public bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        pontaArma = GetComponentInChildren<PontaArma>();
        state = GetComponent<State>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
        if (canMove)
        {
            if (!state.strafing)
            {

                switch (horizontal)
                {
                    case 1:
                        pontaArma.ChangeDirection("Direita");
                        break;
                    case -1:
                        pontaArma.ChangeDirection("Esquerda");
                        break;
                }

                switch (vertical)
                {
                    case -1:
                        pontaArma.ChangeDirection("Baixo");
                        break;
                    case 1:
                        pontaArma.ChangeDirection("Cima");
                        break;
                }

            }

            Move();
        }
        
    }
    void Move( )
    {
        rb.velocity = new Vector2(horizontal, vertical).normalized * (runSpeed);
    }




   public void UpdateRunSpeed(int velocidade)
    {
        runSpeed = velocidade * 2;
    }

 
}


