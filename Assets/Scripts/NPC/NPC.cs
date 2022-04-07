using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    //Managers
    private GeneralManagerScript generalManager;


    //Variaveis
    [SerializeField] private NpcStruct missaoEstadoDialogo;
    [SerializeField] private Missao missao;
    [SerializeField] private DialogueList lista;

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
        //missao = null;
        //lista=null;
        //TrocarDialogo(null);
        for (int i = 0; i < assalto.GetMissaoPrincipal.Count; i++)
        {        
            if (assalto.GetMissaoPrincipal[i].GetId == missaoEstadoDialogo.GetMissao.GetId)
            {
                Debug.Log("Entrei no fi");
                exit = true;
                missao = assalto.GetMissaoPrincipal[i];

                Debug.Log("Teste " + missaoEstadoDialogo.GetEstadoDialogo.Count);
                foreach (var item in missaoEstadoDialogo.GetEstadoDialogo)
                {
                    /*Debug.Log("primeiro "+ item.GetEstado+"\nsegundo "+missao.GetEstado);
                    if(item.GetEstado == missao.GetEstado)
                    {
                        Debug.Log("Sas");
                        lista = item.GetDialogueList;
                        TrocarDialogo(lista.GetDialogueList[0]);
                    }*/
                }

                break;
            }
                
        }
    }
    

   
}
[Serializable]
public struct NpcStruct
{
    [SerializeField] private Missao missao;
    [SerializeField] private List<EstadoDIalogo> testes;

    public Missao GetMissao => missao;
    public List<EstadoDIalogo> GetEstadoDialogo => testes;

}
[Serializable] 
public struct EstadoDIalogo
{
    [SerializeField] private Missoes.Estado estado;
    [SerializeField] private DialogueList dialogo;
    public DialogueList GetDialogueList => dialogo;
    public Missoes.Estado GetEstado => estado;

}