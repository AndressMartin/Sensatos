using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject alvo;
    Vector3 pontaArmaAoDisparar;

    public Vector3 playerVector3;
    public GameObject FatherFromGun;

    Vector3 directionPlayer;
    float horizontal, vertical;
    bool saberDirecaoDisparo;
    bool disparou;
    public float velocidadeProjetil;
    public float distanciaMaxProjetil;
    public int dano;
    public float knockBackValue;

    public enum Direcao { Esquerda, Cima, Direita, Baixo };
    public Direcao direcao;
    private Direcao tempDirecao;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FatherFromGun.GetComponent<Player>())
        {
            FatherPlayer();
        }
        else if (FatherFromGun.GetComponent<Enemy>())
        {
            FatherEnemy();
        }

    }
    void FatherPlayer()
    {
        if (disparou)
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

            rb.velocity = new Vector2(horizontal, vertical) * velocidadeProjetil;
        }
    }
    void FatherEnemy()
    {
        if (disparou)
        {
            if (!saberDirecaoDisparo)
            {
                Vector3 temp = FindObjectOfType<Player>().GetComponent<Transform>().position;
                playerVector3 = new Vector3(temp.x,temp.y,temp.z);
               
                directionPlayer = playerVector3 - transform.position;
                horizontal = directionPlayer.x;
                vertical = directionPlayer.y;
                directionPlayer.Normalize();
                saberDirecaoDisparo = true;
                disparou = false;
            }
                   
        }
        MOVE(directionPlayer);
        
    }
    void MOVE(Vector2 _direction)
    {
        DistanciaProjetil();
        rb.MovePosition((Vector2)transform.position + (_direction * velocidadeProjetil * Time.deltaTime));

    }


    void DistanciaProjetil()
    {
        
            float difX = Mathf.Abs(pontaArmaAoDisparar.x) - Mathf.Abs(transform.position.x);
            //if (Mathf.Abs(difX) >= Mathf.Abs(distanciaMaxProjetil))
            //{
            //   DestroyGameObject();
            //}
        

        
            float difY = Mathf.Abs(pontaArmaAoDisparar.y) - Mathf.Abs(transform.position.y);
            if (Mathf.Abs(difY) >= Mathf.Abs(distanciaMaxProjetil) || Mathf.Abs(difX) >= Mathf.Abs(distanciaMaxProjetil))
            {
                DestroyGameObject();
            }
        
    }

    public void Shooted(Transform _pontaArma)
    {
        disparou = true;
        transform.position = _pontaArma.transform.position;
        pontaArmaAoDisparar = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        alvo = collision.gameObject;

        if (alvo.tag == "Enemy" || alvo.tag == "Player")
        {
            if(alvo.tag != FatherFromGun.tag)
                HitTarget();
        }
        else if(alvo.tag =="porta" || alvo.tag =="cerca")
            DestroyGameObject();

    }
 
 
    void HitTarget()
    {

        EntityModel temp;
        temp = alvo.GetComponent<EntityModel>();
        temp.TomarDano(dano, horizontal, vertical,knockBackValue);
        DestroyGameObject();
        
    }
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
