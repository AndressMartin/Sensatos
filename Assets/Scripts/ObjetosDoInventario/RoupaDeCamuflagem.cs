using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Roupa de Camuflagem")]

public class RoupaDeCamuflagem : ItemDoInventario
{
    //Variaveis
    [SerializeField] private float fatorDePercepcao;

    //Getters
    public virtual int ID => Listas.instance.ListaDeRoupas.GetID[this.name];
    public float FatorDePercepcao => fatorDePercepcao;
}
