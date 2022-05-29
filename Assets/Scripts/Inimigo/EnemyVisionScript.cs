using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyVisionScript : MonoBehaviour 
{

    //variaveis
    [SerializeField] LayerMask mask;
    [SerializeField] private float fovNormal, distanciaNormal;
    [SerializeField] private float fovAlerta, distanciaAlerta;
    [SerializeField] private float raioVisaoCircular;
    [SerializeField] private float offSet;
 
    private float fov, distancia;
    private float offSetOrigemX, offSetOrigemY;

    Vector2 v1;
    EntityModel.Direcao direcao;

    //Variaveis de controle
    [SerializeField] private bool vendoPlayer;
    [SerializeField] private bool vendoPlayerCircular;
    [SerializeField] private float intervaloDeTempo;

    EntityModel.Direcao direcaoAntiga;

    private float tempo;
    bool controle;

    //Componentes
    private Enemy enemy;
    private VisaoCircularEnemy visaoCircularEnemy;
    private PolygonCollider2D polygonCollider;
    private FieldOfView fieldOfView;

    //Getters
    public bool GetVendoPlayer => vendoPlayer;
    public bool GetVendoPlayerCircular => vendoPlayerCircular;

    void Start()
    {
        direcaoAntiga = direcao;
        //transform.Rotate(0, 0, -90);

        fov = fovNormal;
        distancia = distanciaNormal;

        offSetOrigemX = 0;
        offSetOrigemY = 0;

        tempo = 0;

        visaoCircularEnemy = GetComponentInChildren<VisaoCircularEnemy>();
        //Debug.Log("tenho "+visaoCircularEnemy);
        polygonCollider = GetComponent<PolygonCollider2D>();
        enemy = GetComponentInParent<Enemy>();

        visaoCircularEnemy.ValorRaioInicial(raioVisaoCircular);

        //visaoCircularEnemy.ValorRaioInicial(Mathf.Sqrt((larguraConeVisao - larguraVisao) * (larguraConeVisao - larguraVisao) + (alturaConeVisao - alturaVisao) * (alturaConeVisao - alturaVisao)));
        
        v1 = new Vector2(offSetOrigemX, offSetOrigemY);

        MudarVisao(false);

        //Field of View
        GeneralManagerScript generalManager = FindObjectOfType<GeneralManagerScript>();

        GameObject novoFieldOfView = Instantiate(generalManager.FieldView);
        novoFieldOfView.transform.parent = generalManager.FieldView.transform.parent;

        fieldOfView = novoFieldOfView.GetComponent<FieldOfView>();
        fieldOfView.SetPai(enemy.gameObject);
        fieldOfView.SetArea(fov, distancia);
    }
    
    public void Main()
    {
        vendoPlayerCircular=visaoCircularEnemy.VendoPlayer;
        direcao = enemy.GetDirecao;
        MudarDirecaoConeVisao();

        if(enemy.GetIAEnemy.GetEstadoDeteccaoPlayer == IAEnemy.EstadoDeteccaoPlayer.PlayerDetectado)
        {
            Vector2[] vectors = new Vector2[1] {new Vector2(0,0)};
            polygonCollider.points = vectors;
            vendoPlayer = false;
        }
        else
        {
            AtualizarFieldView();
        }

    }

    public void ResetarVariaveisDeControle()
    {
        fov = fovNormal;
        distancia = distanciaNormal;
        vendoPlayer = false;
    }
    public void EntrarModoPatrulha()
    {
        fov = fovAlerta;
        distancia = distanciaAlerta;
    }
    public void SairModoPatrulha()
    {
        fov = fovNormal;
        distancia = distanciaNormal;
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
                v1 = new Vector2(offSetOrigemX - offSet, offSetOrigemY);

                fieldOfView.SetOrigin((Vector2)enemy.transform.position + v1);
                fieldOfView.SetDirection(Vector2.down);
                break;

            case EntityModel.Direcao.Direita:
                v1 = new Vector2(offSetOrigemX + offSet, offSetOrigemY);

                fieldOfView.SetOrigin((Vector2)enemy.transform.position + v1);
                fieldOfView.SetDirection(Vector2.up);
                break;

            case EntityModel.Direcao.Cima:
                v1 = new Vector2(offSetOrigemX, offSetOrigemY + offSet);

                fieldOfView.SetOrigin((Vector2)enemy.transform.position + v1);
                fieldOfView.SetDirection(Vector2.left);
                break;

            case EntityModel.Direcao.Baixo:
                v1 = new Vector2(offSetOrigemX, offSetOrigemY - offSet);

                fieldOfView.SetOrigin((Vector2)enemy.transform.position + v1);
                fieldOfView.SetDirection(Vector2.right);
                break;
        }
        if(direcaoAntiga != direcao)
        {
            direcaoAntiga = direcao;
            AtualizarPollygonCollider();
        }
        //float h2 = (larguraConeVisao - pontoX) * (larguraConeVisao - pontoX) + (alturaConeVisao - pontoY) * (alturaConeVisao - pontoY);

    }
    public void MudarVisao(bool estado)
    {
        if (estado && !controle)
        {
            EntrarModoPatrulha();
            controle = true;
        }
        else if (!estado)
        {
            SairModoPatrulha();
            controle = false;
        }
    }

    private void AtualizarFieldView()
    {
        //fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
        fieldOfView.SetArea(fov, distancia);
    }

    public void FieldOfViewAtiva(bool ativa)
    {
        if(fieldOfView != null)
        {
            fieldOfView.gameObject.SetActive(ativa);
        }
    }
    void AtualizarPollygonCollider()
    {
        List<Vector2> temp = new List<Vector2>();
        for (int i = 0; i < fieldOfView.GetPontos.Count; i++)
        {
            Vector2 ponto = fieldOfView.GetPontos[i];
            var pontoCorrigido = ponto - ((Vector2)enemy.transform.position);
            temp.Add(pontoCorrigido);
        }
        if (polygonCollider != null)
        {
            polygonCollider.points = temp.ToArray();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        tempo += Time.deltaTime;

        if (tempo < intervaloDeTempo)
        {
            return;
        }

        if (collision.CompareTag("HitboxDano"))
        {
            EntityModel entityModelTemp = collision.transform.parent.gameObject.GetComponent<EntityModel>();

            if (entityModelTemp != null)
            {
                bool viuEntidade = false;
                RaycastHit2D[] hits = new RaycastHit2D[5];

                hits[0] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x - collision.bounds.size.x / 2) - v1.x - transform.position.x, entityModelTemp.transform.position.y - v1.y - transform.position.y), distancia, mask.value);

                hits[1] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x + collision.bounds.size.x / 2) - v1.x - transform.position.x, entityModelTemp.transform.position.y - v1.y - transform.position.y), distancia, mask.value);

                hits[2] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x - collision.bounds.size.x / 2) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y) - v1.y - transform.position.y), distancia, mask.value);

                hits[3] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x + collision.bounds.size.x / 2) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y) - v1.y - transform.position.y), distancia, mask.value);

                hits[4] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y / 2) - v1.y - transform.position.y), distancia, mask.value);

                //Debugs
                /*
                Debug.DrawRay(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x - collision.bounds.size.x / 2) - v1.x - transform.position.x, entityModelTemp.transform.position.y - v1.y - transform.position.y), Color.black);

                Debug.DrawRay(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x + collision.bounds.size.x / 2) - v1.x - transform.position.x, entityModelTemp.transform.position.y - v1.y - transform.position.y), Color.black);

                Debug.DrawRay(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x - collision.bounds.size.x / 2) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y) - v1.y - transform.position.y), Color.black);

                Debug.DrawRay(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x + collision.bounds.size.x / 2) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y) - v1.y - transform.position.y), Color.black);

                Debug.DrawRay(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y / 2) - v1.y - transform.position.y), Color.black);
                */

                for (int i = 0; i < hits.Length; i++)
                {
                    if(hits[i] == false)
                    {
                        viuEntidade = true;
                        break;
                    }
                }

                if (viuEntidade == true)
                {
                    if (entityModelTemp is Enemy)
                    {
                        enemy.GetIAEnemy.VendoOutroInimigo((Enemy)entityModelTemp);
                    }

                    if (entityModelTemp is Player)
                    {
                        vendoPlayer = true;
                    }
                }
                else
                {
                    vendoPlayer = false;
                }
            }
        }

        tempo = 0;
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
