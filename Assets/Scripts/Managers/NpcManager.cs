using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private List<Npc> npcs = new List<Npc>();
    
    public List<Npc> GetNpcs => npcs;

    public void AddList(Npc _npc)
    {
        npcs.Add(_npc);
    }
    public void RemoveList(Npc _npc)
    {
        npcs.Remove(_npc);
    }
    public void PassarAssalto(Assalto _assalto)
    {
        foreach (var item in npcs)
        {
            item.ReceberAssaltoDoManager(_assalto);
        }
    }
}
