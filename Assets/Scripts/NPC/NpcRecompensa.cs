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
                if (recompensa.GetItem != null)
                {
                    for (int i = 0; i < recompensa.GetQuantidade; i++)
                    {
                        npc.GetGeneralManager.Player.InventarioMissao.AdicionarItem(chave);
                    }
                }
            }
            else
            {
                if (recompensa.GetArma != null)
                {
                    for (int i = 0; i < recompensa.GetQuantidade; i++)
                    {
                        npc.GetGeneralManager.Player.Inventario.AdicionarArma(recompensa.GetArma);
                    }
                }
                if (recompensa.GetItem != null)
                {
                    for (int i = 0; i < recompensa.GetQuantidade; i++)
                    {
                        npc.GetGeneralManager.Player.Inventario.AdicionarItem(recompensa.GetItem);
                    }
                }
                if (recompensa.GetRoupa != null)
                {
                    for (int i = 0; i < recompensa.GetQuantidade; i++)
                    {
                        npc.GetGeneralManager.Player.Inventario.AdicionarRoupa(recompensa.GetRoupa);
                    }
                }
            }
        }
    }
    [Serializable]
    public class Recompensa
    {
        [SerializeField] private ArmaDeFogo arma;

        [SerializeField] private Item itemRecompensa;
        [SerializeField] private RoupaDeCamuflagem roupaDeCamuflagem;

        [SerializeField] private int quantidadeitem;

        public Item GetItem => itemRecompensa;
        public RoupaDeCamuflagem GetRoupa => roupaDeCamuflagem;
        public ArmaDeFogo GetArma => arma;


        public int GetQuantidade => quantidadeitem;
    }
}
