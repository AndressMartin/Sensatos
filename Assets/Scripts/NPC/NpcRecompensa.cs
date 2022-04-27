using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRecompensa : MonoBehaviour
{
    NPC npc;
    [SerializeField] private List<Recompensa> itemRecompensaList;

    private void Start()
    {
        npc = GetComponentInParent<NPC>();
    }
    public void PassarItemPlayer()
    {
        foreach (var recompensa in itemRecompensaList)
        {
            if (recompensa.GetItem is ItemChave chave)
            {
                for (int i = 0; i < recompensa.GetQuantidade; i++)
                {
                    npc.GetGeneralManager.Player.InventarioMissao.AdicionarItem(chave);
                }
            }
            else
            {
                for (int i = 0; i < recompensa.GetQuantidade; i++)
                {
                    npc.GetGeneralManager.Player.Inventario.AdicionarItem(recompensa.GetItem);
                }
            }
        } 
    }
    [Serializable]
    public class Recompensa
    {
        [SerializeField] private Item itemRecompensa;
        [SerializeField] private int quantidadeitem;

        public Item GetItem => itemRecompensa;
        public int GetQuantidade => quantidadeitem;
    }
}
