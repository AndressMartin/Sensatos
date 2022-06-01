using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarCapitulo : MonoBehaviour
{
    public GameManager.Capitulo capituloParaMudar;
   
    public void MudarCapituloEvento()
    {
        GameManager.instance.SetCapituloAtual(capituloParaMudar);
    }
}
