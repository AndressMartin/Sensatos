using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHUD : UIScreen
{
    [Header("Lista de apenas dois elementos")]
    [SerializeField] private List<WeaponFrame> equipedWeaponFrames = new List<WeaponFrame>();
    [SerializeField] private List<ItemFrame> inventoryItens = new List<ItemFrame>();
    [SerializeField] private List<ItemFrame> atalhoItens = new List<ItemFrame>();
    [SerializeField] private GameObject itensGridHolder;
    public Inventario inventario;
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
        for (int i = 0; i < inventoryItens.Count; i++)
        {
            if(i < inventario.Itens.Count)
                inventoryItens[i].Init(inventario.Itens[i]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SortWeapons();
        //}
    }

    public void SortWeapons()
    {
        equipedWeaponFrames[0].Init(inventario.ArmaSlot1);
        equipedWeaponFrames[1].Init(inventario.ArmaSlot2);
    }

}
