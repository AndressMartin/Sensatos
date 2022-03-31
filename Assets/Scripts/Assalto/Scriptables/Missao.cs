using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missao : ScriptableObject
{
    //Enuns
    public enum Estado { Inativa, Ativa, Concluida }

    //Variaveis
    [SerializeField] protected string nome;
    [SerializeField] protected string descricao;
    [SerializeField] private Flags.Flag flagCorrespondente;
    [SerializeField] private Estado estado = Estado.Inativa;
    [SerializeField] private int id;
    //Getters
    public string Nome => nome;
    public string Descricao => descricao;
    public Estado GetEstado => estado;
    public Flags.Flag GetFlag => flagCorrespondente;
    public int GetId => id;

    //Setters
    public void SetEstado(Estado estado)
    {
        this.estado = estado;
    }

    public virtual void ConferirMissao()
    {
        //Nada
    }
    public void SetarFlag(bool valor)
    {
        if(valor)
        {
            SetEstado(Estado.Concluida);
        }
        Flags.SetFlag(flagCorrespondente, valor);
    }
}
