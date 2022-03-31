using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private NpcStruct tes;

    [SerializeField] public List<NpcStruct> npcStruct;

    DialogueActivator dialogueActivator;
    private void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();
        generalManager.NpcManager.AddList(this);

        dialogueActivator = GetComponent<DialogueActivator>();
    }
    public void TrocarDialogo(DialogueObject dialogo)
    {
        dialogueActivator.SetDialogo(dialogo);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //Interagir(collision.GetComponent<Player>());
        }
    }
    /*public override void Interagir(Player player)
    {
        foreach (var item in listaAtualMissoesNpc)
        {
            VerificarAssaltoMissao.VerificarMissao(item.GetMissao, player);
        }
    }*/
    public void ReceberAssaltoDoManager(Assalto assalto)
    {
        bool exit=false;
        tes.Zerar();
        for (int i = 0; i < assalto.GetMissaoPrincipal.Count; i++)
        {
            if (exit)
            {
                break;
            }
            else
            {
                for (int x = 0; i < npcStruct.Count; i++)
                {
                    Debug.Log("Quantidade no struct: " + npcStruct.Count);
                    Debug.Log("Missao: " + assalto.GetMissaoPrincipal[i].Nome + " Struct: " + npcStruct[x].GetMissao.Nome);
                    if (exit)
                    {
                        break;
                    }
                    
                    else if (assalto.GetMissaoPrincipal[i].Nome == npcStruct[x].GetMissao.Nome)
                    {
                        Debug.Log("Entrei no fi");
                        tes = npcStruct[x];
                        exit = true;
                    }
                }
            }
        }
    }

   
}
[Serializable]
public struct NpcStruct
{
    [SerializeField] private Missao missao;
    [SerializeField] private DialogueList dialogo;

    public Missao GetMissao => missao;
    public DialogueList GetDialogueList => dialogo;
    public void Zerar()
    {
        missao = null;
        dialogo = null;
    }
}