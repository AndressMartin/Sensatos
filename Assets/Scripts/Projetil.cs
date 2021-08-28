using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public GameObject objetoQueChamou;
    public EnemyState enemyState;
    public Target alvo;
    private Item item;
    public Vector3 alvoDef;
    private Transform pontaArmaDef;
    private Transform player;
    private PontaArma pontaArma;

    bool teste;
    bool disparou;
    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        pontaArma = transform.parent.GetComponentInChildren<PontaArma>();
        alvo = FindObjectOfType<Target>();
        player = FindObjectOfType<Movement>().GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(disparou)    
        {
            if (!teste)
            {
                pontaArmaDef = pontaArma.transform;
                transform.position = pontaArmaDef.position;
                alvoDef = alvo.transform.position;
                teste = true;
            }
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position,alvoDef,step);
            

        }
    }

    public void Shooted(Item _item)
    {
        disparou = true;
        item = _item;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            HitTarget(collision);
        }
        else if (collision.gameObject.tag != objetoQueChamou.tag)
        {
            Destroy();
        }
        else
            Debug.Log("se atingiu-se");

    }
 

    void HitTarget(Collider2D collision)
    {
        enemyState = collision.gameObject.GetComponent<EnemyState>();
        enemyState.TomarDano(item.dano);
        Destroy();
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
