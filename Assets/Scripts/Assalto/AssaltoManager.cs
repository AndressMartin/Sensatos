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

    public static void SetarAssalto(Assalto _assalto)
    {
        nomeAssalto = _assalto.GetNomeAssalto;
        missaoPrincipais = _assalto.GetMissaoPrincipal;
        missaoSecundaria = _assalto.GetMissaoSecundaria;
    }
    public static bool VerificarAssalto(Assalto _assalto,Player player)
    {
        missoesPrincipais_Cumprir.Clear();
        missoesSecundarias_Cumprir.Clear();

        nomeAssalto = _assalto.GetNomeAssalto;
        missaoPrincipais = _assalto.GetMissaoPrincipal;
        missaoSecundaria = _assalto.GetMissaoSecundaria;

        List<Missao> missoesTemp = new List<Missao>();
        VerificarMissaoCompleta(missaoPrincipais,missoesPrincipais_Cumprir,missoesTemp,player);
        VerificarMissaoCompleta(missaoSecundaria, missoesSecundarias_Cumprir, missoesTemp, player);    
        missoesTemp = null;


        if (missoesPrincipais_Cumprir.Count <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void VerificarMissao(Missao _missao, Player player)
    {
        foreach (var item in missaoPrincipais)
        {
            if(_missao.GetIdMissao == item.GetIdMissao)
            {
                if(_missao.GetConcluida)
                {
                    // olha so vc ja councluiu a missão muito obrigado
                    Debug.Log("olha so vc ja councluiu a missão muito obrigado");
                    break;
                }
                else if (_missao is Missao_ColetarItem)
                {
                    var _missao_Item = _missao as Missao_ColetarItem;

                    if(VerificarItem(_missao_Item.GetItemDeMissao,player))
                    {
                        // obrigado por entregar os itens aqui sua recompensa
                        Debug.Log("obrigado por entregar os itens aqui sua recompensa");
                        break;
                    }
                    else
                    {
                        if(player.InventarioMissao.ProcurarQuantidadeItem(_missao_Item.GetItemDeMissao) > 0)
                        {
                            //esse iten agora eu preciso de mais dele
                            Debug.Log("esse iten agora eu preciso de mais dele");
                            break;
                        }
                        else
                        {
                            //voce precisa coletar os itens
                            Debug.Log("voce precisa coletar os itens");
                            break;
                        }
                    }
                }
            }
        }
    }
    private static void VerificarMissaoCompleta(List<Missao> _listaMissoes, List<Missao> _listaMissoes_Cumprir, List<Missao> _listaMissoes_temp, Player player)
    {
        foreach (Missao item in _listaMissoes)
        {
            if (item.GetEstado != Missao.Estado.Concluida)
            {
                _listaMissoes_Cumprir.Add(item);
            }
        }
        foreach (var item in _listaMissoes_Cumprir)
        {
            if (item is Missao_ColetarItem)
            {
                Missao_ColetarItem _item_temp = item as Missao_ColetarItem;
                if (VerificarItem(_item_temp.GetItemDeMissao, player))
                {
                    _listaMissoes_temp.Add(item);
                }
            }
        }
        foreach (var item in _listaMissoes_temp)
        {
            _listaMissoes_Cumprir.Remove(item);
        }
        _listaMissoes_temp.Clear();
    }

    public static bool VerificarItem(Item item,Player player)
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
            if(missaoTemp.GetItemDeMissao == item)
            {
                if(player.InventarioMissao.ProcurarQuantidadeItem(item) < missaoTemp.GetquantidadeItensCompletarMissao)
                {
                    //AtualizaHud
                    //Debug.Log("Falta macas ainda");
                    return false;
                }
                else
                {
                    //Debug.Log("Coletaste todas as macas");

                    //AtualizaHud
                    //Ativar Flag Missao Completa
                    return true;
                }
            }
        }
        return false;
    }
}
