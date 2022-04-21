using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcMissao : MonoBehaviour
{
    //Variaveis
    [SerializeField] private List<NpcStruct> listaMissao;
    public List<NpcStruct> GetListaMissao => listaMissao;
    NPC npc;
    public void Iniciar(NPC _npc)
    {
        npc = _npc;
    }
    public void MudarDialogoConformeMissao(List<Missao> _missoes)
    {
        for (int i = 0; i < _missoes.Count; i++)
        {
            foreach (var listaMissoesPropria in listaMissao)
            {
                if(_missoes[i].GetId == listaMissoesPropria.GetMissao.GetId)
                {
                    foreach (var item in listaMissoesPropria.GetMissaoEstado)
                    {
                        npc.TrocarMissaoAtual(listaMissoesPropria.GetMissao);
                        if (item.GetEstado == npc.GetMissaoAtual.GetEstado)
                        {
                            TrocarDialogo(item.GetListaDialogo);
                            break;
                        }  
                    }
                }
            }          
        }
    }
    public void TrocarDialogoMissao(Missao _missao)
    {
        foreach (var listaMissoesPropria in listaMissao)
        {
            if (_missao.GetId == listaMissoesPropria.GetMissao.GetId)
            {
                foreach (var item in listaMissoesPropria.GetMissaoEstado)
                {
                    npc.TrocarMissaoAtual(listaMissoesPropria.GetMissao);
                    if (item.GetEstado == npc.GetMissaoAtual.GetEstado)
                    {
                        TrocarDialogo(item.GetListaDialogo);
                        break;
                    }
                }
            }
        }
    }
    public void TrocarDialogoMissaoEspecifico(Missao _missao,Missoes.Estado estado)
    {
        foreach (var listaMissoesPropria in listaMissao)
        {
            if (_missao.GetId == listaMissoesPropria.GetMissao.GetId)
            {
                foreach (var item in listaMissoesPropria.GetMissaoEstado)
                {
                    if (item.GetEstado == estado)
                    {
                        TrocarDialogo(item.GetListaDialogo);
                        break;
                    }
                }
            }
        }
    }
    public void TrocarDialogo(DialogueList dialogo)
    {
        npc.TrocarDialogo(dialogo);
    }
}
[Serializable]
public struct NpcStruct
{
    [SerializeField] private Missao missao;
    [SerializeField] private List<EstadoDIalogo> testes;
    [SerializeField] private UnityEvent eventosRecompensa;

    public Missao GetMissao => missao;
    public List<EstadoDIalogo> GetMissaoEstado => testes;

}
[Serializable] 
public struct EstadoDIalogo
{
    [SerializeField] private Missoes.Estado estado;
    [SerializeField] private DialogueList dialogo;
    public DialogueList GetListaDialogo => dialogo;
    public Missoes.Estado GetEstado => estado;

}