using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponInfo : UIScreen
{
    public List<Button> weaponButtons = new List<Button>();
    public GameObject weaponsPanel;
    public GameObject weaponPanelPrefab;
    private UIScrollToSelection UIKeyboardScroller;
    [SerializeField] private Inventario inventario;
    // Start is called before the first frame update
    void Start()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
        if (UIKeyboardScroller == null) UIKeyboardScroller = GetComponent<UIScrollToSelection>();
        //Add o item que já vem com o Canvas para a primeira seleção.
        weaponButtons.Add(weaponsPanel.transform.GetChild(0).GetComponent<Button>());
        for (int i = 1; i < inventario.armas.Count; i++)
        {
            //Skippa o primeiro item. 
            var newPanel = Instantiate(weaponPanelPrefab, weaponsPanel.transform);
            newPanel.name += i;
            var newPanelButton = newPanel.GetComponent<Button>();
            weaponButtons.Add(newPanelButton);
            newPanelButton.onClick.AddListener(delegate { SortInventoryWeapons(newPanelButton); });
        }
        //Sets the spacing and size for the panel that holds the weapons
        weaponsPanel.GetComponent<RectTransform>().sizeDelta = 
            new Vector2(weaponsPanel.GetComponent<RectTransform>().sizeDelta.x,
            weaponsPanel.transform.GetChild(0).GetComponent<RectTransform>().rect.height 
            * weaponButtons.Count 
            + weaponsPanel.GetComponent<VerticalLayoutGroup>().spacing * weaponButtons.Count-1);

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
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            if (Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                var currentSelected = EventSystem.current.currentSelectedGameObject;

                foreach (var btn in weaponButtons)
                {
                    if (currentSelected.name == btn.name)
                    {
                        UIKeyboardScroller.SnapTo(btn.GetComponent<RectTransform>());
                        Debug.LogWarning("Selected: " + btn.name);
                    }
                }
            }
        }
    }

    public void SortInventoryWeapons(Button panel)
    {
        Debug.LogWarning($"TRYING TO SWITCH {weaponButtons.IndexOf(panel)} & {objectThatCalled.GetComponent<WeaponFrame>().GetSavedWeapon().index}");
        ArmaDeFogo clickedWeapon = weaponButtons[weaponButtons.IndexOf(panel)].gameObject.GetComponent<WeaponFrame>().GetSavedWeapon();
        if (clickedWeapon.index == 0 || clickedWeapon.index == 1)
            return;
        inventario.TrocarArmaDoInventario(clickedWeapon, objectThatCalled);
        CloseScreen();
    }

    private void SetWeaponSlots()
    {
        for (int i = 0; i < inventario.armas.Count; i++)
        {
            if(weaponButtons.Count >= i)
            {
                weaponButtons[i].GetComponent<WeaponFrame>().Init(inventario.armas[i]);
                weaponButtons[i].transform.SetSiblingIndex(inventario.armas[i].index);
                if (objectThatCalled.transform.GetSiblingIndex() == inventario.armas[i].index)
                {
                    weaponButtons[i].GetComponent<WeaponFrame>().EquipedColor();
                }
            }
        }
    }

    void SortListAndNav()
    {
        Navigation customNavFirstItem = new Navigation();
        customNavFirstItem.mode = Navigation.Mode.Explicit;
        customNavFirstItem.selectOnUp = weaponButtons[weaponButtons.Count - 1];
        customNavFirstItem.selectOnDown = weaponButtons[1];
        weaponButtons[0].navigation = customNavFirstItem;

        Navigation customNavLastItem = new Navigation();
        customNavLastItem.mode = Navigation.Mode.Explicit;
        customNavLastItem.selectOnDown = weaponButtons[0];
        customNavLastItem.selectOnUp = weaponButtons[weaponButtons.Count - 2];
        weaponButtons[weaponButtons.Count - 1].navigation = customNavLastItem;

        for (int i = 1; i < weaponButtons.Count-1; i++)
        {
            Navigation customNav = weaponButtons[i].navigation;
            customNav.selectOnUp = weaponButtons[i - 1];
            customNav.selectOnDown = weaponButtons[i + 1];
            weaponButtons[i].navigation = customNav;
        }
    }
}
