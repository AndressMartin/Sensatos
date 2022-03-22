using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missao/Missao_Coletar_Item")]

public class Missao_ColetarItem : Missao
{
    [SerializeField] private ItemColetavel itemColetavels;
    [SerializeField] private int quantidadeItensCompletarMissao;
    public ItemColetavel GetItemDeMissao => itemColetavels;
    public int GetquantidadeItensCompletarMissao => quantidadeItensCompletarMissao;

    void Test()
    {

    }
}
