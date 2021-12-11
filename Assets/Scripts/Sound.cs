using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    //Managers
    private ObjectManagerScript objectManager;

    //Componentes
    CircleCollider2D circleCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();

        //Componentes
        circleCollider2D = GetComponent<CircleCollider2D>();

        circleCollider2D.enabled = false;
    }

    public void AtualizarHitBox(float raio)
    {
        circleCollider2D.radius = raio;
    }

    public void GerarSom(Player player, float raio, bool somTiro)
    {
        AtualizarHitBox(raio);
        ProcurarInimigos(player, somTiro);
    }

    private void ProcurarInimigos(Player player, bool somTiro)
    {
        circleCollider2D.enabled = true;
        foreach (Enemy inimigo in objectManager.listaInimigos)
        {
            if (Colisao.HitTest(inimigo.transform.position.x, inimigo.transform.position.y, circleCollider2D))
            {
                InimigoEscutarSom(player, inimigo, somTiro);
            }
        }
        circleCollider2D.enabled = false;
    }

    private void InimigoEscutarSom(Player player, Enemy inimigo, bool somTiro)
    {
        inimigo.EscutarSom(player, somTiro);
    }
}
