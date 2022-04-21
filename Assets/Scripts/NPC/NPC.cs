using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Missao missaoAtual;
    [SerializeField] private DialogueList listaDialogoAtual;
    [SerializeField] private DialogueList listaDialogoSemMissao;
    //Managers
    private GeneralManagerScript generalManager;
    DialogueActivator dialogueActivator;

    NpcMissao npcMissao;
    Assalto assaltoAtual;

    public Missao GetMissaoAtual => missaoAtual;
    public NpcMissao GetNpcMissao => npcMissao;
    void Start()
    {
        npcMissao = gameObject.GetComponent<NpcMissao>();
        npcMissao.Iniciar(this);
       
        generalManager = FindObjectOfType<GeneralManagerScript>();
        generalManager.NpcManager.AddList(this);
        dialogueActivator = GetComponent<DialogueActivator>();

        missaoAtual = null;
        listaDialogoAtual = listaDialogoSemMissao;
        TrocarDialogo(listaDialogoAtual);

    }
    public void TrocarDialogo(DialogueList _list)
    {
        listaDialogoAtual = _list;
        dialogueActivator.SetDialogo(_list.GetDialogueList[0]);
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
                TrocarDialogo(listaDialogoAtual);
            }

            else
            {
                listaDialogoAtual = null;
                //tenta primeiro com missao principal, se npc n tiver missao principal desse assalto vai pra secundaria
                npcMissao.MudarDialogoConformeMissao(assalto.GetMissaoPrincipal);
                if (listaDialogoAtual == null)
                {
                    npcMissao.MudarDialogoConformeMissao(assalto.GetMissaoSecundaria);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enabled)
        {
            if (collision.CompareTag("Player"))
            {
                //Interagir(collision.GetComponent<Player>());
            }
        }
    }
    public void Interagir(Player player)
    {
        if (missaoAtual != null)
        {
            if (missaoAtual.GetEstado == Missoes.Estado.Concluida)
            {
                listaDialogoAtual = listaDialogoSemMissao;
                TrocarDialogo(listaDialogoAtual);
            }
            else
            {
                VerificarAssaltoMissao.VerificarMissao(missaoAtual, player, npcMissao);
            }
            
        }
        else
        {
            
        }
    }
}
