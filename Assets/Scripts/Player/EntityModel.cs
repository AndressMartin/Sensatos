using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel : MonoBehaviour
{
    //Enums
    public enum Direcao { Baixo, Esquerda, Cima, Direita };
    public enum Alvo { Player, Enemy };

    //Variaveis
    protected int vida;
    protected int vidaMax;
    protected Direcao direcao;

    //Getters
    public Direcao GetDirecao => direcao;

    public virtual bool IsMorto()
    {
        return false;
    }

    public virtual void TomarDano(int _dano, float _knockBack, float _knockBackTrigger, Vector2 _direcaoKnockBack)
    { }

    public virtual void KnockBack(float _knockBack, Vector2 _direcaoKnockBack)
    { }

    public Vector2 VetorDirecao(Direcao direcao)
    {
        switch(direcao)
        {
            case Direcao.Baixo:
                return new Vector2(0, -1);

            case Direcao.Esquerda:
                return new Vector2(-1, 0);

            case Direcao.Cima:
                return new Vector2(0, 1);

            case Direcao.Direita:
                return new Vector2(1, 0);

            default:
                return new Vector2(0, -1);
        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
