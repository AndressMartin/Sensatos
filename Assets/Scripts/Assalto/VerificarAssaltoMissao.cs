using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VerificarAssaltoMissao
{
    [SerializeField] private static string nomeAssalto;


    [SerializeField] private static List<Missao> missaoPrincipais = new List<Missao>();
    [SerializeField] private static List<Missao> missaoSecundaria = new List<Missao>();


    [SerializeField] private static List<Missao> missoesPrincipais_Cumprir = new List<Missao>();
    [SerializeField] private static List<Missao> missoesSecundarias_Cumprir = new List<Missao>();

    public static void SetarAssalto(Assalto _assalto)
    {
        //desativo as missoes
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
    public static void VerificarMissao(Missao _missao, Player player,NpcMissao npcMissao)
    {
        foreach (var missaoPr in missaoPrincipais)
        {      
            if (_missao.GetId == missaoPr.GetId)
            {
                if (_missao.GetEstado == Missoes.Estado.Concluida)//Verifica se a missao ja foi concluida
                {
                    // olha so vc ja councluiu a missão muito obrigado
                    Debug.Log("olha so vc ja councluiu a missão muito obrigado");
                    break;
                }
                else if (_missao is Missao_ColetarItem)//conclui a missao, passa a recompensa e muda o estado
                {
                    var _missao_Item = _missao as Missao_ColetarItem;

                    if (VerificarItem(_missao_Item.GetItemDeMissao, player))
                    {
                        if (_missao.GetEstado == Missoes.Estado.Ativa)
                        {
                            _missao_Item.SetarFlag(true);
                            npcMissao.TrocarDialogoMissao(_missao);
                            // obrigado por entregar os itens aqui sua recompensa
                            if (_missao_Item.GetRecompensa)
                            {
                                //Colocar evento recompensa,
                                switch (_missao_Item.GetTipoRecompensa)
                                {
                                    case Missao.Recompensa.item:
                                        _missao_Item.PlayerRecerberRecompensaItem(player);
                                        break;
                                    case Missao.Recompensa.evento:
                                        _missao_Item.PlayerRecerberRecompensaEvento(player);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            npcMissao.TrocarDialogoMissao(_missao);
                            _missao.SetEstado(Missoes.Estado.Ativa);
                        }
                        break;
                    }
                    else
                    {
                        if(_missao.GetEstado == Missoes.Estado.Inativa)
                        {
                            //esse iten agora eu preciso de mais dele
                            Debug.Log("Inativo");
                            npcMissao.TrocarDialogoMissao(_missao);
                            _missao.SetEstado(Missoes.Estado.Ativa);
                            break;
                        }
                        else if (_missao.GetEstado == Missoes.Estado.Ativa)
                        {
                            //voce precisa coletar os itens
                            Debug.Log("Ativo");
                            npcMissao.TrocarDialogoMissao(_missao);
                            break;
                        }
                        /*if (player.InventarioMissao.ProcurarQuantidadeItem(_missao_Item.GetItemDeMissao) > 0)
                        {
                            npcMissao.TrocarDialogo(_missao);
                            break;
                        }
                        else
                        {

                            npcMissao.TrocarDialogo(_missao);
                            _missao.SetEstado(Missoes.Estado.Ativa);
                            break;
                        }*/
                    }
                }
            }
        }
    }
    private static void VerificarMissaoCompleta(List<Missao> _listaMissoes, List<Missao> _listaMissoes_Cumprir, List<Missao> _listaMissoes_temp, Player player)
    {
        foreach (Missao item in _listaMissoes)
        {
            if (item.GetEstado != Missoes.Estado.Concluida)
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

    public static bool VerificarItem(ItemChave item,Player player)
    {
        Missao_ColetarItem missaoTemp=null;
        foreach (var ms in missaoPrincipais)
        {
            if((Missao_ColetarItem)ms)
            {
                missaoTemp = (Missao_ColetarItem)ms;
                break;
            }
        }
        foreach (var ms in missaoSecundaria)
        {
            if ((Missao_ColetarItem)ms)
            {
                missaoTemp = (Missao_ColetarItem)ms;
                break;
            }
        }

        if(missaoTemp != null)
        {
            if(missaoTemp.GetItemDeMissao == item)
            {
                if(player.InventarioMissao.ProcurarQuantidadeItem(item) < missaoTemp.GetquantidadeItensCompletarMissao)
                {
                    //AtualizaHud
                    return false;
                }
                else
                {
                    //AtualizaHud
                    //Ativar Flag Missao Completa
                    return true;
                }
            }
        }
        return false;
    }
}
