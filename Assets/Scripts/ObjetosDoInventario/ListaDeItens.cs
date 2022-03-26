using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Listas/Lista de Itens")]

public class ListaDeItens : ScriptableObject
{
    //Variaveis
    [SerializeField] private Item[] itens;
    private Dictionary<string, int> getID;
    private Dictionary<int, Item> getItem;

    //Getters
    public Dictionary<string, int> GetID => getID;
    public Dictionary<int, Item> GetItem => getItem;

    public void Iniciar()
    {
        //Cria os dicionarios com os IDs automaticamente
        getID = new Dictionary<string, int>();
        getItem = new Dictionary<int, Item>();

        for (int i = 0; i < itens.Length; i++)
        {
            Debug.Log("Item ID " + i + " : " + itens[i].name);
            getID.Add(itens[i].name, i);
            getItem.Add(i, itens[i]);
        }
    }
}
