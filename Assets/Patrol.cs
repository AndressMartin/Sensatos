using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public enum Stances { idle, patrolling };
    public Stances stance = Stances.idle;
    public float speed;

    public float waitTime;
    public float startWaitTime;

    public List<Transform> moveSpots = new List<Transform>();
    private int lastMoveSpot;
    private int randomSpot;
    private Animator myAnim;
    private Sprite defaultSprite;
    private SpriteRenderer spriteRend;
    private Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        myAnim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRend.sprite;
        stance = Stances.patrolling;
        randomSpot = Random.Range(0, moveSpots.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

        rigidbody2D.MovePosition(transform.position+(moveSpots[randomSpot].position-transform.position) * speed * Time.deltaTime);


        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            //gera um novo lugar de waypoint
            randomSpot = Random.Range(0, moveSpots.Count);
            if (randomSpot != lastMoveSpot)
            {
                lastMoveSpot = randomSpot;
            }
            else
            {
                while (randomSpot == lastMoveSpot)
                {
                    randomSpot = Random.Range(0, moveSpots.Count);
                }
            }
        }
        else
        {
            stance = Stances.patrolling;
        }

    }
}
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public enum Stances { idle, patrolling };
    public Stances stance = Stances.idle;
    public float speed;

    public float waitTime;
    public float startWaitTime;

    public List<Transform> moveSpots = new List<Transform>();
    private int lastMoveSpot;
    private int randomSpot;
    private Animator myAnim;
    private Sprite defaultSprite;
    private SpriteRenderer spriteRend;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        myAnim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRend.sprite;
        stance = Stances.patrolling;
        randomSpot = Random.Range(0, moveSpots.Count);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveSpots[randomSpot].position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 400*Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            stance = Stances.idle;
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Count);
                if (randomSpot != lastMoveSpot)
                {
                    lastMoveSpot = randomSpot;
                }
                else
                {
                    while(randomSpot == lastMoveSpot)
                    {
                        randomSpot = Random.Range(0, moveSpots.Count);
                    }
                }
                transform.rotation = new Quaternion();
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        else
        {
            stance = Stances.patrolling;
        }
        if (stance == Stances.patrolling)
        {
            myAnim.enabled = true;
        }
        if (stance == Stances.idle)
        {
            myAnim.enabled = false;
            spriteRend.sprite = defaultSprite;
        }
    }
}
*/