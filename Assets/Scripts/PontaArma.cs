using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArma : EntityModel
{
    public BoxCollider2D objPai;
    private GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        objPai = GetComponentInParent<BoxCollider2D>();
        obj = objPai.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = FrenteDoPersonagem(obj);
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
    }
}
