using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponInfo : InfoScreen
{
    public GameObject weaponsPanel;
    public GameObject weaponPanelPrefab;
    private UIScrollToSelection UIKeyboardScroller;
    void Start()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
        if (UIKeyboardScroller == null) UIKeyboardScroller = GetComponent<UIScrollToSelection>();
        //Add o item que já vem com o Canvas para a primeira seleção.
        elementsButtons.Add(weaponsPanel.transform.GetChild(0).GetComponent<Button>());
        for (int i = 1; i < inventario.Armas.Count; i++)
        {
            //Skippa o primeiro item. 
            var newPanel = Instantiate(weaponPanelPrefab, weaponsPanel.transform);
            newPanel.name += i;
            var newPanelButton = newPanel.GetComponent<Button>();
            elementsButtons.Add(newPanelButton);
            newPanelButton.onClick.AddListener(delegate { SortInventoryWeapons(newPanelButton); });
        }
        //Seta spacing e size para o painel da arma
        weaponsPanel.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(weaponsPanel.GetComponent<RectTransform>().sizeDelta.x,
            weaponsPanel.transform.GetChild(0).GetComponent<RectTransform>().rect.height 
            * elementsButtons.Count 
            + weaponsPanel.GetComponent<VerticalLayoutGroup>().spacing * elementsButtons.Count-1);
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
        SetWeaponSlots();
        SortListAndNav();
        if (transform.GetChild(0).gameObject.activeSelf) UpdateExplainPanel(EventSystem.current.currentSelectedGameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (!transform.GetChild(0).gameObject.activeSelf) return;
        if (Input.GetAxisRaw("Vertical") == 0) return;
        currentSelected = EventSystem.current.currentSelectedGameObject;
        UpdateExplainPanel(EventSystem.current.currentSelectedGameObject);
        foreach (var btn in elementsButtons)
        {
            if (currentSelected.name == btn.name)
            {
                UIKeyboardScroller.SnapTo(btn.GetComponent<RectTransform>());
                Debug.LogWarning("Selected: " + btn.name);
            }
        }
    }

    public void SortInventoryWeapons(Button panel)
    {
        /*
        //Debug.LogWarning($"TRYING TO SWITCH {elementsButtons.IndexOf(panel)} & {objectThatCalled.GetComponent<WeaponFrame>().GetSavedElement().index}");
        ArmaDeFogo clickedWeapon = elementsButtons[elementsButtons.IndexOf(panel)].gameObject.GetComponent<WeaponFrame>().GetSavedElement() as ArmaDeFogo;
        if (clickedWeapon.index == 0 || clickedWeapon.index == 1)
            return;
        inventario.TrocarArmaDoInventario(clickedWeapon, objectThatCalled);
        CloseScreen();
        */
    }

    private void SetWeaponSlots()
    {
        /*
        for (int i = 0; i < inventario.Armas.Count; i++)
        {
            if(elementsButtons.Count >= i)
            {
                elementsButtons[i].GetComponent<WeaponFrame>().Init(inventario.Armas[i]);
                elementsButtons[i].transform.SetSiblingIndex(inventario.Armas[i].index);
                if (objectThatCalled.transform.GetSiblingIndex() == inventario.Armas[i].index)
                {
                    elementsButtons[i].GetComponent<WeaponFrame>().EquipedColor();
                }
            }
        }
        */
    }

    void SortListAndNav()
    {
        Navigation customNavFirstItem = new Navigation();
        customNavFirstItem.mode = Navigation.Mode.Explicit;
        customNavFirstItem.selectOnUp = elementsButtons[elementsButtons.Count - 1];
        customNavFirstItem.selectOnDown = elementsButtons[1];
        elementsButtons[0].navigation = customNavFirstItem;

        Navigation customNavLastItem = new Navigation();
        customNavLastItem.mode = Navigation.Mode.Explicit;
        customNavLastItem.selectOnDown = elementsButtons[0];
        customNavLastItem.selectOnUp = elementsButtons[elementsButtons.Count - 2];
        elementsButtons[elementsButtons.Count - 1].navigation = customNavLastItem;

        for (int i = 1; i < elementsButtons.Count-1; i++)
        {
            Navigation customNav = elementsButtons[i].navigation;
            customNav.selectOnUp = elementsButtons[i - 1];
            customNav.selectOnDown = elementsButtons[i + 1];
            elementsButtons[i].navigation = customNav;
        }
    }
}
