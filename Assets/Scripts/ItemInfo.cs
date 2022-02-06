using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : InfoScreen
{
    [SerializeField] private OptionsPanel optionsPanel;
    // Start is called before the first frame update
    void Start()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
    }
    public override void OpenScreen()
    {
        base.OpenScreen();
        if (transform.GetChild(0).gameObject.activeSelf) UpdateExplainPanel(objectThatCalled);
        optionsPanel.ChangePositionRelativeToUIElement(objectThatCalled.GetComponent<RectTransform>());
        explainPanel.CheckIfFlip(objectThatCalled.GetComponent<RectTransform>());
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.GetChild(0).gameObject.activeSelf);
        if (!transform.GetChild(0).gameObject.activeSelf) return;
        if (Input.GetAxisRaw("Vertical") == 0) return;
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
        CloseScreen();
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
