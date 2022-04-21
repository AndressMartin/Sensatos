using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteDialogo : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    private DialogueActivator dialogo;
    private NPC npc;
    private bool jaPassei;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        dialogo = GetComponent<DialogueActivator>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);
        npc = GetComponent<NPC>();
    }

    public override void Interagir(Player player)
    {
        if (npc.GetMissaoAtual != null)//caso tente falar com npc e ele tenha missao pro assalto atual
        { 
            if(npc.GetMissaoAtual.GetEstado == Missoes.Estado.Inativa)
            {
                dialogo.ShowDialogue(player.GeneralManager);
                npc.Interagir(player);
                if (npc.GetMissaoAtual.GetEstado == Missoes.Estado.Concluida)
                {
                    npc.GetNpcMissao.TrocarDialogoMissaoEspecifico(npc.GetMissaoAtual, Missoes.Estado.Concluida);
                    dialogo.ShowDialogue(player.GeneralManager);
                }
            }
            else if(npc.GetMissaoAtual.GetEstado == Missoes.Estado.Ativa)
            {
                npc.Interagir(player);
                dialogo.ShowDialogue(player.GeneralManager);
                
            }
            else if (npc.GetMissaoAtual.GetEstado == Missoes.Estado.Concluida)
            {
                npc.Interagir(player);
                dialogo.ShowDialogue(player.GeneralManager);
            }
        }
        else
        {
            dialogo.ShowDialogue(player.GeneralManager);
        }
    }
}
