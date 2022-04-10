using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Listas/Lista de Missoes")]

public class ListaDeMissoes : ScriptableObject
{
    //Variaveis
    [SerializeField] private Missao[] missoes;
    private Dictionary<string, int> getIndice;

    //Getters
    public Dictionary<string, int> GetIndice => getIndice;
    public int TamanhoListaDeMissoes => missoes.Length;

    public void Iniciar()
    {
        //Cria os dicionarios com os IDs automaticamente
        getIndice = new Dictionary<string, int>();

        for (int i = 0; i < missoes.Length; i++)
        {
            getIndice.Add(missoes[i].name, i);
        }
    }
}
