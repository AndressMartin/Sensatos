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
    private Transform player;
    public PontaArma pontaArma;

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
                alvoDef = alvo.transform.position;
                teste = true;
            }
            float step = speed * Time.deltaTime;        
            //transform.position = Vector3.MoveTowards(pontaArma.transform.position,alvoDef,step);
            transform.position = Vector3.MoveTowards(alvo.transform.position,alvoDef,step); //correto porem esta colidindo com a propria hitbox do player, solução fazer um hitbox externa do player de onde os projeteis irão sair
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
        //Destroy(gameObject);
    }
}
