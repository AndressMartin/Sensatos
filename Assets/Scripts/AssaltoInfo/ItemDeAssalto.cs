using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assaltos/Item de Assalto")]

public class ItemDeAssalto : ScriptableObject
{
    //Variaveis
    private string nome = "";

    [SerializeField] private string nomePortugues;
    [SerializeField] private string nomeIngles;

    [SerializeField] private int valor;

    private int quantidade = 0;

    //Getters
    public string Nome => nome;
    public int Valor => valor;
    public int Quantidade => quantidade;

    //Setters
    public void SetQuantidade(int quantidade)
    {
        this.quantidade = quantidade;
    }

    public void TrocarIdioma()
    {
        switch (IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                nome = nomePortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                nome = nomeIngles;
                break;
        }
    }
}
