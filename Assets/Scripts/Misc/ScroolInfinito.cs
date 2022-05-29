using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScroolInfinito : MonoBehaviour
{
    //Componentes
    private BoxCollider2D boxCollider2D;

    //Variaveis
    [SerializeField] private float velocidadeX;
    [SerializeField] private float velocidadeY;

    private float largura, altura;
    private Vector2 posicaoInicial;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        largura = boxCollider2D.size.x;
        altura = boxCollider2D.size.y;

        posicaoInicial = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = (Vector2)transform.position + (new Vector2(velocidadeX, velocidadeY) * Time.deltaTime);

        if(transform.position.x < posicaoInicial.x - largura)
        {
            transform.position += new Vector3(largura, 0);
        }

        if (transform.position.x > posicaoInicial.x + largura)
        {
            transform.position -= new Vector3(largura, 0);
        }

        if (transform.position.y < posicaoInicial.y - altura)
        {
            transform.position += new Vector3(0, altura);
        }

        if (transform.position.y > posicaoInicial.y + altura)
        {
            transform.position -= new Vector3(0, altura);
        }
    }
}
