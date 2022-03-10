using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDoInventario : ScriptableObject
{
    [SerializeField] protected Sprite imagemInventario;
    [SerializeField] protected string nome;
    [SerializeField] protected string descricao;

    //Getters
    public Sprite ImagemInventario => imagemInventario;
    public string Nome => nome;
    public string Descricao => descricao;
}
