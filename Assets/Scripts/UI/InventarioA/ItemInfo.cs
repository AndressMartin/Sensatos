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
    [SerializeField] private ItensController itensController;
    [SerializeField] private ItemInfoState state;
    // Start is called before the first frame update
    void Start()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
        state = ItemInfoState.Normal;
    }
    // Update is called once per frame
    void Update()
    {
        if (!transform.GetChild(0).gameObject.activeSelf) return;
        if (Input.GetAxisRaw("Vertical") == 0) return;
        if (state != ItemInfoState.Normal) return;
        UpdateExplainPanel(objectThatCalled);
    }

    #region Open & Close
    public override void OpenScreen()
    {
        //Don't allow opening the screen if there isn't any item in the ItemFrame
        if (objectThatCalled.GetComponent<ItemFrame>()?.GetSavedElement())
        {
            screenState = UIScreenState.Functional;
        }
        else if (objectThatCalled.GetComponent<AtalhoFrame>()?.Referencia)
        {
            screenState = UIScreenState.Functional;
            //Do Stuff for Atalho
        }
        else
        {
            CloseScreen();
            screenState = UIScreenState.NonFunctional;
            return;
        }
        if (state != ItemInfoState.Normal) return;
        base.OpenScreen();
        if (transform.GetChild(0).gameObject.activeSelf) UpdateExplainPanel(objectThatCalled);
        optionsPanel.ChangePositionRelativeToUIElement(objectThatCalled.GetComponent<RectTransform>());
        explainPanel.CheckIfFlip(objectThatCalled.GetComponent<RectTransform>());
    }
    public override void CloseScreen()
    {
        base.CloseScreen();
        state = ItemInfoState.Normal;
        atalhosController?.NormalizeNavigation();
    }

    #endregion

    #region opcoes do menu

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
        Selection.GetInstance().SetEventSysFirstButton(atalhosController.elements[0].gameObject);
        atalhosController.AlterNavigation();
        foreach (Button atalho in atalhosController.elements)
        {
            //TODO: You removed the important listeners. Now you need to restore it.
            atalho.onClick.RemoveAllListeners();
            atalho.onClick.AddListener(delegate { SetAtalho(atalho); });
        }
    }
    public void SetAtalho(Button btn)
    {
        Debug.Log(objectThatCalled.GetComponent<AtalhoFrame>());
        Debug.LogWarning("SetarItemFrame for " + btn.name);
        //Está na parte de inventário
        //if (!objectThatCalled.GetComponent<AtalhoFrame>())
        //{
            //TODO: 
        //}
        //Está na parte de atalhos
        //Item frame sets a reference to its shortcut, and warns the shortcut to store a reference to the itemFrame
        objectThatCalled.GetComponent<ItemFrame>().AtalhoFrame = btn.GetComponent<AtalhoFrame>();
        transform.GetChild(0).gameObject.SetActive(true);
        Selection.GetInstance().SetEventSysFirstButton(FirstSelection.gameObject);
        atalhosController.NormalizeNavigation();
        optionsPanel.ChangePositionRelativeToUIElement(objectThatCalled.GetComponent<RectTransform>());
        explainPanel.CheckIfFlip(objectThatCalled.GetComponent<RectTransform>());
        state = ItemInfoState.Normal;
        NormalizeControllerOnClicks(atalhosController);
        //TODO: You removed the important listeners. Now you need to restore it.
    }

    public void NormalizeControllerOnClicks(UIElementsController controller)
    {
        var selection = Selection.GetInstance();
        foreach (var btn in controller.elements)
        {
            btn.onClick.RemoveAllListeners();
            selection.ReturnToDefaultButtonFunc(btn.GetComponent<UIFrame>());
        }
    }
    public void AlterarPosicao()
    {
        Debug.Log("AlterarPosicao.");
        state = ItemInfoState.AlterandoPos;
        transform.GetChild(0).gameObject.SetActive(false);
        var item = objectThatCalled.GetComponent<ItemFrame>();
        Selection.GetInstance().SetEventSysFirstButton(item.gameObject);
        itensController.AlterNavigation();
        foreach (Button element in itensController.elements)
        {
            //TODO: You removed the important listeners. Now you need to restore it.
            element.onClick.RemoveAllListeners();
            element.onClick.AddListener(delegate { SetItemFrame(element); });
        }
    }

    public void SetItemFrame(Button btn)
    {
        btn.GetComponent<ItemFrame>().Init(objectThatCalled.GetComponent<ItemFrame>().GetSavedElement());
        var atalho = objectThatCalled.GetComponent<ItemFrame>().AtalhoFrame;
        objectThatCalled.GetComponent<ItemFrame>().RemoveElement();
        if (atalho) btn.GetComponent<ItemFrame>().AtalhoFrame = atalho;
        transform.GetChild(0).gameObject.SetActive(true);
        Selection.GetInstance().SetEventSysFirstButton(FirstSelection.gameObject);
        itensController.NormalizeNavigation();
        optionsPanel.ChangePositionRelativeToUIElement(objectThatCalled.GetComponent<RectTransform>());
        explainPanel.CheckIfFlip(objectThatCalled.GetComponent<RectTransform>());
        state = ItemInfoState.Normal;
        NormalizeControllerOnClicks(itensController);
        //TODO: You removed the important listeners. Now you need to restore it.
    }

    public void JogarFora()
    {
        Debug.Log("JogarFora.");
        var frame = objectThatCalled.GetComponent<ItemFrame>();
        var item = frame.GetSavedElement() as Item;
        //item.JogarFora();
        frame.RemoveElement(); 
        CloseScreen();
    }
    #endregion

}

public enum ItemInfoState 
{ 
    Normal,
    SetandoAtalho,
    AlterandoPos
}

