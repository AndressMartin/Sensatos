using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    private Rigidbody2D rb;
    public EnemyState enemyState;
    private ArmaDeFogo armaDeFogo;
    private Item item;
    private Transform pontaArmaDef;
    private Transform player;
    private PontaArma pontaArma;

    Vector3 pontaArmaAoDisparar;
    float horizontal, vertical;
    bool teste;
    bool disparou;
    public float velocidadeProjetil;
    public float distanciaMaxProjetil;
    public int dano;

    public enum Direcao { Esquerda, Cima, Direita, Baixo };
    public Direcao direcao;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pontaArma = transform.parent.GetComponentInChildren<PontaArma>();
        player = FindObjectOfType<Movement>().GetComponent<Transform>();

        pontaArmaDef = pontaArma.transform;
        transform.position = pontaArmaDef.position;
        pontaArmaAoDisparar = pontaArmaDef.position;
    }

    // Update is called once per frame
    void Update()
    {

        if(disparou)    
        {
            if (!teste)
            {
                switch (direcao)
                {
                    case Direcao.Esquerda:
                        horizontal = -1;
                    break;
                    case Direcao.Direita:
                        horizontal = 1;
                        break;
                    case Direcao.Cima:

                        vertical = 1;
                        break;
                    case Direcao.Baixo:
                        vertical = -1;
                        break;

                }
                teste = true;
            }
            DistanciaProjetil();

            rb.velocity = new Vector2(horizontal, vertical).normalized * velocidadeProjetil;

        }
    }

    void DistanciaProjetil()
    {
        if(horizontal != 0)
        {
            float dif = Mathf.Abs(pontaArmaAoDisparar.x) - Mathf.Abs(transform.position.x);
            if (Mathf.Abs(dif) >= Mathf.Abs(distanciaMaxProjetil))
            {
                DestroyGameObject();
            }
        }

        else if(vertical !=0)
        {
            float dif = Mathf.Abs(pontaArmaAoDisparar.y) - Mathf.Abs(transform.position.y);
            if (Mathf.Abs(dif) >= Mathf.Abs(distanciaMaxProjetil))
            {
                DestroyGameObject();
            }
        }
    }

    public void Shooted(Item _item)
    {
        disparou = true;
        armaDeFogo = (ArmaDeFogo)_item;
        item = _item;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            HitTarget(collision);
        }
        else
            DestroyGameObject();

    }
 

    void HitTarget(Collider2D collision)
    {
        enemyState = collision.gameObject.GetComponent<EnemyState>();
        enemyState.TomarDano(dano);
        DestroyGameObject();
    }
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
