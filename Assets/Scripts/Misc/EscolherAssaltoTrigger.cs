using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscolherAssaltoTrigger : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);

        //Desativar o sprite renderer
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void Interagir(Player player)
    {
        generalManager.Hud.MenuEscolherAssalto.IniciarMenu();
    }
}
