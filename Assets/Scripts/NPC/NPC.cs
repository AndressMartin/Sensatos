using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    NPCDialogo NpcDialogo;
    NpcMissao npcMissao;

    //Variaveis
    Assalto assaltoAtual;
    private Missao missaoAtual;
    private DialogueList listaDialogoAtual;
    [SerializeField] private DialogueList listaDialogoSemMissao;

    public Missao GetMissaoAtual => missaoAtual;
    public NpcMissao GetNpcMissao => npcMissao;
    public DialogueList GetDialogueListSemMissao => listaDialogoSemMissao;
    public DialogueList GetDialogueListAtual=>listaDialogoAtual;
    public GeneralManagerScript GetGeneralManager => generalManager;
    void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();
        NpcDialogo = GetComponent<NPCDialogo>();
        npcMissao = GetComponent<NpcMissao>();

        generalManager.NpcManager.AddList(this);
        NpcDialogo.Iniciar(this);

        listaDialogoAtual = listaDialogoSemMissao;

        NpcDialogo.TrocarDialogoComponenteLista(listaDialogoAtual.GetDialogueList[0]);

        missaoAtual = null;
    }

    public void TrocarMissaoAtual(Missao _missao)
    {
        missaoAtual = _missao;
    }
    public void ReceberAssaltoDoManager(Assalto assalto)
    {
        if(assaltoAtual != assalto)
        {
            assaltoAtual = assalto;
            bool value = false;
            //Verifica se possui o assalto que recebeu tem alguma missao sua
            foreach (var item in assalto.GetMissaoPrincipal)
            {
                foreach (var item2 in npcMissao.GetListaMissao)
                {
                    if (item.GetId == item2.GetMissao.GetId)
                    {
                        value = true;
                    }
                }
            }
            foreach (var item in assalto.GetMissaoSecundaria)
            {
                foreach (var item2 in npcMissao.GetListaMissao)
                {
                    if (item.GetId == item2.GetMissao.GetId)
                    {
                        value = true;
                    }
                }
            }

            if (!value)
            {
                missaoAtual = null;
                listaDialogoAtual = listaDialogoSemMissao;
                NpcDialogo.TrocarDialogoComponenteLista(listaDialogoAtual.GetDialogueList[0]);
            }

            else
            {
                listaDialogoAtual = null;
                //tenta primeiro com missao principal, se npc n tiver missao principal desse assalto vai pra secundaria
                NpcDialogo.TrocarDialogoConformeMissao(assalto.GetMissaoPrincipal);
                if (listaDialogoAtual == null)
                {
                    NpcDialogo.TrocarDialogoConformeMissao(assalto.GetMissaoSecundaria);
                }
                if (missaoAtual.GetEstado != Missoes.Estado.Concluida)
                {
                    VerificarAssaltoMissao.VerificarMissao(missaoAtual, generalManager.Player,npcMissao);
                }
            }
        }
        
        else
        {
            print("São iguaos");
        }
    }

    /// <summary>
    /// Verifica o estado da missao e atualiza o dialogo do Npc
    /// </summary>
    /// <param name="player"></param>
    public void Interagir(Player player)
    {
        if (missaoAtual != null)
        {
            if (missaoAtual.GetEstado == Missoes.Estado.Concluida)
            {
                listaDialogoAtual = listaDialogoSemMissao;
                NpcDialogo.TrocarDialogoComponenteLista(listaDialogoAtual.GetDialogueList[0]);
            }
            else if (missaoAtual.GetEstado == Missoes.Estado.Ativa)
            {
                VerificarAssaltoMissao.VerificarMissao(missaoAtual, player, npcMissao);
                NpcDialogo.TrocarDialogoMissaoEspecifico(missaoAtual, missaoAtual.GetEstado);

            }
            else if (missaoAtual.GetEstado == Missoes.Estado.Inativa)
            {
                VerificarAssaltoMissao.VerificarMissao(missaoAtual, player, npcMissao);
                NpcDialogo.TrocarDialogoMissaoEspecifico(missaoAtual, Missoes.Estado.Ativa);
            }


        }
    }
    public void CompletarMissao()
    {
        missaoAtual.SetEstado(Missoes.Estado.Concluida);
    }
}
