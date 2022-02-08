using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtalhosController : MonoBehaviour
{
    public List<Button> atalhos;
    public List<Navigation> atalhosNavigation;
    public UiNavigationState state = UiNavigationState.Normal;
    // Start is called before the first frame update
    void Start()
    {
        SaveAtalhos();
        SaveButtonsNavigation();
    }

    private void OnDestroy()
    {
        
    }

    private void SaveAtalhos()
    {
        foreach (Transform child in transform.GetChild(0))
        {
            atalhos.Add(child.GetComponent<Button>());
        }
    }

    private void SaveButtonsNavigation()
    {
        foreach (var atalho in atalhos)
        {
            atalhosNavigation.Add(atalho.navigation);
        }
    }

    public void AlterNavigation()
    {
        if (state == UiNavigationState.Altered) 
        { 
            Debug.LogError("Double navigation alteration."); 
            return; 
        }
        for (int i = 0; i < 4; i++)
        {
            var nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;
            if (i < 2)
            {
                nav.selectOnLeft = atalhos[(i + 1) % 2];
                nav.selectOnRight = atalhos[(i + 1) % 2];
            }
            else if (i == 2)
            {
                nav.selectOnLeft = atalhos[(i + 1) % 4];
                nav.selectOnRight = atalhos[(i + 1) % 4];
            }
            else
            {
                nav.selectOnLeft = atalhos[(i + 1) -2];
                nav.selectOnRight = atalhos[(i + 1) -2];
            }
            
            nav.selectOnUp = atalhos[(i + 2) % 4];
            nav.selectOnDown = atalhos[(i + 2) % 4];
            atalhos[i].navigation = nav;
        }
        state = UiNavigationState.Altered;
    }

    public void NormalizeNavigation()
    {
        Debug.Log("normalize");
        if (state == UiNavigationState.Normal) return;
        for (int i = 0; i < 4; i++)
        {
            atalhos[i].navigation = atalhosNavigation[i];
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
