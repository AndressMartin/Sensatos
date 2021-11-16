using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    public EventSystem eventSys;
    public Button FirstSelection;
    public List<ScreenOpeningHandle> ScreensToOpen = new List<ScreenOpeningHandle>();
    public UIScreen openedScreen;
    private bool beganSelection;
    public bool onSubScreen;
    void Start()
    {
        foreach (var screen in ScreensToOpen)
        {
            foreach (var button in screen.buttonsThatOpenIt)
            {
                button.onClick.AddListener(delegate { OpenNextScreen(screen.ScreenToOpen); });
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
    }

    private void SetEventSysFirstButton(GameObject obj)
    {
        eventSys.firstSelectedGameObject = obj;
        eventSys.firstSelectedGameObject.GetComponent<Button>().Select();
    }

    void OpenNextScreen(UIScreen screenToOpen)
    {
        foreach (var screen in ScreensToOpen)
        {
            if (screen.ScreenToOpen != null) screen.ScreenToOpen.CloseScreen();
        }
        openedScreen = screenToOpen;
        screenToOpen.OpenScreen();
        onSubScreen = true;
        SetEventSysFirstButton(openedScreen.FirstSelection.gameObject);
    }

    void CloseNextScreen()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (openedScreen && onSubScreen)
            {
                openedScreen.transform.GetChild(0).gameObject.SetActive(false);
                SetEventSysFirstButton(FirstSelection.gameObject);
            }
            else if (!onSubScreen) FindObjectOfType<InventoryHudController>().CloseScreen();
        }
    }
}

[System.Serializable]
public class ScreenOpeningHandle
{
    public UIScreen ScreenToOpen;
    public List<Button> buttonsThatOpenIt;
}

public class MyScreenEvent: UnityEvent<UIScreen>
{

}
