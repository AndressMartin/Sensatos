using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private Missao missaoAtual;
    [SerializeField] private DialogueList listaDialogoAtual;
    [SerializeField] private DialogueList listaDialogoSemMissao;
    //Managers
    private GeneralManagerScript generalManager;
    DialogueActivator dialogueActivator;

    NpcMissao npcMissao;

    public Missao GetMissaoAtual => missaoAtual;
    void Start()
    {
        npcMissao = gameObject.GetComponent<NpcMissao>();
        npcMissao.Iniciar(this);
       
        generalManager = FindObjectOfType<GeneralManagerScript>();
        generalManager.NpcManager.AddList(this);

        dialogueActivator = GetComponent<DialogueActivator>();
        
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

        if(!value)
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
            VerificarAssaltoMissao.VerificarMissao(missaoAtual, generalManager.Player);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enabled)
        {
            if (collision.CompareTag("Player"))
            {
                Interagir(collision.GetComponent<Player>());
            }
        }
    }
    public void Interagir(Player player)
    {
        VerificarAssaltoMissao.VerificarMissao(missaoAtual, player);

        /*foreach (var item in listaMissao.GetEstadoDialogo)
        {
            if (item.GetEstado == missao.GetEstado)
            {
                lista = item.GetDialogueList;
                TrocarDialogo(lista.GetDialogueList[0]);
                break;
            }
        }   */
    }
}
