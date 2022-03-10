using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryHudController : UIScreen, IInventory
{
    bool inventoryOpen = false;
    public static UnityEvent invOpen;
    public static UnityEvent invClose;
    private InventoryHUD inventoryHUD;
    private Selection selection;

    // Start is called before the first frame update
    void Start()
    {
        inventoryHUD = transform.GetChild(0).GetComponent<InventoryHUD>();
        selection = transform.GetChild(0).GetComponent<Selection>();
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
            if (!inventoryOpen)
            {
                OpenScreen();
                selection.EnableInfoScreens();
            }
            else
            {
                CloseScreen();
                selection.DisableInfoScreens();
            }
            invClose.RemoveListener(CloseScreen);
        }
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
        invOpen.Invoke();
        inventoryHUD.Init();
    }

    public override void CloseScreen()
    {
        base.CloseScreen();
        selection.beganSelection = false;
        selection.onSubScreen = false;
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
