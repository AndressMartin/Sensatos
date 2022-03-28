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
    private Estado estado = Estado.Inativa;

    //Getters
    public string Nome => nome;
    public string Descricao => descricao;
    public Estado GetEstado => estado;

    //Setters
    public void SetEstado(Estado estado)
    {
        this.estado = estado;
    }

    public virtual void ConferirMissao()
    {
        //Nada
    }
}
