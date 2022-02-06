using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoScreen : UIScreen
{
    public List<Button> elementsButtons = new List<Button>();
    [SerializeField] protected ExplainPanel explainPanel;
    protected GameObject currentSelected;
    [SerializeField] protected Inventario inventario;

    public override void OpenScreen()
    {
        base.OpenScreen();
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
        inventario.ReSort();
    }

    protected virtual void UpdateExplainPanel(GameObject objToShowExplanation)
    {
        if (objToShowExplanation.GetComponent<UIFrame>().GetSavedElement())
        {
            var selectedElement = objToShowExplanation.GetComponent<UIFrame>().GetSavedElement();
            //Debug.LogWarning($"Changing UI for {selectedElement}");
            explainPanel.ChangeUI(selectedElement);
        }
    }
}
