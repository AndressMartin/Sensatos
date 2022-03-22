using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssaltoManager
{
    [SerializeField] private static string nomeAssalto;


    [SerializeField] private static List<Missao> missaoPrincipais = new List<Missao>();
    [SerializeField] private static List<Missao> missaoSecundaria = new List<Missao>();


    [SerializeField] private static List<Missao> missoesPrincipais_Cumprir = new List<Missao>();
    [SerializeField] private static List<Missao> missoesSecundarias_Cumprir = new List<Missao>();
    public static bool Verificar(Assalto _assalto)
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
    public static void VerificarItem(Item item,Player player)
    {
        Missao_ColetarItem missaoTemp=null;
        foreach (var ms in missaoPrincipais)
        {
            if((Missao_ColetarItem)ms)
            {
                missaoTemp = (Missao_ColetarItem)ms;
            }
        }
        foreach (var ms in missaoSecundaria)
        {
            if ((Missao_ColetarItem)ms)
            {
                missaoTemp = (Missao_ColetarItem)ms;
            }
        }

        if(missaoTemp != null)
        {
            if(missaoTemp.GetItemDeMissao.GetItem == item)
            {
                if(missaoTemp.GetquantidadeItensCompletarMissao < player.InventarioMissao.ProcurarQuantidadeItem(item))
                {
                    //AtualizaHud
                }
                else
                {
                    //AtualizaHud
                    //Ativar Flag Missao Completa
                }
            }
        }
    }
}
