using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArmaScript : MonoBehaviour
{
    public void AtualizarPontaArma(EntityModel.Direcao _direcao, float _distanceCenter, float _distanceY) //Atualiza a ponta da arma
    {
        transform.position = FrenteDoPersonagem(_direcao, transform.parent.transform, 0.5f, 1);
    }

    private Vector3 FrenteDoPersonagem(EntityModel.Direcao direcao, Transform _objFather, float _distanceCenter, float _distanceY)
    {
        switch (direcao) //Verifica a direção que o player esta se movimentando, seta a posição do objeto para a posicao do player -/+ uma distancia pre configurada
        {
            case EntityModel.Direcao.Esquerda:
                return new Vector3(_objFather.position.x - _distanceCenter, _objFather.position.y + _distanceY, 0);

            case EntityModel.Direcao.Direita:
                return new Vector3(_objFather.position.x + _distanceCenter, _objFather.position.y + _distanceY, 0);

            case EntityModel.Direcao.Cima:
                return new Vector3(_objFather.position.x, _objFather.position.y + _distanceCenter + _distanceY, 0);

            case EntityModel.Direcao.Baixo:
                return new Vector3(_objFather.position.x, _objFather.position.y - _distanceCenter + _distanceY, 0);

            default:
                Debug.LogWarning("Direcao inexistente, Jesus!");
                return transform.position;
        }
    }
}
