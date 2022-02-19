using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyVisionScript : MonoBehaviour 
{

    //variaveis
    [SerializeField] LayerMask mask;
    [SerializeField] private float larguraVisao, alturaVisao;
    private float larguraConeVisao, alturaConeVisao;
    private float pontoXMax, pontoYMax;
    EntityModel.Direcao direcao;
    Vector2 v1, v2 = new Vector2(0, 0), v3 = new Vector2(0, 0);

    //variaveis controle
    [SerializeField] private bool vendoPlayer;
    [SerializeField] private bool vendoPlayerCircular;
    [SerializeField] private float tempo;
    [SerializeField] private float intervaloDeTempo;

    //componente
    private Enemy enemy;
    private VisaoCircularEnemy visaoCircularEnemy;
    private PolygonCollider2D polygonCollider;

    //Getter
    public bool GetVendoPlayer => vendoPlayer;
    public bool GetVendoPlayerCircular => vendoPlayerCircular;


    void Start()
    {
        pontoXMax = larguraVisao;
        pontoYMax = alturaVisao;

        visaoCircularEnemy = GetComponentInChildren<VisaoCircularEnemy>();
        //Debug.Log("tenho "+visaoCircularEnemy);
        polygonCollider = GetComponent<PolygonCollider2D>();
        enemy = GetComponentInParent<Enemy>();
 
        visaoCircularEnemy.ValorRaioInicial(Mathf.Sqrt((larguraConeVisao - larguraVisao) * (larguraConeVisao - larguraVisao) + (alturaConeVisao - alturaVisao) * (alturaConeVisao - alturaVisao)));
        v1 = new Vector2(larguraConeVisao, alturaConeVisao);
    }
    public void Main()
    {
        vendoPlayerCircular=visaoCircularEnemy.VendoPlayer;
        direcao = enemy.GetDirecao;
        MudarDirecaoConeVisao();
    }

    public void ResetarVariaveisDeControle()
    {
        larguraVisao = pontoXMax;
        alturaVisao = pontoYMax;
        vendoPlayer = false;
    }
    public void EntrarModoPatrulha()
    {
        larguraVisao += 2;
        alturaVisao += 2;
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
                v2 = new Vector2(-(larguraVisao - larguraConeVisao), (alturaVisao + alturaConeVisao));
                v3 = new Vector2(-(larguraVisao - larguraConeVisao), -(alturaVisao - alturaConeVisao));
                break;

            case EntityModel.Direcao.Direita:
                v2 = new Vector2((larguraVisao + larguraConeVisao), (alturaVisao + alturaConeVisao));
                v3 = new Vector2((larguraVisao + larguraConeVisao), -(alturaVisao - alturaConeVisao));
                break;

            case EntityModel.Direcao.Cima:
                v2 = new Vector2(-(alturaVisao - larguraConeVisao), (larguraVisao + alturaConeVisao));
                v3 = new Vector2(+(alturaVisao + larguraConeVisao), (larguraVisao + alturaConeVisao));
                break;

            case EntityModel.Direcao.Baixo:
                v2 = new Vector2((alturaVisao + larguraConeVisao), -(larguraVisao - alturaConeVisao));
                v3 = new Vector2(-(alturaVisao - larguraConeVisao), -(larguraVisao - alturaConeVisao));
                break;

        }
        polygonCollider.points = new[] { v1, v2, v3 };
        //float h2 = (larguraConeVisao - pontoX) * (larguraConeVisao - pontoX) + (alturaConeVisao - pontoY) * (alturaConeVisao - pontoY);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        tempo += Time.deltaTime;

        if (tempo >= intervaloDeTempo)
        {
            if (collision.CompareTag("HitboxDano"))
            {
                EntityModel entityModelTemp = collision.transform.parent.gameObject.GetComponent<EntityModel>();

                if (entityModelTemp != null)
                {
                    RaycastHit2D hits;
                    hits = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y),
                        new Vector2(entityModelTemp.transform.position.x - transform.position.x, entityModelTemp.transform.position.y - transform.position.y), larguraVisao, mask.value);

                    if (!hits)
                    {
                        if (entityModelTemp is Enemy)
                        {
                            enemy.GetIA_Enemy_Basico.VendoOutroInimigo((Enemy)entityModelTemp);
                        }

                        if (entityModelTemp is Player)
                        {
                            vendoPlayer = true;
                        }
                    }
                    if (hits)
                    {
                        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), new Vector2(hits.transform.position.x - transform.position.x, hits.transform.position.y - transform.position.y), Color.red);
                    }
                }
                tempo = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("HitboxDano"))
        {
            Player player = collision.transform.parent.GetComponent<Player>();
            if (player != null)
            {
                ZerarVariaveis();
            }
        }

    }
}
