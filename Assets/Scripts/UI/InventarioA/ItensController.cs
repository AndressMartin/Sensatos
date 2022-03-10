using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItensController : UIElementsController
{
    public List<Navigation> closedNavigation;

    private void Start()
    {
        SaveElements();
        SaveButtonsNavigation();
    }
    public void AlterNavigation()
    {
        if (state == UiNavigationState.Altered)
        {
            Debug.LogError("Double navigation alteration.");
            return;
        }
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].navigation = closedNavigation[i];
        }
        state = UiNavigationState.Altered;
    }
}
