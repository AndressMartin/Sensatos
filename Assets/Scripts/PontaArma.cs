using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArma : MonoBehaviour
{
    private Movement movement;
    float distanciaObjeto = 0.5F;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponentInParent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (movement.direction)//verifica a direção que o player esta se movimentando, seta a posição do objeto para a posicao do player -/+ uma distancia pre configurada
        {
            case Movement.Direction.Esquerda:
                transform.position = new Vector3(movement.transform.position.x - distanciaObjeto , movement.transform.position.y, 0);
                break;
            case Movement.Direction.Direita:
                transform.position = new Vector3(movement.transform.position.x + distanciaObjeto, movement.transform.position.y, 0);
                break;
            case Movement.Direction.Cima:
                transform.position = new Vector3(movement.transform.position.x , movement.transform.position.y + distanciaObjeto, 0);
                break;
            case Movement.Direction.Baixo:
                transform.position = new Vector3(movement.transform.position.x, movement.transform.position.y - distanciaObjeto, 0);
                break;

        }
      

    }
}
