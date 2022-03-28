using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Listas/Lista de Roupas")]

public class ListaDeRoupas : ScriptableObject
{
    //Variaveis
    [SerializeField] private RoupaDeCamuflagem[] roupas;
    private Dictionary<string, int> getID;
    private Dictionary<int, RoupaDeCamuflagem> getRoupa;

    //Getters
    public Dictionary<string, int> GetID => getID;
    public Dictionary<int, RoupaDeCamuflagem> GetRoupa => getRoupa;

    public void Iniciar()
    {
        //Cria os dicionarios com os IDs automaticamente
        getID = new Dictionary<string, int>();
        getRoupa = new Dictionary<int, RoupaDeCamuflagem>();

        for (int i = 0; i < roupas.Length; i++)
        {
            getID.Add(roupas[i].name, i);
            getRoupa.Add(i, roupas[i]);
        }
    }
}
