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
    [SerializeField] bool vendoPlayer;
    [SerializeField] bool vendoPlayerCircular;
    [SerializeField] float tempo;
    [SerializeField] float DeQuantoEmQuantoSegundos;

    //componente
    private Enemy enemy;
    private VisaoCircularEnemy visaoCircularEnemy;
    private PolygonCollider2D polygonCollider;

    //Getter
    public bool GetVendoPlayer => vendoPlayer;
    public bool GetVendoPlayerCircular => vendoPlayerCircular;


    void Start()
    {
        pontoXMax = pontoX;
        pontoYMax = pontoY;

        visaoCircularEnemy = GetComponentInChildren<VisaoCircularEnemy>();
        //Debug.Log("tenho "+visaoCircularEnemy);
        polygonCollider = GetComponent<PolygonCollider2D>();
        enemy = GetComponentInParent<Enemy>();
 
        visaoCircularEnemy.ValorRaioInicial(Mathf.Sqrt((larguraConeVisao - pontoX) * (larguraConeVisao - pontoX) + (alturaConeVisao - pontoY) * (alturaConeVisao - pontoY)));
        v1 = new Vector2(larguraConeVisao, alturaConeVisao);
    }
    public void Main()
    {
        vendoPlayerCircular=visaoCircularEnemy.VendoPlayer;
        direcao = enemy.GetDirecao;
        MudarDirecaoConeVisao();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Raycast(collision);
    }
    void Raycast(Collider2D collision)
    {
        tempo+=Time.deltaTime;

        if (tempo >= DeQuantoEmQuantoSegundos)
        {
            EntityModel entityModelTemp = collision.gameObject.GetComponent<EntityModel>();
            if (entityModelTemp != null)
            {
                RaycastHit2D hits;
                hits = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y),
                    new Vector2(entityModelTemp.transform.position.x - transform.position.x, entityModelTemp.transform.position.y - transform.position.y), pontoX, mask.value);

                if (hits)//caso tenha acertado algo
                {
                    if (hits.transform.GetComponent<Player>() != null)//caso atinga o player o raio
                    {
                        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), new Vector2(hits.transform.position.x - transform.position.x, hits.transform.position.y - transform.position.y), Color.red);
                        vendoPlayer = true;
                    }
                }
            }
            tempo = 0;
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
        visaoCircularEnemy.MudarRaio();

    }
    void ZerarVariaveis()
    {
        vendoPlayer = false;
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
