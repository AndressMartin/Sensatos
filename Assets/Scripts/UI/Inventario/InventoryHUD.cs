using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHUD : MonoBehaviour
{
    [Header("Lista de apenas dois elementos")]
    [SerializeField] private List<WeaponFrame> equipedWeaponFrames = new List<WeaponFrame>();
    [SerializeField] private List<ItemFrame> inventoryItens = new List<ItemFrame>();
    //[SerializeField] private List<ItemFrame> atalhoItens = new List<ItemFrame>();
    [SerializeField] private GameObject itensGridHolder;
    public Inventario inventario;
    //TODO: Terrible way to make this. Remove this bool and make a safer method to insert itens
    public bool colocouTodosOsItens = false;
    public void Init()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
        InventoryHudController.invOpen.AddListener(SortWeapons);
        SortWeapons();
        inventario.hasSortedWeapons.AddListener(SortWeapons);

        foreach (Transform itemSlot in itensGridHolder.transform)
        {
            inventoryItens.Add(itemSlot.GetComponent<ItemFrame>());
        }
        if (!colocouTodosOsItens)
        {
            for (int i = 0; i < inventoryItens.Count; i++)
            {
                if (i < inventario.Itens.Count)
                    inventoryItens[i].Init(inventario.Itens[i]);
            }
            colocouTodosOsItens = true;
        }
        
    }

    public void SortWeapons()
    {
        equipedWeaponFrames[0].Init(inventario.ArmaSlot1);
        equipedWeaponFrames[1].Init(inventario.ArmaSlot2);
    }

}
