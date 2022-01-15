using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisaoCircularEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;

    private bool vendoPlayer;
    public bool VendoPlayer => vendoPlayer;
    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            vendoPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            vendoPlayer = false;
        }
    }
    public void mudarRaio(float _raio)
    {
        circleCollider2D.radius = _raio;
    }
}
