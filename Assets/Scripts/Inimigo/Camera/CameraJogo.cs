using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraJogo : MonoBehaviour
{

    //componente
    private PolygonCollider2D polygonCollider;
    private FieldOfView fieldOfView;
    private GeneralManagerScript generalManager;

    //variavel
    [SerializeField] EntityModel.Direcao direcao;
    [SerializeField] LayerMask mask;
    [SerializeField] private float fovNormal, distanciaNormal;
    [SerializeField] private float offSet;
    Vector2 v1, v2 = new Vector2(0, 0), v3 = new Vector2(0, 0);
    Vector2 posicaoInimigoMorto;

    private float fov, distancia;
    private float larguraVisao, alturaVisao;
    private float offSetOrigemX, offSetOrigemY;
    private float tempoFazerRaycast;
    private float tempoDetectarPlayer;

    //controle
    [SerializeField] private float tempoFazerRaycastMax;
    [SerializeField] private float tempoDetectarPlayerMax;
    private bool vendoPlayer, playerDetectado;
    private bool emLockdown;
    private bool vendoInimigo;
    private bool fazerPollygonCollider;


    // Start is called before the first frame update
    void Start()
    {

        polygonCollider = GetComponent<PolygonCollider2D>();
        generalManager = FindObjectOfType<GeneralManagerScript>();
        generalManager.ObjectManager.AdicionarAsCameras(this);
        GameObject novoFieldOfView = Instantiate(generalManager.FieldView);
        novoFieldOfView.transform.parent = generalManager.FieldView.transform.parent;

        fieldOfView = novoFieldOfView.GetComponent<FieldOfView>();
        fieldOfView.SetPai(gameObject);

        fov = fovNormal;
        distancia = distanciaNormal;
        tempoFazerRaycast = 0;
        fazerPollygonCollider = true;
        emLockdown = false;

        FieldOfViewAtiva(true);
        AtualizarFieldView();
    }

    // Update is called once per frame
    void Update()
    {
        if (!emLockdown)
        {
            MudarDirecaoConeVisao();
            AtualizarFieldView();
            AtualizarPollygonCollider();
            DetectarPlayer();
            AtivarLockdown();
        }

    }
    private void AtualizarFieldView()
    {
        fieldOfView.SetOrigin(gameObject.transform.position);
        fieldOfView.SetArea(fov, distancia);
    }
    void MudarDirecaoConeVisao()
    {
        switch (direcao)
        {
            //rodar plano cartesiano
            case EntityModel.Direcao.Esquerda:
                v1 = new Vector2(offSetOrigemX - offSet, offSetOrigemY);
                v2 = new Vector2((offSetOrigemX - offSet - larguraVisao), (offSetOrigemY + alturaVisao));
                v3 = new Vector2((offSetOrigemX - offSet - larguraVisao), (offSetOrigemY - alturaVisao));

                fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
                fieldOfView.SetDirection(Vector2.down);
                break;

            case EntityModel.Direcao.Direita:
                v1 = new Vector2(offSetOrigemX + offSet, offSetOrigemY);
                v2 = new Vector2((offSetOrigemX + offSet + larguraVisao), (offSetOrigemY + alturaVisao));
                v3 = new Vector2((offSetOrigemX + offSet + larguraVisao), (offSetOrigemY - alturaVisao));

                fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
                fieldOfView.SetDirection(Vector2.up);
                break;

            case EntityModel.Direcao.Cima:
                v1 = new Vector2(offSetOrigemX, offSetOrigemY + offSet);
                v2 = new Vector2((offSetOrigemX - alturaVisao), (offSetOrigemY + offSet + larguraVisao));
                v3 = new Vector2((offSetOrigemX + alturaVisao), (offSetOrigemY + offSet + larguraVisao));

                fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
                fieldOfView.SetDirection(Vector2.left);
                break;

            case EntityModel.Direcao.Baixo:
                v1 = new Vector2(offSetOrigemX, offSetOrigemY - offSet);
                v2 = new Vector2((offSetOrigemX + alturaVisao), (offSetOrigemY - offSet - larguraVisao));
                v3 = new Vector2((offSetOrigemX - alturaVisao), (offSetOrigemY - offSet - larguraVisao));

                fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
                fieldOfView.SetDirection(Vector2.right);
                break;
        }
        if (fazerPollygonCollider)
        {
            AtualizarPollygonCollider();
            fazerPollygonCollider = false;
        }
    }
    public void FieldOfViewAtiva(bool ativa)
    {
        if (fieldOfView != null)
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
            var pontoCorrigido = ponto - ((Vector2)fieldOfView.GetPai.transform.position);
            temp.Add(pontoCorrigido);
        }
        if (polygonCollider != null)
        {
            polygonCollider.points = temp.ToArray();
        } 
    }

    void DetectarPlayer()
    {
        if(vendoPlayer && !playerDetectado)
        {
            tempoDetectarPlayer += Time.deltaTime * generalManager.Player.Inventario.RoupaAtual.FatorDePercepcao;
            if(tempoDetectarPlayer >= tempoDetectarPlayerMax)
            {
                tempoDetectarPlayer = 0;
                playerDetectado = true;
                emLockdown = true;
                Debug.Log("Detectei o troxa");
            }
            else
            {
                Debug.Log("To vendo o player mas n detectei ainda");
            }
        }
        else
        {
            tempoDetectarPlayer-= Time.deltaTime;
            if (tempoDetectarPlayer <= 0)
                tempoDetectarPlayer = 0;
        }
    }
    private void AtivarLockdown()
    {
        if (playerDetectado || vendoInimigo)
        {
            if (playerDetectado)
            {
                generalManager.ObjectManager.ListaAlarmes[0].AtivarLockDown(generalManager.Player.transform.position);
                return;
            }
            generalManager.ObjectManager.ListaAlarmes[0].AtivarLockDown(posicaoInimigoMorto);
        }
    }
    public void ReceberLockdown(bool valor)
    {
        emLockdown = valor;

        fazerPollygonCollider = true; //habilita a camera a redesenhar o collider
        playerDetectado = false; 
        FieldOfViewAtiva(!valor); //fieldView inverso a se esta em lockDown
        tempoDetectarPlayer = 0;
        tempoFazerRaycast = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        tempoFazerRaycast += Time.deltaTime;

        if (tempoFazerRaycast < tempoFazerRaycastMax)
        {
            return;
        }

        if (collision.CompareTag("HitboxDano"))
        {
            EntityModel entityModelTemp = collision.transform.parent.gameObject.GetComponent<EntityModel>();
            SpriteRenderer sprite = collision.transform.parent.GetComponentInChildren<SpriteRenderer>();

            if (entityModelTemp != null)
            {
                bool viuEntidade = false;
                RaycastHit2D[] hits = new RaycastHit2D[5];

                hits[0] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x - collision.bounds.size.x / 2) - v1.x - transform.position.x, entityModelTemp.transform.position.y - v1.y - transform.position.y), distancia, mask);

                hits[1] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x + collision.bounds.size.x / 2) - v1.x - transform.position.x, entityModelTemp.transform.position.y - v1.y - transform.position.y), distancia, mask);

                hits[2] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x - collision.bounds.size.x / 2) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y) - v1.y - transform.position.y), distancia, mask);

                hits[3] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x + collision.bounds.size.x / 2) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y) - v1.y - transform.position.y), distancia, mask);

                hits[4] = Physics2D.Raycast(v1 + (Vector2)transform.position,
                    new Vector2((entityModelTemp.transform.position.x) - v1.x - transform.position.x, (entityModelTemp.transform.position.y + collision.bounds.size.y / 2) - v1.y - transform.position.y), distancia, mask);

                //Debugs
                
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
                


                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i] == false)
                    {
                        viuEntidade = true;
                        break;
                    }
                }

                if (viuEntidade == true)
                {
                    if (entityModelTemp is Enemy)
                    {
                        Enemy enemy = (Enemy)entityModelTemp;
                        if (enemy.Morto)
                        {
                            vendoInimigo = true;
                            posicaoInimigoMorto = enemy.transform.position;
                        }
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
                hits = null;
            }
        }

        tempoFazerRaycast = 0;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HitboxDano"))
        {
            Player player = collision.transform.parent.GetComponent<Player>();
            if (player != null)
            {
                vendoPlayer = false;
            }
        }

    }
}
