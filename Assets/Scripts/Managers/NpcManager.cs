using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private List<NPC> npcs = new List<NPC>();
    
    public List<NPC> GetNpcs => npcs;

    public void AddList(NPC _npc)
    {
        npcs.Add(_npc);
    }
    public void RemoveList(NPC _npc)
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
