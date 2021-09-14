using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Player player;
    private State state;
    private Rigidbody2D rb;
    private PontaArma pontaArma;
    float horizontal;
    float vertical;
    public float runSpeed;

   

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

        if (!state.strafing)
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
        rb.velocity = new Vector2(horizontal, vertical).normalized * (runSpeed);
    }




   public void UpdateRunSpeed(int velocidade)
    {
        runSpeed = velocidade * 2;
    }

 
}


