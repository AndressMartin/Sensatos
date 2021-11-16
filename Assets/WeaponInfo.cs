using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfo : UIScreen
{
    public List<Button> armasButtons = new List<Button>();
    public GameObject weaponsPanel;
    [SerializeField] private Inventario inventario;
    // Start is called before the first frame update
    void Start()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
        foreach (Transform item in weaponsPanel.transform)
        {
            armasButtons.Add(item.GetComponent<Button>());
        }
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
        inventario.ReSort();
        SetWeaponSlots();
        SortListAndNav();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetWeaponSlots()
    {
        for (int i = 0; i < inventario.armas.Count; i++)
        {
            if(armasButtons.Count >= i)
            {
                armasButtons[i].GetComponent<WeaponFrame>().Init(inventario.armas[i]);
                armasButtons[i].transform.SetSiblingIndex(inventario.armas[i].index);
                if (objectThatCalled.transform.GetSiblingIndex() == inventario.armas[i].index)
                {
                    armasButtons[i].GetComponent<WeaponFrame>().EquipedColor();
                }
            }
        }
    }

    void SortListAndNav()
    {
        Navigation customNavFirstItem = new Navigation();
        customNavFirstItem.mode = Navigation.Mode.Explicit;
        customNavFirstItem.selectOnUp = armasButtons[armasButtons.Count - 1];
        customNavFirstItem.selectOnDown = armasButtons[1];
        armasButtons[0].navigation = customNavFirstItem;

        Navigation customNavLastItem = new Navigation();
        customNavLastItem.mode = Navigation.Mode.Explicit;
        customNavLastItem.selectOnDown = armasButtons[0];
        customNavLastItem.selectOnUp = armasButtons[armasButtons.Count - 2];
        armasButtons[armasButtons.Count - 1].navigation = customNavLastItem;

        for (int i = 1; i < armasButtons.Count-1; i++)
        {
            Navigation customNav = armasButtons[i].navigation;
            customNav.selectOnUp = armasButtons[i - 1];
            customNav.selectOnDown = armasButtons[i + 1];
            armasButtons[i].navigation = customNav;
        }
    }
}
