using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    private Rigidbody2D rb;
    private Item item;
    public EnemyState enemyState;
    private PontaArma pontaArma;
    public GameObject donoDoProjeto;
    private GameObject alvo;
    Vector3 pontaArmaAoDisparar;

    float horizontal, vertical;
    bool saberDirecaoDisparo;
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
        transform.position = pontaArma.transform.position;
        pontaArmaAoDisparar = pontaArma.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if(disparou)    
        {
            if (!saberDirecaoDisparo)
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
                saberDirecaoDisparo = true;
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
        item = _item;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag != "item" && collision.gameObject.tag != "itemChave")
        {
            HitTarget(collision);
        }
        else
            DestroyGameObject();

    }
 

    void HitTarget(Collider2D _collision)
    {
        alvo = _collision.gameObject;
        if (alvo.tag == "Player" || alvo.tag == "Enemy")
        {
            alvo.GetComponent<EntityModel>().TomarDano(dano);
        }
        DestroyGameObject();

    }
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
