using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMissao : MonoBehaviour
{

   

    //Variaveis
    [SerializeField] private List<NpcStruct> listaMissao;
    public List<NpcStruct> GetListaMissao => listaMissao;
    Npc npc;
    public void Iniciar(Npc _npc)
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
                    foreach (var item in listaMissoesPropria.GetEstadoDialogo)
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

    public Missao GetMissao => missao;
    public List<EstadoDIalogo> GetEstadoDialogo => testes;

}
[Serializable] 
public struct EstadoDIalogo
{
    [SerializeField] private Missoes.Estado estado;
    [SerializeField] private DialogueList dialogo;
    public DialogueList GetListaDialogo => dialogo;
    public Missoes.Estado GetEstado => estado;

}