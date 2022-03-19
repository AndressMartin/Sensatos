using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assalto")]
public class Assalto : ScriptableObject
{
    [SerializeField] private string nomeAssalto;
    [SerializeField] private List<Missao> missaoPrincipais = new List<Missao>();
    [SerializeField] private List<Missao> missaoSecundaria = new List<Missao>();

    public string GetNomeAssalto => nomeAssalto;
    public List<Missao> GetMissaoPrincipal => missaoPrincipais;
    public List<Missao> GetMissaoSecundaria => missaoSecundaria;
}
