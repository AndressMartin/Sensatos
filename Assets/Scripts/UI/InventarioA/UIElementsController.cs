using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElementsController : MonoBehaviour
{
    public List<Button> elements;
    public List<Navigation> elementsNavigation;
    public UiNavigationState state = UiNavigationState.Normal;

    public void SaveElements()
    {
        foreach (Transform child in transform.GetChild(0))
        {
            elements.Add(child.GetComponent<Button>());
        }
    }

    public void SaveButtonsNavigation()
    {
        foreach (var atalho in elements)
        {
            elementsNavigation.Add(atalho.navigation);
        }
    }

    public void NormalizeNavigation()
    {
        Debug.Log("normalize");
        if (state == UiNavigationState.Normal) return;
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].navigation = elementsNavigation[i];
        }
        state = UiNavigationState.Normal;
        Debug.Log("finished normalize");
    }
}

public enum UiNavigationState
{
    Normal,
    Altered
}
