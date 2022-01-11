using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel : MonoBehaviour
{
    public virtual int vida { get; protected set; }
    public enum Direcao {Baixo, Esquerda, Cima, Direita};
    public Direcao direcao;

    public virtual void TomarDano(int _dano, float _horizontal, float _vertical, float _knockBack)
    { }

    public virtual void KnockBack(float _horizontal,float _vertical, float _knockBack)
    { }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
