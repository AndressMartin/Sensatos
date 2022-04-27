using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModoDeJogoManager : MonoBehaviour
{
    //Variaveis
    [SerializeField] private GameManager.Modo modoDeJogo;

    private void Start()
    {
        GameManager.instance.SetModoDeJogo(modoDeJogo);
    }
}
