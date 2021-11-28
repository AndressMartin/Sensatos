using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
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
        //Trocar arma
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventario.TrocarArma();
            player.AtualizarArma();
        }

        //Atirar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Atirar();
        }

        //Botão de interação
        if (Input.GetKeyDown(KeyCode.E) && colldown <= 0)
        {
            player.Interagir();
        }

        //Debug - Tomar dano
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            player.TomarDano(0, Random.Range(-1f, 1f), Random.Range(-1f, 1f), 2);
        }
        */

        //Atacar
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Atacar();
        }

        //Usar o item no atalho 1
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.UsarItemAtalho(0);
        }

        //Usar o item no atalho 2
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.UsarItemAtalho(1);
        }

        //Usar o item no atalho 3
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.UsarItemAtalho(2);
        }

        //Usar o item no atalho 4
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            player.UsarItemAtalho(3);
        }

        //Ativar o strafing
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.X))
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

        //Andar sorrateiramente
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
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
        
    }
}
