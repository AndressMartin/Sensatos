using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assalto")]
public class Assalto : ScriptableObject
{
    //Variaveis
    private string nome;

    [SerializeField] private string nomePortugues;
    [SerializeField] private string nomeIngles;
    [SerializeField] private Sprite imagem;
    [SerializeField] private List<Missao> missoesPrincipais = new List<Missao>();
    [SerializeField] private List<Missao> missoesSecundarias = new List<Missao>();

    //Getters
    public string Nome => nome;
    public Sprite Imagem => imagem;
    public List<Missao> GetMissoesPrincipais => missoesPrincipais;
    public List<Missao> GetMissoesSecundarias => missoesSecundarias;
    public bool MissoesPrincipaisConcluidas()
    {
        foreach (Missao missao in missoesPrincipais)
        {
            if (missao.GetEstado != Missoes.Estado.Concluida)
            {
                return false;
            }
        }

        return true;
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
