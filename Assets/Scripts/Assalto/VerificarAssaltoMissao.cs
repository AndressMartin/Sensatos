using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VerificarAssaltoMissao
{
    [SerializeField] private static string nomeAssalto;


    [SerializeField] private static List<Missao> missaoPrincipais = new List<Missao>();
    [SerializeField] private static List<Missao> missaoSecundaria = new List<Missao>();



    public static List<Missao> GetMissaoPrincipal => missaoPrincipais;


    public static List<Missao> GetMissaoSecundaria => missaoSecundaria;


    public static void SetarAssalto(Assalto _assalto,Player player)
    {
        VerificarAssalto(_assalto, player);
    }
    public static void VerificarAssalto(Assalto _assalto,Player player)
    {
        nomeAssalto = _assalto.GetNomeAssalto;
        missaoPrincipais = _assalto.GetMissaoPrincipal;
        missaoSecundaria = _assalto.GetMissaoSecundaria;

        List<Missao> missoesTemp = new List<Missao>();
        missoesTemp = null;

    }
    
    public static void VerificarMissao(Missao _missao, Player player,NpcMissao npcMissao)
    {
        MissaoVerificar(_missao, player, missaoPrincipais);
        MissaoVerificar(_missao, player, missaoSecundaria);
    }
    static void MissaoVerificar(Missao _missao, Player player, List<Missao> listaMissao)
    {
        foreach (var missaoPr in listaMissao)
        {
            if (_missao.GetId == missaoPr.GetId)
            {
                if (_missao.GetEstado == Missoes.Estado.Inativa)
                {
                    //esse iten agora eu preciso de mais dele
                    Debug.Log("Inativo");
                    _missao.SetEstado(Missoes.Estado.Ativa);
                }
                else if (_missao.GetEstado == Missoes.Estado.Ativa)
                {
                    _missao.ConferirMissao(player.GeneralManager);
                }
            }
        }
    }
}
