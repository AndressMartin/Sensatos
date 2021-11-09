using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel : MonoBehaviour
{
    public virtual int vida { get; protected set; }
    public enum Direction { Esquerda, Cima, Direita, Baixo };
    public Direction direction;

    public  Vector3 FrenteDoPersonagem(Transform _objFather, float _distance)
    {
        switch (direction)//verifica a direção que o player esta se movimentando, seta a posição do objeto para a posicao do player -/+ uma distancia pre configurada
        {

            case Direction.Esquerda:
                return transform.position = new Vector3(_objFather.transform.position.x - _distance, _objFather.transform.position.y, 0);
                
            case Direction.Direita:
                return transform.position = new Vector3(_objFather.transform.position.x + _distance, _objFather.transform.position.y, 0);

            case Direction.Cima:
                return transform.position = new Vector3(_objFather.transform.position.x, _objFather.transform.position.y + _distance, 0);

            case Direction.Baixo:
                return transform.position = new Vector3(_objFather.transform.position.x, _objFather.transform.position.y - _distance, 0);

            default:
                return transform.position;
        }
    }

    public virtual void TomarDano(int _dano, float _horizontal, float _vertical, float _knockBack)
    { }

    public virtual void KnockBack(float _horizontal,float _vertical, float _knockBack)
    { }

    public virtual IEnumerator Piscar()
    { yield return new WaitForSeconds(0.1f); }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
