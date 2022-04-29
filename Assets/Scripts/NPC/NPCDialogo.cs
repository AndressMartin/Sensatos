using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogo : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    private DialogueActivator dialogueActivator;
    private NPC npc;

    public void Iniciar(NPC _npc)
    {
        npc = _npc;
        generalManager = npc.GetGeneralManager;
        dialogueActivator = GetComponent<DialogueActivator>();

        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);
    }

    public override void Interagir(Player player)
    {
        if (npc.GetMissaoAtual != null)//caso tente falar com npc e ele tenha missao pro assalto atual
        { 
            if(npc.GetMissaoAtual.GetEstado == Missoes.Estado.Inativa)
            {
                dialogueActivator.ShowDialogue(player.GeneralManager);
                npc.Interagir(player);
            }
            else if(npc.GetMissaoAtual.GetEstado == Missoes.Estado.Ativa)
            {
                npc.Interagir(player);
                dialogueActivator.ShowDialogue(player.GeneralManager);
                
            }
            else if (npc.GetMissaoAtual.GetEstado == Missoes.Estado.Concluida)
            {
                npc.Interagir(player);
                TrocarDialogoComponenteListaIndexExpecifico(npc.GetDialogueListSemMissao,npc.RetornarDialogoGenericoAtual().GetCont);
                dialogueActivator.ShowDialogue(player.GeneralManager);
            }
        }
        else
        {
            TrocarDialogoComponenteListaIndexExpecifico(npc.GetDialogueListSemMissao, npc.RetornarDialogoGenericoAtual().GetCont);
            dialogueActivator.ShowDialogue(player.GeneralManager);
        }
    }
    public void TrocarDialogoComponenteListaIndexExpecifico(DialogueList _list,int num)
    {
        dialogueActivator.SetDialogo(_list.GetDialogueList[num]);
        Contador();

    }
    public void TrocarDialogoComponenteLista(DialogueObject _list)
    {
        dialogueActivator.SetDialogo(_list);
    }
    public void TrocarDialogoMissaoEspecifico(Missao _missao, Missoes.Estado estado)
    {
        foreach (var listaMissoesPropria in npc.GetNpcMissao.GetListaMissao)
        {
            if (_missao.GetId == listaMissoesPropria.GetMissao.GetId)
            {
                foreach (var item in listaMissoesPropria.GetMissaoEstado)
                {
                    if (item.GetEstado == estado)
                    {
                        TrocarDialogoComponenteLista(item.GetListaDialogo);
                        break;
                    }
                }
            }
        }
    }
    public void TrocarDialogoConformeMissao(List<Missao> _missoes)
    {
        for (int i = 0; i < _missoes.Count; i++)
        {
            foreach (var listaMissoesPropria in npc.GetNpcMissao.GetListaMissao)
            {
                if (_missoes[i].GetId == listaMissoesPropria.GetMissao.GetId)
                {
                    foreach (var item in listaMissoesPropria.GetMissaoEstado)
                    {
                        npc.TrocarMissaoAtual(listaMissoesPropria.GetMissao);
                        if (item.GetEstado == npc.GetMissaoAtual.GetEstado)
                        {
                            TrocarDialogoComponenteLista(item.GetListaDialogo);
                            break;
                        }
                    }
                }
            }
        }
    }

    void Contador()
    {
        npc.RetornarDialogoGenericoAtual().addCont();
    }
}
