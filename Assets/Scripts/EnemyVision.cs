using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : EntityModel
{
    public bool playerOnAttackRange;

    private EnemyMove enemyMove;
    public PolygonCollider2D polygonCollider;
    private CircleCollider2D circleCollider2D;
    [SerializeField] private float xPointOrigin, yPointOrigin;
    [SerializeField] private float xPointMax, yPointMax;
    GameObject playerGameObject;
    Transform transformFather;
    Enemy enemyModel;
    public bool seePlayer;
    [SerializeField]private float xp, yp;
    Vector2 v1, v2 = new Vector2(0, 0), v3 = new Vector2(0, 0);
    public EntityModel entityModelTemp;
    private SpriteRenderer spriteRenderer;
    public Vector3 wallDistance, playerDistance;
    private Vector3 lastPlayerPosition;

    

    // Start is called before the first frame update
    void Start()
    {
        lastPlayerPosition = transform.position;
        circleCollider2D = GetComponent<CircleCollider2D>();
        transformFather = GetComponentInParent<Transform>();
        enemyMove = GetComponentInParent<EnemyMove>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        enemyModel = GetComponentInParent<Enemy>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        enemyMove.EnemyVissionReference(this);
        wallDistance = transform.position;
        v1 = new Vector2(xPointOrigin, yPointOrigin);
    }

    private void Update()
    {

        xp = transformFather.position.x;
        yp = transformFather.position.y;
        direction = enemyModel.direction;
        if (seePlayer)
        {
            spriteRenderer.color = Color.black;
            polygonCollider.enabled = false;
            circleCollider2D.enabled = true;

        }
        else
        {
            spriteRenderer.color = Color.white;
            circleCollider2D.enabled = false;
            polygonCollider.enabled = true;

        }

        switch (direction)
        {
            //rodar plano cartesiano
            case Direction.Esquerda:
                v2 = new Vector2(-(xPointMax - xPointOrigin), (yPointMax + yPointOrigin));
                v3 = new Vector2(-(xPointMax - xPointOrigin), -(yPointMax - yPointOrigin));

                break;

            case Direction.Direita:
                v2 = new Vector2((xPointMax + xPointOrigin), (yPointMax + yPointOrigin));
                v3 = new Vector2((xPointMax + xPointOrigin), -(yPointMax - yPointOrigin));
                break;

            case Direction.Cima:
                v2 = new Vector2(-( yPointMax - xPointOrigin), (xPointMax + yPointOrigin));
                v3 = new Vector2(+( yPointMax + xPointOrigin), (xPointMax + yPointOrigin));
                break;
            
            case Direction.Baixo:
                v2 = new Vector2((yPointMax + xPointOrigin), -(xPointMax - yPointOrigin));
                v3 = new Vector2(-(yPointMax - xPointOrigin), -(xPointMax - yPointOrigin));
                break;
 
        }
        float h2 = (xPointOrigin - xPointMax) * (xPointOrigin - xPointMax) + (yPointOrigin - yPointMax) * (yPointOrigin - yPointMax);
        circleCollider2D.radius = Mathf.Sqrt(h2);
        polygonCollider.points = new[] { v1, v2, v3 };

        if (seePlayer && playerGameObject != null && polygonCollider.enabled == true)//flip do cone de vis�o
        {
            BoxCollider2D playerBoxCollider2D = playerGameObject.GetComponent<BoxCollider2D>();
            BoxCollider2D thisBoxCollider2D = GetComponentInParent<BoxCollider2D>();
            //Debug.Log(playerGameObject.transform.position.y + playerBoxCollider2D.bounds.extents.y);
            //Debug.Log(playerBoxCollider2D.bounds.extents.y);

            if (transform.position.y > playerGameObject.transform.position.y )//if do entre o y horizontal
            {
                Debug.Log("ta entre");
                if (playerGameObject.transform.position.x > transform.position.x)
                {
                    enemyModel.ChangeDirection(Direction.Direita);
                }

                else
                {
                    enemyModel.ChangeDirection(Direction.Esquerda);
                }
            }

           /* if (playerGameObject.transform.position.y > transform.position.y)

                enemyModel.ChangeDirection(Direction.Cima);

            else
                enemyModel.ChangeDirection(Direction.Baixo);*/



        }
    }
    void ChangeCollision(bool _poligon)
    {
        polygonCollider.enabled = _poligon;
        circleCollider2D.enabled = !_poligon;
        if (circleCollider2D.enabled == true)
            seePlayer = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {//Debug.DrawRay(new Vector3(xp, yp, 0), new Vector3(cerca.transform.localPosition.x - xp, cerca.transform.localPosition.y - yp, cerca.transform.localPosition.z), Color.red);


        if (polygonCollider.enabled && !seePlayer)
        {
            PollygonCollider(collision);
        }
        if(circleCollider2D.enabled)
        {
            CircleCollider(collision);
        }
    }
    void PollygonCollider(Collider2D collision)
    {
        entityModelTemp = collision.gameObject.GetComponent<Player>();
        if (entityModelTemp != null)
        {
            //Debug.Log(entityModelTemp.transform.name);

            if (!playerOnAttackRange)
            {

                if (false)
                {
                }
                else
                {
                    wallDistance = Vector3.zero;
                    RaycastHit2D[] hits;
                    hits = Physics2D.RaycastAll(new Vector2(xp, yp), new Vector2(entityModelTemp.transform.position.x - xp, entityModelTemp.transform.position.y - yp));
                    //Debug.Log(hits.Length);
                    foreach (RaycastHit2D hit in hits)
                    {
                        //Debug.Log(hit.transform.name);
                        Debug.DrawRay(new Vector3(xp, yp, 0), new Vector3(hit.transform.localPosition.x - xp, hit.transform.localPosition.y - yp, hit.transform.localPosition.z), Color.red);

                        if (hit.transform.GetComponent<ParedeModel>() != null)
                        {
                            wallDistance = hit.transform.position;
                        }

                        //qunado ve o player

                        if (hit.transform.GetComponent<Player>() != null)
                        {
                            playerDistance = hit.transform.position;

                        }

                        if (wallDistance != Vector3.zero)
                        {
                            if (Vector2.Distance(transform.position, playerDistance) <= Vector2.Distance(transform.position, wallDistance))//caso o player esteja mais perto do inimigo que a parede
                            {
                                //Debug.Log("player mais perto q a parede");
                                wallDistance = Vector3.zero;
                                SeePlayer(true, collision.gameObject);
                            }
                            else
                            {
                                //Debug.Log("player mais longe q a parede");
                                SeePlayer(false, null);
                            }
                        }
                        else
                        {
                            //Debug.Log("Sem parede no caminho to vendo o player");//caso o player esteja mais perto do inimigo que a parede(caso nem tenha parede)
                            SeePlayer(true, collision.gameObject);
                        }



                    }
                }
            }
        }
    }

    void CircleCollider(Collider2D collision)
    {
        PollygonCollider(collision);
        //SeePlayer(seePlayer, playerGameObject);
    }

    void SeePlayer(bool _player, GameObject _gameObject)
    {
        seePlayer = _player;
        playerGameObject = _gameObject;
        enemyMove.SawEnemy(playerGameObject);
    }
    
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && polygonCollider.enabled == true)
        {
            wallDistance = Vector3.zero;
           // seePlayer = false;
            enemyMove.SawEnemy(null);
        }

        else if (collision.gameObject.tag == "Player" && circleCollider2D.enabled==true)
        {
            wallDistance = Vector3.zero;
            enemyMove.SawEnemy(null);
            lastPlayerPosition = collision.transform.position;
            ZerarVariaveis();
        }

    }
    
    void FollowEnemyEyes()
    {

    }
    public void OnAttackRange(bool _playerVisible,GameObject _playerGameObject)
    {
        playerOnAttackRange = _playerVisible;

        if (playerOnAttackRange) { SeePlayer(seePlayer, _playerGameObject); ChangeCollision(false); }
    }

    void ZerarVariaveis()
    {
        polygonCollider.enabled = true;
        circleCollider2D.enabled = false;
        playerOnAttackRange = false;
        playerGameObject = null;
        seePlayer = false;
        entityModelTemp = null;
    }
}