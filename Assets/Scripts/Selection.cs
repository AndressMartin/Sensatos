using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selection : SingletonInstance<Selection>
{
    public EventSystem eventSys;
    public Button FirstSelection;
    public List<ScreenOpeningHandle> ScreensToOpen = new List<ScreenOpeningHandle>();
    public UIScreen openedScreen;
    public bool beganSelection;
    public bool onSubScreen;
    void Start()
    {
        foreach (var screenHandles in ScreensToOpen)
        {
            foreach (var button in screenHandles.buttonsThatOpenIt)
            {
                button.onClick.AddListener(delegate { OpenNextScreen(screenHandles.screenToOpen, button); });
                screenHandles.screenToOpen.buttonsThatOpenMe.Add(button);
            }
        }
    }

    void Update()
    {
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !beganSelection)
        { 
            beganSelection = true;
            Debug.Log(beganSelection);
            SetEventSysFirstButton(FirstSelection.gameObject);
        }
        CloseNextScreen();
        CheckCurrentlySelected();
    }

    private void CheckCurrentlySelected()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.LogWarning(EventSystem.current.currentSelectedGameObject.name);
        }
#endif
    }

    public void SetEventSysFirstButton(GameObject obj)
    {
        eventSys.firstSelectedGameObject = obj;
        eventSys.firstSelectedGameObject.GetComponent<Button>().Select();
    }

    void OpenNextScreen(UIScreen screenToOpen, Button button)
    {
        foreach (var screen in ScreensToOpen)
        {
            if (screen.screenToOpen != null) screen.screenToOpen.CloseScreen();
        }
        openedScreen = screenToOpen;
        screenToOpen.objectThatCalled = button.gameObject;
        screenToOpen.OpenScreen();
        onSubScreen = true;
        SetEventSysFirstButton(openedScreen.FirstSelection.gameObject);
        Debug.LogWarning($"ScreenToOpen: {screenToOpen}, ObjectThatCalled: {screenToOpen.objectThatCalled}, Button: {button}");

    }

    void CloseNextScreen()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (openedScreen && onSubScreen)
            {
                if(openedScreen.objectThatCalled) SetEventSysFirstButton(openedScreen.objectThatCalled);
                else SetEventSysFirstButton(FirstSelection.gameObject);
                onSubScreen = false;
                openedScreen.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if (!onSubScreen)
            {
                FindObjectOfType<InventoryHudController>().CloseScreen();
                beganSelection = false;
            }
        }
    }
}

[System.Serializable]
public class ScreenOpeningHandle
{
    public UIScreen screenToOpen;
    public List<Button> buttonsThatOpenIt;
}

public class MyScreenEvent: UnityEvent<UIScreen>
{

}
