using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Listas/Lista de Armas")]

public class ListaDeArmas : ScriptableObject
{
    //Variaveis
    [SerializeField] private ArmaDeFogo[] armas;
    private Dictionary<string, int> getID;
    private Dictionary<int, ArmaDeFogo> getArma;

    //Getters
    public Dictionary<string, int> GetID => getID;
    public Dictionary<int, ArmaDeFogo> GetArma => getArma;

    public void Iniciar()
    {
        //Cria os dicionarios com os IDs automaticamente
        getID = new Dictionary<string, int>();
        getArma = new Dictionary<int, ArmaDeFogo>();

        for(int i = 0; i < armas.Length; i++)
        {
            getID.Add(armas[i].name, i);
            getArma.Add(i, armas[i]);
        }
    }
}
