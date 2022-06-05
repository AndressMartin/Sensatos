using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarCapitulo : MonoBehaviour
{
    [SerializeField] private GameManager.Capitulo capituloParaMudar;
   
    public void MudarCapituloEvento()
    {
        GameManager.instance.SetCapituloAtual(capituloParaMudar);
    }
}
