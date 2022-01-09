using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedWeaponsHUD : UIScreen
{
    [Header("Lista de apenas dois elementos")]
    [SerializeField] private List<WeaponFrame> equipedWeaponFrames = new List<WeaponFrame>();
    public Inventario inventario;
    public void Init()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
        InventoryHudController.invOpen.AddListener(SortWeapons);
        SortWeapons();
        inventario.sortedWeapons.AddListener(SortWeapons);
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
        equipedWeaponFrames[0].Init(inventario.armaSlot1);
        equipedWeaponFrames[1].Init(inventario.armaSlot2);
    }

}
