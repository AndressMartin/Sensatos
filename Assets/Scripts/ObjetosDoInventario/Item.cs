using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    //Variaveis
    public enum Tipo { Consumivel, Ferramenta, ItemChave };

    [HideInInspector] public virtual Tipo tipo { get; protected set; }
    [SerializeField] protected Sprite imagemInventario;
    [SerializeField] protected string nome;
    [SerializeField] protected string descricao;

    //Getters
    public Sprite ImagemInventario => imagemInventario;
    public string Nome => nome;
    public string Descricao => descricao;

    virtual public void Usar(Player player)
    {
        //Nada.
    }

    virtual public void UsarNaGameplay(Player player)
    {
        //Nada.
    }

    virtual public string GetNomeAnimacao()
    {
        return "";
    }

    public void ThrowAway()
    {

    }
}
