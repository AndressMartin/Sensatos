using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EntityModel
{
    float horizontal;
    float vertical;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    void Move()
    {

    }
}


