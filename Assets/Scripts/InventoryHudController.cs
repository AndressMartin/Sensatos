using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryHudController : UIScreen, IInventory
{
    bool inventoryOpen = false;
    public static UnityEvent invOpen;
    public static UnityEvent invClose;
    // Start is called before the first frame update
    void Start()
    {
        if (invOpen == null)
            invOpen = new UnityEvent(); 
        if (invClose == null)
            invClose = new UnityEvent();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        inventoryOpen = transform.GetChild(0).gameObject.activeSelf;

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("T");
            if (!inventoryOpen) OpenScreen();
            else CloseScreen();
            invClose.RemoveListener(CloseScreen);
        }
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
        invOpen.Invoke();
        transform.GetChild(0).GetComponent<EquipedWeaponsHUD>().Init();
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
        transform.GetChild(0).GetComponent<Selection>().beganSelection = false;
        invClose.Invoke();
    }

    public void MakeStacksOfScreens()
    {
        throw new System.NotImplementedException();
    }

    public void FindStacksOfScreens()
    {
        throw new System.NotImplementedException();
    }
}
