using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManagerScript : MonoBehaviour
{
    private static bool jogoPausado, permitirInput;

    //Getters
    public bool JogoPausado => jogoPausado;
    public bool PermitirInput => permitirInput;

    void Start()
    {
        jogoPausado = false;
        permitirInput = true; //Permite que o jogador use comandos da gameplay principal, isto nao inclui os menus, inventario, lojas e caixas de dialogo
    }

    public void Pausar()
    {
        if(jogoPausado == false)
        {
            jogoPausado = true;
            Time.timeScale = 0; //Pausa todas as operacoes calculadas com tempo
            AudioListener.pause = true; //Pausa os sons do jogo
        }
        else
        {
            jogoPausado = false;
            Time.timeScale = 1; //Resume todas as operacoes calculadas com tempo
            AudioListener.pause = false; //Resume os sons do jogo
        }
    }

    public void SetPermitirInput(bool permitir)
    {
        permitirInput = permitir;
    }
}
