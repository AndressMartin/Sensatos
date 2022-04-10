using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    //Managers
    private GeneralManagerScript generalManager;


    //Variaveis
    [SerializeField] private List<NpcStruct> listaMissao;
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
        VerificarAssaltoMissao.VerificarMissao(missao, player);

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
    public void ReceberAssaltoDoManager(Assalto assalto)
    {
        if (missao != null)
        {
            VerificarAssaltoMissao.VerificarMissao(missao, generalManager.Player);
        }

        lista = null;
        //tenta primeiro com missao principal, se npc n tiver missao principal desse assalto vai pra secundaria
        MudarDialogoConformeMissao(assalto.GetMissaoPrincipal);
        if (lista == null)
        {
            MudarDialogoConformeMissao(assalto.GetMissaoSecundaria);
        }

    }
    void MudarDialogoConformeMissao(List<Missao> _missoes)
    {
        for (int i = 0; i < _missoes.Count; i++)
        {
            foreach (var listaMissoesPropria in listaMissao)
            {
                if(_missoes[i].GetId == listaMissoesPropria.GetMissao.GetId)
                {
                    foreach (var item in listaMissoesPropria.GetEstadoDialogo)
                    {     
                        if (item.GetEstado == missao.GetEstado)
                        {
                            lista = item.GetDialogueList;
                            TrocarDialogo(lista.GetDialogueList[0]);
                            break;
                        }  
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