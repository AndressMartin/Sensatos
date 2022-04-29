using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VerificarAssaltoMissao
{  
    public static void VerificarMissao(Missao _missao, GeneralManagerScript generalManager)
    {
        foreach (var missao in generalManager.AssaltoManager.GetAssaltoAtual.GetMissoesPrincipais)
        {
            if (_missao.GetId == missao.GetId)
            {
                if (_missao.GetEstado == Missoes.Estado.Inativa)
                {
                    _missao.SetEstado(Missoes.Estado.Ativa);
                }
                else if (_missao.GetEstado == Missoes.Estado.Ativa)
                {
                    _missao.ConferirMissao(generalManager);
                }
            }
        }

        foreach (var missao in generalManager.AssaltoManager.GetAssaltoAtual.GetMissoesSecundarias)
        {
            if (_missao.GetId == missao.GetId)
            {
                if (_missao.GetEstado == Missoes.Estado.Inativa)
                {
                    _missao.SetEstado(Missoes.Estado.Ativa);
                }
                else if (_missao.GetEstado == Missoes.Estado.Ativa)
                {
                    _missao.ConferirMissao(generalManager);
                }
            }
        }
    }
}
