using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtalhosController : UIElementsController
{
    // Start is called before the first frame update
    void Start()
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
            var nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;
            if (i < 2)
            {
                nav.selectOnLeft = elements[(i + 1) % 2];
                nav.selectOnRight = elements[(i + 1) % 2];
            }
            else if (i == 2)
            {
                nav.selectOnLeft = elements[(i + 1) % 4];
                nav.selectOnRight = elements[(i + 1) % 4];
            }
            else
            {
                nav.selectOnLeft = elements[(i + 1) -2];
                nav.selectOnRight = elements[(i + 1) -2];
            }
            
            nav.selectOnUp = elements[(i + 2) % 4];
            nav.selectOnDown = elements[(i + 2) % 4];
            elements[i].navigation = nav;
        }
        state = UiNavigationState.Altered;
    }
}
