using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel : MonoBehaviour
{
    public virtual int vida { get; protected set; }
    public enum Direcao {Baixo, Esquerda, Cima, Direita};
    public Direcao direcao;

    public  Vector3 FrenteDoPersonagem(Transform _objFather, float _distance, float _distanceY)
    {
        switch (direcao)//verifica a direção que o player esta se movimentando, seta a posição do objeto para a posicao do player -/+ uma distancia pre configurada
        {

            case Direcao.Esquerda:
                return new Vector3(_objFather.position.x - _distance, _objFather.position.y + _distanceY, 0);
                
            case Direcao.Direita:
                return new Vector3(_objFather.position.x + _distance, _objFather.position.y + _distanceY, 0);

            case Direcao.Cima:
                return new Vector3(_objFather.position.x, _objFather.position.y + _distance + _distanceY, 0);

            case Direcao.Baixo:
                return new Vector3(_objFather.position.x, _objFather.position.y - _distance + _distanceY, 0);

            default:
                return transform.position;
        }
    }

    public virtual void TomarDano(int _dano, float _horizontal, float _vertical, float _knockBack)
    { }

    public virtual void KnockBack(float _horizontal,float _vertical, float _knockBack)
    { }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
