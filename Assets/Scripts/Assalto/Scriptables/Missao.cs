using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Missao : ScriptableObject
{
    //Variaveis
    [SerializeField] protected string nome;
    [SerializeField] protected string descricao;
    [SerializeField] private Flags.Flag flagCorrespondente;
    [SerializeField] private int id;
    [SerializeField] private bool recompensa;
    [SerializeField] private Recompensa tipoRecompensa;
    [SerializeField] private Item itemRecompesa;
    [SerializeField] private int quantidadeItensRecompensa;
    [SerializeField] private UnityEvent eventoRecompensa;
    public enum Recompensa {item,evento};
    //Getters
    public string Nome => nome;
    public string Descricao => descricao;
    public Missoes.Estado GetEstado => Missoes.GetEstadoMissao(Listas.instance.ListaDeMissoes.GetIndice[this.name]); //Pega o estado da lista de estados de missoes, com o indice da lista de missoes
    public int GetId => id;
    public bool GetRecompensa => recompensa;
    public Recompensa GetTipoRecompensa => tipoRecompensa;

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
    public void PlayerRecerberRecompensaItem(Player player)
    {
        for (int i = 0; i < quantidadeItensRecompensa; i++)
        {
           player.ReceberItemRecompensa(itemRecompesa);
        }
    }
    public void PlayerRecerberRecompensaEvento(Player player)
    {

    }
}
