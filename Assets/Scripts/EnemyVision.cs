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
    public List<GameObject> walls;
    public EntityModel entityModelTemp;
    public float difDistance;
    public List<GameObject> listaDeParedes;
    [SerializeField] LayerMask mask;

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
        v1 = new Vector2(xPointOrigin, yPointOrigin);
    }

    public void Main()
    {
        xp = transformFather.position.x;
        yp = transformFather.position.y;
        direcao = enemyModel.direcao;

        if (seePlayer)
        {
            polygonCollider.enabled = false;
            circleCollider2D.enabled = true;
        }
        else
        {
            circleCollider2D.enabled = false;
            polygonCollider.enabled = true;

        }
        MudarDirecaoConeVisao();
    }
    public void EntrarModoPatrulha()
    {
        xPointMax += 2;
        yPointMax += 2;
    }
    void MudarDirecaoConeVisao()
    {
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
                v2 = new Vector2(-(yPointMax - xPointOrigin), (xPointMax + yPointOrigin));
                v3 = new Vector2(+(yPointMax + xPointOrigin), (xPointMax + yPointOrigin));
                break;

            case Direcao.Baixo:
                v2 = new Vector2((yPointMax + xPointOrigin), -(xPointMax - yPointOrigin));
                v3 = new Vector2(-(yPointMax - xPointOrigin), -(xPointMax - yPointOrigin));
                break;

        }
        float h2 = (xPointOrigin - xPointMax) * (xPointOrigin - xPointMax) + (yPointOrigin - yPointMax) * (yPointOrigin - yPointMax);
        circleCollider2D.radius = Mathf.Sqrt(h2);
        polygonCollider.points = new[] { v1, v2, v3 };
    }


    void ChangeCollision(bool _poligon)
    {
        if (!enemyModel.morto)
        {
            polygonCollider.enabled = _poligon;
            circleCollider2D.enabled = !_poligon;
            if (circleCollider2D.enabled == true)
                seePlayer = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {//Debug.DrawRay(new Vector3(xp, yp, 0), new Vector3(cerca.transform.localPosition.x - xp, cerca.transform.localPosition.y - yp, cerca.transform.localPosition.z), Color.red);

        if (!enemyModel.morto)
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
            if(entityTemp.morto)
            {
                //Debug.Log("To vendo um corpo");
                //deteceta um corpo de inimigo no chao
            }
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (!listaDeParedes.Contains(collision.gameObject))
            {
                listaDeParedes.Add(collision.gameObject);
            }
        }

        entityModelTemp = collision.gameObject.GetComponent<Player>();
        if (entityModelTemp != null)
        {
            //Debug.Log(entityModelTemp.transform.name);

            if (!playerOnAttackRange)
            {
                float wDif=0.0F;
                RaycastHit2D hits;
                hits = Physics2D.Raycast(new Vector2(xp, yp), new Vector2(entityModelTemp.transform.position.x - xp, entityModelTemp.transform.position.y - yp),11000,mask.value);

                if (hits)
                {
                    Debug.DrawRay(new Vector2(xp, yp), new Vector2(hits.transform.position.x - xp, hits.transform.position.y - yp), Color.red);

                    if (hits.transform.GetComponent<ParedeModel>() != null)
                    {
                        if (!walls.Contains(hits.transform.gameObject))
                        {
                            walls.Add(hits.transform.gameObject);// =new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
                        }

                    }

                    if (hits.transform.GetComponent<Player>() != null)
                    {
                        SeePlayer(true, collision.gameObject);

                        // =new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z); 
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
        if (!enemyModel.morto)
        {
            if (collision.gameObject.tag == "Player" && polygonCollider.enabled == true)
            {
                // seePlayer = false;
                ZerarVariaveis();
            }

            else if (collision.gameObject.tag == "Player" && circleCollider2D.enabled == true)
            {
                enemyMove.SawEnemy(null);
                ZerarVariaveis();
            }
        }

    }

    public void OnAttackRange(bool _playerVisible,GameObject _playerGameObject)
    {
        if (!enemyModel.morto)
        {
            playerOnAttackRange = _playerVisible;

            if (playerOnAttackRange) { SeePlayer(seePlayer, _playerGameObject); ChangeCollision(false); }
        }
    }

    void ZerarVariaveis()
    {
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
