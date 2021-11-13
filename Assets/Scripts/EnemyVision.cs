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
    private SpriteRenderer spriteRenderer;
    public Vector3 wallDistance, playerDistance;
    public List<GameObject> walls;
    public List<float> wallsDistance;
    public EntityModel entityModelTemp;
    public HitboxTile tempPlayer;
    public float difDistance;
    private Vector3 lastPlayerPosition;
    public List<ParedeModel> paredeModels;


    // Start is called before the first frame update
    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        transformFather = GetComponentInParent<Transform>();
        enemyMove = GetComponentInParent<EnemyMove>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        enemyModel = GetComponentInParent<Enemy>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        enemyMove.EnemyVissionReference(this);
        wallDistance = transform.position;
        v1 = new Vector2(xPointOrigin, yPointOrigin);
        tempPlayer = FindObjectOfType<HitboxTile>();


    }

    public void Main()
    {
        distanceWallsZero();
        distanceWalls();
        difDistance = Vector2.Distance(transform.position, tempPlayer.transform.position);

        xp = transformFather.position.x;
        yp = transformFather.position.y;
        direcao = enemyModel.direcao;
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

        switch (direcao)
        {
            //rodar plano cartesiano
            case Direcao.Esquerda:
                v2 = new Vector2(-(xPointMax - xPointOrigin), (yPointMax + yPointOrigin));
                v3 = new Vector2(-(xPointMax - xPointOrigin), -(yPointMax - yPointOrigin));

                break;

            case Direcao.Direita:
                v2 = new Vector2((xPointMax + xPointOrigin), (yPointMax + yPointOrigin));
                v3 = new Vector2((xPointMax + xPointOrigin), -(yPointMax - yPointOrigin));
                break;

            case Direcao.Cima:
                v2 = new Vector2(-( yPointMax - xPointOrigin), (xPointMax + yPointOrigin));
                v3 = new Vector2(+( yPointMax + xPointOrigin), (xPointMax + yPointOrigin));
                break;
            
            case Direcao.Baixo:
                v2 = new Vector2((yPointMax + xPointOrigin), -(xPointMax - yPointOrigin));
                v3 = new Vector2(-(yPointMax - xPointOrigin), -(xPointMax - yPointOrigin));
                break;
 
        }
        float h2 = (xPointOrigin - xPointMax) * (xPointOrigin - xPointMax) + (yPointOrigin - yPointMax) * (yPointOrigin - yPointMax);
        circleCollider2D.radius = Mathf.Sqrt(h2);
        polygonCollider.points = new[] { v1, v2, v3 };

        if (seePlayer && playerGameObject != null && polygonCollider.enabled == true)//flip do cone de visão
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
                    enemyModel.ChangeDirection(Direcao.Direita);
                }

                else
                {
                    enemyModel.ChangeDirection(Direcao.Esquerda);
                }
            }

           /* if (playerGameObject.transform.position.y > transform.position.y)

                enemyModel.ChangeDirection(Direction.Cima);

            else
                enemyModel.ChangeDirection(Direction.Baixo);*/



        }
    }
    void distanceWalls()
    {
        foreach (ParedeModel item in paredeModels)
        {
            wallsDistance.Add(Vector2.Distance(item.transform.position,transform.position));        
            
        }
    }
    void distanceWallsZero()
    {
        wallsDistance.Clear();
    }
    void ChangeCollision(bool _poligon)
    {
        if (!enemyModel.dead)
        {
            polygonCollider.enabled = _poligon;
            circleCollider2D.enabled = !_poligon;
            if (circleCollider2D.enabled == true)
                seePlayer = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {//Debug.DrawRay(new Vector3(xp, yp, 0), new Vector3(cerca.transform.localPosition.x - xp, cerca.transform.localPosition.y - yp, cerca.transform.localPosition.z), Color.red);

        if (!enemyModel.dead)
        {
            if (polygonCollider.enabled && !seePlayer)
            {
                PollygonCollider(collision);
            }
            if (circleCollider2D.enabled)
            {
                CircleCollider(collision);
            }
        }
    }
    void PollygonCollider(Collider2D collision)
    {
        Enemy entityTemp = collision.gameObject.GetComponent<Enemy>();
        if (entityTemp != null)
        {
            if(entityTemp.dead)
            {
                //Debug.Log("To vendo um corpo");
                //deteceta um corpo de inimigo no chao
            }
        }

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
                    foreach (float item in wallsDistance)
                    {
                        if(item < difDistance)
                        {
                            Debug.Log("Estou mais perto que o player");
                        }
                        if (item > difDistance)
                        {
                            Debug.Log("Estou mais longe que o player");
                            wallDistance = Vector3.zero;
                            SeePlayer(true, collision.gameObject);
                        }
                    }

                
                    /*wallDistance = Vector3.zero;
                    RaycastHit2D[] hits;
                    hits = Physics2D.RaycastAll(new Vector2(xp, yp), new Vector2(entityModelTemp.transform.position.x - xp, entityModelTemp.transform.position.y - yp));
                    //Debug.Log(hits.Length);
                    foreach (RaycastHit2D hit in hits)
                    {
                        //Debug.Log(hit.transform.name);
                        Debug.DrawRay(new Vector3(xp, yp, 0), new Vector3(hit.transform.localPosition.x - xp, hit.transform.localPosition.y - yp, hit.transform.localPosition.z), Color.red);

                        if (hit.transform.GetComponent<ParedeModel>() != null)
                        {
                            if(!walls.Contains(hit.transform.gameObject))
                            {
                                walls.Add(hit.transform.gameObject);// =new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
                            }
                            
                        }

                        if (hit.transform.GetComponent<EntityModel>() != null)
                        {
                            Debug.Log("entrou aqui");
                            // =new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z); 
                        }
                        foreach (ParedeModel item in paredeModels)
                        {
                            if(difDistance<item.distnacia)
                            {
                                //Debug.Log("player perto");
                                wallDistance = Vector3.zero;
                                SeePlayer(true, collision.gameObject);
                            }
                            if (difDistance > item.distnacia)
                            {
                                //Debug.Log("parede perto");
                                SeePlayer(false, null);

                            }
                        }
                    }
                    if (tempPlayer != null)
                    {
                        foreach (GameObject item in walls)
                        {
                            if (Vector2.Distance(item.transform.position, transform.position) > difDistance)
                            {
                                Debug.Log("player mais perto que a parede");
                            }
                            else if(Vector2.Distance(item.transform.position, transform.position) < difDistance)
                            {
                                Debug.Log("parde mais perto q player");
                            }
                        }
                    }

                        if (wallDistance != Vector3.zero && playerDistance != null)
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
                        }*/
                           
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
        if (!enemyModel.dead)
        {
            if (collision.gameObject.tag == "Player" && polygonCollider.enabled == true)
            {
                // seePlayer = false;
                ZerarVariaveis();
            }

            else if (collision.gameObject.tag == "Player" && circleCollider2D.enabled == true)
            {
                enemyMove.SawEnemy(null);
                lastPlayerPosition = collision.transform.position;
                ZerarVariaveis();
            }
        }

    }

    public void OnAttackRange(bool _playerVisible,GameObject _playerGameObject)
    {
        if (!enemyModel.dead)
        {
            playerOnAttackRange = _playerVisible;

            if (playerOnAttackRange) { SeePlayer(seePlayer, _playerGameObject); ChangeCollision(false); }
        }
    }

    void ZerarVariaveis()
    {
        wallDistance = Vector3.zero;
        polygonCollider.enabled = true;
        circleCollider2D.enabled = false;
        playerOnAttackRange = false;
        playerGameObject = null;
        seePlayer = false;
        entityModelTemp = null;
        difDistance = 0;
        enemyMove.SawEnemy(null);

    }
}
