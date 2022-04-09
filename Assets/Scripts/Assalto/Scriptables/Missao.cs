using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missao : ScriptableObject
{
    //Variaveis
    [SerializeField] protected string nome;
    [SerializeField] protected string descricao;
    [SerializeField] private Flags.Flag flagCorrespondente;
    [SerializeField] private int id;
    //Getters
    public string Nome => nome;
    public string Descricao => descricao;
    public Missoes.Estado GetEstado => Missoes.GetEstadoMissao(Listas.instance.ListaDeMissoes.GetIndice[this.name]); //Pega o estado da lista de estados de missoes, com o indice da lista de missoes
    public int GetId => id;

    //Setters
    public void SetEstado(Missoes.Estado estado)
    {
        Missoes.SetEstadoMissao(Listas.instance.ListaDeMissoes.GetIndice[this.name], estado);
    }

    public virtual void ConferirMissao()
    {
        //Nada
    }

    public void SetarFlag(bool valor)
    {
        if(valor)
        {
            SetEstado(Missoes.Estado.Concluida);
        }

        Flags.SetFlag(flagCorrespondente, valor);
    }
}
