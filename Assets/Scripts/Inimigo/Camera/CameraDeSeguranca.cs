using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeSeguranca : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private SpriteRenderer sprite;
    private PolygonCollider2D polygonCollider;
    private Animacao animacao;

    private FieldOfView fieldOfView;
    private BarraDeVisaoDoInimigo barraDeVisao;

    //Variaveis
    [SerializeField] LayerMask layerDasZonas;

    [SerializeField] EntityModel.Direcao posicaoCamera;

    EntityModel.Direcao direcao;

    [SerializeField] LayerMask mask;
    [SerializeField] private float fovNormal, distanciaNormal;

    [SerializeField] private bool posicoesIdaEVolta;
    [SerializeField] private List<PosicaoCamera> posicoesDaCamera;

    private float offSet = 0;

    Vector2 v1;
    Vector2 posicaoInimigoMorto;

    private float fov, distancia;
    private float offSetOrigemX, offSetOrigemY;
    private float tempoFazerRaycast;
    private float tempoDetectarPlayer;

    private int zona;

    //Variaveis de controle
    [SerializeField] private float tempoFazerRaycastMax;
    [SerializeField] private float tempoDetectarPlayerMax;
    private bool vendoPlayer, playerDetectado;
    private bool emLockdown;
    private bool vendoInimigo;
    private bool fazerPollygonCollider;
    private bool ativada;

    private bool vendoPlayerRespawn, playerDetectadoRespawn, emLockdownRespawn, vendoInimigoRespawn, fazerPollygonColliderRespawn, ativadaRespawn;

    private int indicePosicao;
    private float tempoPosicao;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Se adicionar a lista de cameras
        generalManager.ObjectManager.AdicionarAsCameras(this);

        //Componentes
        sprite = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        animacao = GetComponent<Animacao>();

        //Field of view
        GameObject novoFieldOfView = Instantiate(generalManager.FieldView);
        novoFieldOfView.transform.parent = generalManager.FieldView.transform.parent;

        fieldOfView = novoFieldOfView.GetComponent<FieldOfView>();
        fieldOfView.SetPai(gameObject);

        //Barra de visao
        var barraDeVisaoTemp = Instantiate(generalManager.Hud.BarraDeVisaoDoInimigo.gameObject);
        barraDeVisaoTemp.GetComponent<RectTransform>().SetParent(generalManager.Hud.BarraDeVisaoDoInimigo.transform.parent, false);
        barraDeVisaoTemp.gameObject.SetActive(true);

        barraDeVisao = barraDeVisaoTemp.GetComponent<BarraDeVisaoDoInimigo>();

        //Variaveis
        AtualizarPosicao();
        PosicoesIdaEVolta();

        fov = fovNormal;
        distancia = distanciaNormal;
        tempoFazerRaycast = 0;
        fazerPollygonCollider = true;
        emLockdown = false;
        ativada = true;

        tempoDetectarPlayer = 0;

        indicePosicao = 0;
        tempoPosicao = 0;

        v1 = new Vector2(offSetOrigemX, offSetOrigemY);

        FieldOfViewAtiva(true);
        AtualizarFieldView();

        SetarZona();
        SetRespawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!emLockdown && ativada && zona == generalManager.ZoneManager.ZonaAtual)
        {
            FieldOfViewAtiva(true);
            MudarDirecaoConeVisao();
            AtualizarFieldView();
            AtualizarPollygonCollider();
            DetectarPlayer();
            AtivarLockdown();

            if (vendoPlayer)
            {
                barraDeVisao.BarraDeVisaoAtiva(true);
                barraDeVisao.IconeDeAlertaAtivo(false);

                barraDeVisao.AtualizarBarraDeVisao(tempoDetectarPlayer, tempoDetectarPlayerMax);
            }
            else
            {
                barraDeVisao.BarraDeVisaoAtiva(false);
                barraDeVisao.IconeDeAlertaAtivo(false);

                if (posicoesDaCamera.Count > 1)
                {
                    AlterarPosicao();
                }
            }

            if (barraDeVisao.IconeAtivo == true)
            {
                generalManager.Hud.AtualizarBarraDeVisao(gameObject, barraDeVisao, sprite);
            }
        }
        else
        {
            barraDeVisao.BarraDeVisaoAtiva(false);
            barraDeVisao.IconeDeAlertaAtivo(false);
            FieldOfViewAtiva(false);
        }

        Animar();
    }

    private void Animar()
    {
        if(emLockdown == false)
        {
            if(animacao.AnimacaoAtual != posicaoCamera.ToString())
            {
                animacao.TrocarAnimacao(posicaoCamera.ToString());
            }
        }
        else
        {
            if (animacao.AnimacaoAtual != posicaoCamera.ToString() + "Lockdown")
            {
                animacao.TrocarAnimacao(posicaoCamera.ToString() + "Lockdown");
            }
        }
    }

    private void AlterarPosicao()
    {
        tempoPosicao += Time.deltaTime;

        if(tempoPosicao > posicoesDaCamera[indicePosicao].Tempo)
        {
            tempoPosicao -= posicoesDaCamera[indicePosicao].Tempo;

            indicePosicao++;

            if(indicePosicao >= posicoesDaCamera.Count)
            {
                indicePosicao = 0;
            }

            AtualizarPosicao();
        }
    }

    private void AtualizarPosicao()
    {
        direcao = posicoesDaCamera[indicePosicao].Direcao;
        offSetOrigemX = posicoesDaCamera[indicePosicao].OffSetOrigemX;
        offSetOrigemY = posicoesDaCamera[indicePosicao].OffSetOrigemY;

        animacao.AtualizarDirecao(posicoesDaCamera[indicePosicao].Direcao);
    }

    private void PosicoesIdaEVolta()
    {
        if(posicoesIdaEVolta == true)
        {
            int valor = posicoesDaCamera.Count;
            for (int i = 1; i < valor - 1; i++)
            {
                posicoesDaCamera.Add(posicoesDaCamera[valor - i - 1]);
            }
        }
    }

    private void AtualizarFieldView()
    {
        fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
        fieldOfView.SetArea(fov, distancia);
    }

    void MudarDirecaoConeVisao()
    {
        switch (direcao)
        {
            //rodar plano cartesiano
            case EntityModel.Direcao.Esquerda:
                v1 = new Vector2(offSetOrigemX - offSet, offSetOrigemY);

                fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
                fieldOfView.SetDirection(Vector2.down);
                break;

            case EntityModel.Direcao.Direita:
                v1 = new Vector2(offSetOrigemX + offSet, offSetOrigemY);

                fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
                fieldOfView.SetDirection(Vector2.up);
                break;

            case EntityModel.Direcao.Cima:
                v1 = new Vector2(offSetOrigemX, offSetOrigemY + offSet);

                fieldOfView.SetOrigin((Vector2)gameObject.transform.position + v1);
                fieldOfView.SetDirection(Vector2.left);
                break;

            case EntityModel.Direcao.Baixo:
                v1 = new Vector2(offSetOrigemX, offSetOrigemY - offSet);

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

    public void ReceberDesativarAtivarCamera(bool valor)
    {
        ativada = valor;
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

    public void SetRespawn()
    {
        vendoPlayerRespawn = vendoPlayer;
        playerDetectadoRespawn = playerDetectado;
        emLockdownRespawn = emLockdown;
        vendoInimigoRespawn = vendoInimigo;
        fazerPollygonColliderRespawn = fazerPollygonCollider;
        ativadaRespawn = ativada;

    }
    public void Respawn()
    {
        vendoPlayer = vendoPlayerRespawn;
        playerDetectado = playerDetectadoRespawn;
        emLockdown = emLockdownRespawn;
        vendoInimigo = vendoInimigoRespawn;
        fazerPollygonCollider = fazerPollygonColliderRespawn;
        ativada = ativadaRespawn;
    }
    private void SetarZona()
    {
        zona = 0;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0, layerDasZonas);
        foreach (Collider2D objeto in hitColliders)
        {
            if (objeto.GetComponent<Zona>())
            {
                zona = objeto.GetComponent<Zona>().GetZona;
                break;
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
                vendoPlayer = false;
            }
        }

    }

    [System.Serializable]
    private struct PosicaoCamera
    {
        //Variaveis
        [SerializeField] private EntityModel.Direcao direcao;
        [SerializeField] private float offSetOrigemX, offSetOrigemY;
        [SerializeField] private float tempo;

        //Getters
        public EntityModel.Direcao Direcao => direcao;
        public float OffSetOrigemX => offSetOrigemX;
        public float OffSetOrigemY => offSetOrigemY;
        public float Tempo => tempo;
    }
}
