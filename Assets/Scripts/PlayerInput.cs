using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Managers
    private PauseManagerScript pauseManager;

    private Player player;
    private PlayerMovement movement;
    private Inventario inventario;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        pauseManager = FindObjectOfType<PauseManagerScript>();

        inventario = GetComponent<Inventario>();
        movement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.PermitirInput == true)
        {
            ComandosMenu();
            if (pauseManager.JogoPausado == false)
            {
                ComandosGameplay();
            }
        }
        else
        {
            movement.horizontal = 0;
            movement.vertical = 0;
        }
    }

    //Comandos de menus, como pausar o jogo
    void ComandosMenu()
    {
        //Pausar o jogo
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            pauseManager.Pausar();
        }
    }

    //Comandos da gameplay principal
    void ComandosGameplay()
    {
        if(player.GetEstado == Player.Estado.Normal)
        {
            //Trocar arma
            if (Input.GetKeyDown(KeyCode.Q))
            {
                inventario.TrocarArma();
                player.AtualizarArma();
            }

            //Atirar
            if(player.RapidFire == true)
            {
                if(Input.GetButton("Atirar"))
                {
                    player.Atirar();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    player.Atirar();
                }
            }

            //Recarregar
            if (Input.GetKeyDown(KeyCode.R))
            {
                player.Recarregar();
            }

            //Botão de interação
            if (Input.GetKeyDown(KeyCode.E))
            {
                player.Interagir();
            }

            //Debug - Tomar dano

            if (Input.GetKeyDown(KeyCode.G))
            {
                player.TomarDano(0, 2, 0, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            }


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
                if (player.modoMovimento != Player.ModoMovimento.Strafing)
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
        
        //Se movimentar
        if(player.GetEstado == Player.Estado.Normal)
        {
            movement.horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
            movement.vertical = Input.GetAxisRaw("Vertical"); // -1 is down
        }
        else
        {
            movement.horizontal = 0;
            movement.vertical = 0;
        }
    }
}
