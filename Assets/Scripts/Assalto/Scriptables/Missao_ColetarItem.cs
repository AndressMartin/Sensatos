using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missao/Missao de Coletar Item")]

public class Missao_ColetarItem : Missao
{
    [SerializeField] private ItemChave itemColetavel;
    [SerializeField] private int quantidadeItensCompletarMissao;
    public ItemChave GetItemDeMissao => itemColetavel;
    public int GetquantidadeItensCompletarMissao => quantidadeItensCompletarMissao;
}
