using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missao/Missao_Coletar_Item")]

public class Missao_ColetarItem : Missao
{
    [SerializeField] private ItemDeMissao itemColetavels;
    [SerializeField] private int quantidadeItensCompletarMissao;
    public ItemDeMissao GetItemDeMissao => itemColetavels;
    public int GetquantidadeItensCompletarMissao => quantidadeItensCompletarMissao;

}
