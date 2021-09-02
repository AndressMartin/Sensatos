using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel : MonoBehaviour
{
    public virtual int vida { get; protected set; }
    public enum Direction { Esquerda, Cima, Direita, Baixo };
    public Direction direction;
    public float distanciaObjeto = 0.5F;

    public  Vector3 FrenteDoPersonagem(GameObject objPaiDaPontaArma)
    {
        switch (direction)//verifica a direção que o player esta se movimentando, seta a posição do objeto para a posicao do player -/+ uma distancia pre configurada
        {

            case Direction.Esquerda:
                return transform.position = new Vector3(objPaiDaPontaArma.transform.position.x - distanciaObjeto, objPaiDaPontaArma.transform.position.y, 0);
                
            case Direction.Direita:
                return transform.position = new Vector3(objPaiDaPontaArma.transform.position.x + distanciaObjeto, objPaiDaPontaArma.transform.position.y, 0);

            case Direction.Cima:
                return transform.position = new Vector3(objPaiDaPontaArma.transform.position.x, objPaiDaPontaArma.transform.position.y + distanciaObjeto, 0);

            case Direction.Baixo:
                return transform.position = new Vector3(objPaiDaPontaArma.transform.position.x, objPaiDaPontaArma.transform.position.y - distanciaObjeto, 0);

            default:
                return transform.position;
        }
    }
}
