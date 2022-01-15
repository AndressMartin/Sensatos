using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyVisionScript : MonoBehaviour 
{

    //variaveis
    [SerializeField] LayerMask mask;
    [SerializeField] private float pontoX, pontoY;
    private float larguraConeVisao, alturaConeVisao;
    private float pontoXMax, pontoYMax;
    EntityModel.Direcao direcao;
    Vector2 v1, v2 = new Vector2(0, 0), v3 = new Vector2(0, 0);

    //variaveis controle
    public bool vendoPlayer;
    public bool vendoPlayerCircular;
    //componente
    private Enemy enemy;
    private EnemyMovement enemyMove;
    private VisaoCircularEnemy visaoCircularEnemy;
    private PolygonCollider2D polygonCollider;
    // Start is called before the first frame update
    void Start()
    {
        pontoXMax = pontoX;
        pontoYMax = pontoY;

        visaoCircularEnemy = GetComponentInChildren<VisaoCircularEnemy>();
        enemyMove = GetComponentInParent<EnemyMovement>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        enemy = GetComponentInParent<Enemy>();
        enemyMove.EnemyVissionReference(this);

        visaoCircularEnemy.mudarRaio(Mathf.Sqrt((larguraConeVisao - pontoX) * (larguraConeVisao - pontoX) + (alturaConeVisao - pontoY) * (alturaConeVisao - pontoY)));
        v1 = new Vector2(larguraConeVisao, alturaConeVisao);
    }
    public void Main()
    {
        vendoPlayerCircular=visaoCircularEnemy.VendoPlayer;
        direcao = enemy.direcao;
        MudarDirecaoConeVisao();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        EntityModel entityModelTemp = collision.gameObject.GetComponent<EntityModel>();

        if (entityModelTemp != null)
        {
            //Debug.Log(entityModelTemp.transform.name);
            float wDif = 0.0F;
            RaycastHit2D hits;
            hits = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(entityModelTemp.transform.position.x - transform.position.x, entityModelTemp.transform.position.y - transform.position.y), 11000, mask.value);

            if (hits)//caso tenha acertado algo
            {
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), new Vector2(hits.transform.position.x - transform.position.x, hits.transform.position.y - transform.position.y), Color.red);

                if (hits.transform.GetComponent<Player>() != null)//caso atinga o player o raio
                {
                    vendoPlayer = true;
                    //antigo see player
                    // =new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z); 
                }

                /*if(hits.transform.GetComponent<Enemy>() != null)
                {
                    if (entityTemp.morto)
                    {
                        //Debug.Log("To vendo um corpo");
                        //deteceta um corpo de inimigo no chao
                    }*/
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            ZerarVariaveis();
        }

        else if (collision.gameObject.tag == "Player")
        {
            vendoPlayer=false;
            ZerarVariaveis();
        }
        

    }
    void SeePlayer(bool _player, GameObject _gameObject)
    {
        vendoPlayer = _player;
        enemyMove.SawEnemy(_gameObject);
    }

    public void ResetarVariaveisDeControle()
    {
        pontoX = pontoXMax;
        pontoY = pontoYMax;
        vendoPlayer = false;
    }
    public void EntrarModoPatrulha()
    {
        pontoX += 2;
        pontoY += 2;
    }
    void ZerarVariaveis()
    {
        vendoPlayer = false;
        enemyMove.SawEnemy(null);

    }
    void MudarDirecaoConeVisao()
    {
        switch (direcao)
        {
            //rodar plano cartesiano
            case EntityModel.Direcao.Esquerda:
                v2 = new Vector2(-(pontoX - larguraConeVisao), (pontoY + alturaConeVisao));
                v3 = new Vector2(-(pontoX - larguraConeVisao), -(pontoY - alturaConeVisao));
                break;

            case EntityModel.Direcao.Direita:
                v2 = new Vector2((pontoX + larguraConeVisao), (pontoY + alturaConeVisao));
                v3 = new Vector2((pontoX + larguraConeVisao), -(pontoY - alturaConeVisao));
                break;

            case EntityModel.Direcao.Cima:
                v2 = new Vector2(-(pontoY - larguraConeVisao), (pontoX + alturaConeVisao));
                v3 = new Vector2(+(pontoY + larguraConeVisao), (pontoX + alturaConeVisao));
                break;

            case EntityModel.Direcao.Baixo:
                v2 = new Vector2((pontoY + larguraConeVisao), -(pontoX - alturaConeVisao));
                v3 = new Vector2(-(pontoY - larguraConeVisao), -(pontoX - alturaConeVisao));
                break;

        }
        polygonCollider.points = new[] { v1, v2, v3 };
        //float h2 = (larguraConeVisao - pontoX) * (larguraConeVisao - pontoX) + (alturaConeVisao - pontoY) * (alturaConeVisao - pontoY);

    }
}
