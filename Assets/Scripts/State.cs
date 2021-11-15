using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public bool estadoCombate = false;
    public bool interagindo;
    public bool usandoItem;
    public int movimento = 2;

    [SerializeField] private float colldown;
    [SerializeField] private float colldowMax;
    [SerializeField] private float colldownUsandoItem;
    [SerializeField] private float colldowMaxUsandoItem;

    private Player player;
    private Movement movement;
    private Inventario inventario;
    private SpriteRenderer spriteRenderer;
    private DirectionHitbox directionHitbox;
    // Start is called before the first frame update
    void Start()
    {
        directionHitbox = GetComponentInChildren<DirectionHitbox>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventario = GetComponent<Inventario>();
        movement = GetComponent<Movement>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
       
        BotoesPressionados();
        Colldowns();

    }

    

    void EstadoCombateOnOff()
    {
        estadoCombate = !estadoCombate;
        if(estadoCombate)
            spriteRenderer.color = (Color.red);
        else
            spriteRenderer.color = (Color.white);

    }
    void Colldowns()
    {
        if (colldown > 0)
            colldown = colldown - 1 * (Time.deltaTime);

        if (colldown > 0 && colldown < colldowMax)
            interagindo = true;

        else
            interagindo = false;


        if (colldownUsandoItem > 0)
            colldownUsandoItem = colldownUsandoItem - 1 * (Time.deltaTime);

        if (colldownUsandoItem > 0 && colldownUsandoItem < colldowMaxUsandoItem)
            usandoItem = true;

        else
            usandoItem = false;
    }

    void BotoesPressionados()
    {
        if (Input.GetKeyDown(KeyCode.E) && colldown <= 0)//Botão de interação
        {
            colldown = colldowMax;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            player.TomarDano(0, Random.Range(-1f, 1f), Random.Range(-1f, 1f), 2);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Atacar(); ;
        }

        if (Input.GetKeyDown(KeyCode.Space) && colldownUsandoItem <= 0)//Botão de usar item
        {
            colldownUsandoItem = colldowMaxUsandoItem;
            usandoItem = true;
            inventario.UsarItemAtual();
            
        }

        if (Input.GetKeyDown(KeyCode.G))//Botão para entar no modo combate (vermelho == combate)
        {
            EstadoCombateOnOff();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.X))//Botão para ativar o strafing
        {
            if(player.modoMovimento != Player.ModoMovimento.Strafing)
            {
                player.modoMovimento = Player.ModoMovimento.Strafing;
            }
            else
            {
                player.modoMovimento = Player.ModoMovimento.Normal;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))//Botão para agachar //se estiver correndo ou em pé, não agachado
        {
            if (player.modoMovimento != Player.ModoMovimento.AndandoSorrateiramente)
            {
                player.modoMovimento = Player.ModoMovimento.AndandoSorrateiramente;
            }
            else
            {
                player.modoMovimento = Player.ModoMovimento.Normal;
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            inventario.TrocarArma();
        }
        
    }
}
