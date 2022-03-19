using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaltoManager : MonoBehaviour
{
    


    [SerializeField] private string nomeAssalto;


    [SerializeField] private List<Missao> missaoPrincipais = new List<Missao>();
    [SerializeField] private List<Missao> missaoSecundaria = new List<Missao>();


    [SerializeField] private List<Missao> missoesPrincipais_Cumprir = new List<Missao>();
    [SerializeField] private List<Missao> missoesSecundarias_Cumprir = new List<Missao>();
    public bool Verificar(Assalto _assalto)
    {
        missoesPrincipais_Cumprir.Clear();
        missoesSecundarias_Cumprir.Clear();

        nomeAssalto = _assalto.GetNomeAssalto;
        missaoPrincipais = _assalto.GetMissaoPrincipal;
        missaoSecundaria = _assalto.GetMissaoSecundaria;

        foreach (var item in missaoPrincipais)
        {
            if (!item.GetConcluida)
            {
                missoesPrincipais_Cumprir.Add(item);
            }
        }
        foreach (var item in missaoSecundaria)
        {
            if (!item.GetConcluida)
            {
                missoesSecundarias_Cumprir.Add(item);
            }
        }
        if (missoesPrincipais_Cumprir.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
