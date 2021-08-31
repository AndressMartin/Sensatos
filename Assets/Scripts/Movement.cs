using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private State state;
    private Rigidbody2D rb;
    float horizontal;
    float vertical;
    public float runSpeed;
    public enum Direction { Esquerda,Cima, Direita, Baixo };
    public Direction direction;

    // Start is called before the first frame update
    void Start()
    {
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
                case -1:
                    direction = Direction.Esquerda;
                    break;
                case 1:
                    direction = Direction.Direita;
                    break;
            }

            switch (vertical)
            {
                case -1:
                    direction = Direction.Baixo;
                    break;
                case 1:
                    direction = Direction.Cima;
                    break;
            }

            Move();
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


