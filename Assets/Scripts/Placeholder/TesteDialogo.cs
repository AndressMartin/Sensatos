using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteDialogo : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    private DialogueActivator dialogo;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        dialogo = GetComponent<DialogueActivator>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);
    }

    public override void Interagir(Player player)
    {
        dialogo.ShowDialogue(player.GeneralManager);
    }
}
