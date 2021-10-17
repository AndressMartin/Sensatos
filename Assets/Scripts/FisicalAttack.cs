using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisicalAttack : MonoBehaviour
{
    public GameObject FatherFromWeapon;
    public enum Direcao { Esquerda, Cima, Direita, Baixo };
    public Direcao direcao;

    public float widthTemp;
    public float heightTemp;
    public int dano;


    [SerializeField] private float tempoHitboxAtiva;
    private Rigidbody2D rb;
    private Item item;
    private PontaArma pontaArma;
    private GameObject alvo;
    private Vector3 pontaArmaAoDisparar;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private enum Sentido { horizontal, vertical };
    private Sentido sentido;

    private float width;
    private float height;

    

    public float horizontal, vertical;
    bool saberDirecaoDisparo;
    bool disparou;
    float dif;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        pontaArma = transform.parent.GetComponentInChildren<PontaArma>();
        transform.position = pontaArma.transform.position;
        pontaArmaAoDisparar = pontaArma.transform.position;
    }

    private void Update()
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
                if (horizontal != 0)
                {
                    sentido = Sentido.horizontal;
                    width = heightTemp;
                    height = widthTemp;
                    

                }
                else if (vertical != 0)
                {
                    width = widthTemp;
                    height = heightTemp;
                    sentido = Sentido.vertical;

                }



                saberDirecaoDisparo = true;
                boxCollider2D.enabled = saberDirecaoDisparo;
                boxCollider2D.size = new Vector2(width, height);
                Debug.Log(boxCollider2D.size);

                transform.localScale = new Vector2(width, height);
                Debug.Log(transform.localScale);

            }
            AttackRange();

        }
    }
    void AttackRange()
    {


        if (horizontal != 0)
        {
            dif += Time.deltaTime;

            if (Mathf.Abs(dif) >= Mathf.Abs(tempoHitboxAtiva))
            {
                DestroyGameObject();
            }
        }

        else if (vertical != 0)
        {
            dif += Time.deltaTime;

            if (Mathf.Abs(dif) >= Mathf.Abs(tempoHitboxAtiva))
            {
               DestroyGameObject();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        alvo = collision.gameObject;

        if (alvo.tag == "Enemy" || alvo.tag == "Player")
        {
            if (alvo.tag != FatherFromWeapon.tag)
                HitTarget();
        }
        else if (alvo.tag == "porta" || alvo.tag == "cerca")
            DestroyGameObject();
    }

    public void Usou(Item _item)
    {
        disparou = true;
        item = _item;
    }

    void HitTarget()
    {
        alvo.GetComponent<EntityModel>().TomarDano(dano);
        DestroyGameObject();
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
