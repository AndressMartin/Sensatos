using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRemoverItem : MonoBehaviour
{
    NPC npc;
    [SerializeField] private List<Recompensa> itensParaRemover;

    private void Start()
    {
        npc = GetComponentInParent<NPC>();
    }
    public void RemoverItemPlayer()
    {
        foreach (var recompensa in itensParaRemover)
        {
            if (recompensa.GetItem is ItemChave chave)
            {
                for (int i = 0; i < recompensa.GetQuantidade; i++)
                {
                    if (npc.GetGeneralManager.Player.InventarioMissao.ProcurarQuantidadeItem(chave) > 0)
                    {
                        npc.GetGeneralManager.Player.InventarioMissao.RemoverItem(chave);
                    }
                }
            }
            else
            {
                for (int i = 0; i < recompensa.GetQuantidade; i++)
                {
                    npc.GetGeneralManager.Player.Inventario.RemoverItem(recompensa.GetItem);  
                }
            }
        }
    }
    [System.Serializable]
    public class Recompensa
    {
        [SerializeField] private Item itemRecompensa;
        [SerializeField] private int quantidadeitem;

        public Item GetItem => itemRecompensa;
        public int GetQuantidade => quantidadeitem;
    }
}
