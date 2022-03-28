using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missao")]
public class Missao : ScriptableObject
{
    [SerializeField] protected string nomeMissao;
    [SerializeField] protected bool concluida;
    [SerializeField] protected int idMissao;
    public string GetNomeMissao => nomeMissao;
    public bool GetConcluida => concluida;
    public int GetIdMissao => idMissao;

    public bool ConferirMissao()
    {
        return concluida;
    }
}
