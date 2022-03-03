using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisaoCircularEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;

    private bool vendoPlayer;
    public bool VendoPlayer => vendoPlayer;

    private float raioOg;
    private float raioAtual;

    private void Start()
    {
        if (circleCollider2D == null)
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitboxDano"))
        {
            Player player = collision.transform.parent.GetComponent<Player>();
            if (player != null)
            {
                vendoPlayer = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("HitboxDano"))
        {
            Player player = collision.transform.parent.GetComponent<Player>();
            if (player != null)
            {
                vendoPlayer = false;
            }
        }
    }
    public void ValorRaioInicial(float _raio)
    {
        raioOg = _raio;
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = raioOg;

    }
    public void AumentarRaio(float taxaAumentoVisao)
    {
        circleCollider2D.radius = raioOg + taxaAumentoVisao;

    }
}
