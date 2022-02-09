using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreen : MonoBehaviour
{
    public UIScreenState screenState = UIScreenState.Functional;
    public Button FirstSelection;
    public GameObject objectThatCalled;
    public List<Button> buttonsThatOpenMe;
    // Start is called before the first frame update
    void Start()
    {
        CloseScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Initialize()
    {
        var screens = FindObjectsOfType<UIScreen>();
        foreach (var screen in screens)
        {
            screen.CloseScreen();
            InventoryHudController.invClose.AddListener(screen.CloseScreen);
        }
    }
    //TODO: Make this return a boolean, so you can return false whenever you want to negate open
    public virtual void OpenScreen()
    {
        Debug.LogWarning($"OPEN {name} + {gameObject.name}");
        transform.GetChild(0).gameObject.SetActive(true);
    }

    //TODO: Make this return a boolean, so you can return false whenever you want to negate close
    public virtual void CloseScreen()
    {
        //Debug.LogWarning($"CLOSE {name} + {gameObject.name}");
        transform.GetChild(0).gameObject.SetActive(false);
        if (objectThatCalled) Selection.GetInstance().SetEventSysFirstButton(objectThatCalled);
    }
}

public enum UIScreenState
{
    Functional,
    OnPause,
    NonFunctional
}
