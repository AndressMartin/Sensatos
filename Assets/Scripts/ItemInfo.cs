using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : InfoScreen
{
    [SerializeField] private OptionsPanel optionsPanel;
    [SerializeField] private AtalhosController atalhosController;
    [SerializeField] private ItemInfoState state;
    // Start is called before the first frame update
    void Start()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
        state = ItemInfoState.Normal;
    }
    public override void OpenScreen()
    {
        //Don't allow opening the screen if there isn't any item in the ItemFrame
        if (EventSystem.current.currentSelectedGameObject.GetComponent<ItemFrame>().GetSavedElement() == null)
        {
            CloseScreen();
            Selection.GetInstance().SetEventSysFirstButton(objectThatCalled);
            return;
        }
        if (state != ItemInfoState.Normal) return;
        base.OpenScreen();
        if (transform.GetChild(0).gameObject.activeSelf) UpdateExplainPanel(objectThatCalled);
        optionsPanel.ChangePositionRelativeToUIElement(objectThatCalled.GetComponent<RectTransform>());
        explainPanel.CheckIfFlip(objectThatCalled.GetComponent<RectTransform>());
    }
    // Update is called once per frame
    void Update()
    {
        if (!transform.GetChild(0).gameObject.activeSelf) return;
        if (Input.GetAxisRaw("Vertical") == 0) return;
        if (state != ItemInfoState.Normal) return;
        UpdateExplainPanel(objectThatCalled);
    }

    public void Usar()
    {
        Debug.Log("Usar.");
        //var item = objectThatCalled.GetComponent<UIFrame>().GetSavedElement() as Item;
        //item.Usar(/*Inserir player*/);
        CloseScreen();
    }

    public void AdicionarAosAtalhos()
    {
        Debug.Log("AdicionarAosAtalhos.");
        state = ItemInfoState.SetandoAtalho;
        transform.GetChild(0).gameObject.SetActive(false);
        Selection.GetInstance().SetEventSysFirstButton(atalhosController.atalhos[0].gameObject);
        atalhosController.AlterNavigation();
        foreach (Button atalho in atalhosController.atalhos)
        {
            //TODO: You removed the important listeners. Now you need to restore it.
            atalho.onClick.RemoveAllListeners();
            atalho.onClick.AddListener(delegate { SetarItemFrame(atalho); });
        }
    }

    public override void CloseScreen()
    {
        if (state != ItemInfoState.Normal) return;
        base.CloseScreen();
        atalhosController?.NormalizeNavigation();
    }

    public void SetarItemFrame(Button btn)
    {
        Debug.LogWarning("SetarItemFrame for " + btn.name);
        //Item frame sets a reference to its shortcut, and warns the shortcut to store a reference to the itemFrame
        objectThatCalled.GetComponent<ItemFrame>().AtalhoFrame = btn.GetComponent<AtalhoFrame>();
        //btn.GetComponent<AtalhoFrame>().Init(objectThatCalled.GetComponent<ItemFrame>().GetSavedElement());
        transform.GetChild(0).gameObject.SetActive(true);
        Selection.GetInstance().SetEventSysFirstButton(FirstSelection.gameObject);
        atalhosController.NormalizeNavigation();
        optionsPanel.ChangePositionRelativeToUIElement(objectThatCalled.GetComponent<RectTransform>());
        explainPanel.CheckIfFlip(objectThatCalled.GetComponent<RectTransform>());
        state = ItemInfoState.Normal;
        //TODO: You removed the important listeners. Now you need to restore it.
    }

    public void AlterarPosicao()
    {
        Debug.Log("AlterarPosicao.");
        CloseScreen();
    }

    public void JogarFora()
    {
        Debug.Log("JogarFora.");
        var item = objectThatCalled.GetComponent<UIFrame>().GetSavedElement() as Item;
        item.JogarFora();
        CloseScreen();
    }
}

public enum ItemInfoState 
{ 
    Normal,
    SetandoAtalho,
    AlterandoPos
}

