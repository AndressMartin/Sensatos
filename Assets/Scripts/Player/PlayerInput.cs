using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    private Player player;
    private PlayerMovement movement;
    private Inventario inventario;

    // Start is called before the first frame update
    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        inventario = GetComponent<Inventario>();
        movement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (generalManager.PauseManager.PermitirInput == true)
        {
            ComandosMenu();
            if (generalManager.PauseManager.JogoPausado == false)
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
        if (InputManager.Pausar())
        {
            if(generalManager.PauseManager.JogoPausado == false)
            {
                generalManager.PauseManager.Pausar(true);
            }
            else
            {
                generalManager.PauseManager.Pausar(false);
            }
        }
    }

    //Comandos da gameplay principal
    void ComandosGameplay()
    {
        if(player.GetEstado == Player.Estado.Normal)
        {
            if(player.ModoDeCombate == true)
            {
                //Trocar arma
                if (InputManager.TrocarArma())
                {
                    inventario.TrocarArma();
                    player.AtualizarArma();
                }

                //Atirar
                if(player.Inventario.ArmaSlot[inventario.ArmaAtual] != null)
                {
                    if (player.RapidFire == true)
                    {
                        if (InputManager.AtirarComRapidFire())
                        {
                            player.Atirar();
                        }
                    }
                    else
                    {
                        if (InputManager.Atirar())
                        {
                            player.Atirar();
                        }
                    }
                }

                //Recarregar
                if (InputManager.RecarregarArma())
                {
                    player.Recarregar();
                }

                //Atacar
                if (InputManager.AtaqueFisico())
                {
                    player.Atacar();
                }

                //Usar o item no atalho 1
                if (InputManager.Atalho1())
                {
                    player.UsarItemAtalho(0);
                }

                //Usar o item no atalho 2
                if (InputManager.Atalho2())
                {
                    player.UsarItemAtalho(1);
                }

                //Usar o item no atalho 3
                if (InputManager.Atalho3())
                {
                    player.UsarItemAtalho(2);
                }

                //Usar o item no atalho 4
                if (InputManager.Atalho4())
                {
                    player.UsarItemAtalho(3);
                }
            }

            //Botão de interação
            if (InputManager.Interagir())
            {
                player.Interagir();
            }

            //Ativar o lockstrafe
            if (InputManager.Lockstrafe())
            {
                if (player.modoMovimento != Player.ModoMovimento.Strafing)
                {
                    player.modoMovimento = Player.ModoMovimento.Strafing;
                }
            }

            //Desativar o lockstrafe
            if (InputManager.SoltarLockstrafe())
            {
                if (player.modoMovimento == Player.ModoMovimento.Strafing)
                {
                    player.modoMovimento = Player.ModoMovimento.Normal;
                }
            }

            //Andar sorrateiramente
            if (InputManager.AndarSorrateiramente())
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
