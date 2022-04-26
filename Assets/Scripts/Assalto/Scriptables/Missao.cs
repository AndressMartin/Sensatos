using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Missao : ScriptableObject
{
    //Variaveis
    [SerializeField] protected string nome;
    [SerializeField] protected string descricao;

    //Getters
    public string Nome => nome;
    public string Descricao => descricao;
    public Missoes.Estado GetEstado => Missoes.GetEstadoMissao(Listas.instance.ListaDeMissoes.GetIndice[this.name]); //Pega o estado da lista de estados de missoes, com o indice da lista de missoes
    public int GetId => Listas.instance.ListaDeMissoes.GetIndice[this.name];

    //Setters
    public void SetEstado(Missoes.Estado estado)
    {
        Missoes.SetEstadoMissao(Listas.instance.ListaDeMissoes.GetIndice[this.name], estado);
    }

    public virtual void ConferirMissao(GeneralManagerScript generalManagerScript)
    {
        //Nada
    }

    public void TrocarIdioma(MudarIdiomaMissao.MissaoTexto missaoTexto)
    {
        nome = missaoTexto.nome;
        descricao = missaoTexto.descricao;
    }
}
