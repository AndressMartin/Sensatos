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
}
[Serializable]
public struct NpcStruct
{
    [SerializeField] private Missao missao;
    [SerializeField] private List<EstadoDIalogo> testes;

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
