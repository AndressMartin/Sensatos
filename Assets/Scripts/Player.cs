using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityModel
{
    [SerializeField]public override int vida { get; protected set; }
    private DirectionHitbox directionHitbox;
    private ItemDirectionHitbox itemDirectionHitbox;
    public int Pv;
    // Start is called before the first frame update
    void Start()
    {
        directionHitbox = GetComponentInChildren<DirectionHitbox>();
        itemDirectionHitbox = GetComponentInChildren<ItemDirectionHitbox>();
        vida = Pv;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void curar(int _cura)
    {
        Debug.Log(vida);
        vida = +_cura;
        Debug.Log(vida);
    }

    public override void TomarDano(int _dano)
    {
        Debug.Log("tomei tiro, player");

    }
    public void ChangeDirection(string lado)
    {
        switch (lado)
        {
            case "Esquerda":
                direction = Direction.Esquerda;
                break;
            case "Direita":
                direction = Direction.Direita;
                break;
            case "Cima":
                direction = Direction.Cima;
                break;
            case "Baixo":
                direction = Direction.Baixo;
                break;
        }
        directionHitbox.ChangeDirection(direction);
        itemDirectionHitbox.ChangeDirection(direction);
    }

}
