using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogo_Ricardo : NPCDialogo
{
    //Variaveis
    [SerializeField] private DialogueObject dialogoFimDoJogo;

    public override void Interagir(Player player)
    {
        if (npc.GetMissaoAtual != null)//caso tente falar com npc e ele tenha missao pro assalto atual
        {
            if (npc.GetMissaoAtual.GetEstado == Missoes.Estado.Inativa)
            {
                dialogueActivator.ShowDialogue(player.GeneralManager);
                npc.Interagir(player);
            }
            else if (npc.GetMissaoAtual.GetEstado == Missoes.Estado.Ativa)
            {
                npc.Interagir(player);
                dialogueActivator.ShowDialogue(player.GeneralManager);

            }
            else if (npc.GetMissaoAtual.GetEstado == Missoes.Estado.Concluida)
            {
                if (generalManager.EventoManager.CondicaoFimDoJogo() == true)
                {
                    dialogueActivator.SetDialogo(dialogoFimDoJogo);
                    dialogueActivator.ShowDialogue(player.GeneralManager);
                    return;
                }

                npc.Interagir(player);
                TrocarDialogoComponenteListaIndexExpecifico(npc.GetDialogueListSemMissao, npc.RetornarDialogoGenericoAtual().GetCont);
                dialogueActivator.ShowDialogue(player.GeneralManager);
            }
        }
        else
        {
            if (generalManager.EventoManager.CondicaoFimDoJogo() == true)
            {
                dialogueActivator.SetDialogo(dialogoFimDoJogo);
                dialogueActivator.ShowDialogue(player.GeneralManager);
                return;
            }

            TrocarDialogoComponenteListaIndexExpecifico(npc.GetDialogueListSemMissao, npc.RetornarDialogoGenericoAtual().GetCont);
            dialogueActivator.ShowDialogue(player.GeneralManager);
        }
    }
}
