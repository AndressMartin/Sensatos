using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private Vector3 storedPos;
    [SerializeField] private Vector3 referencedFirstObj;
    [SerializeField] private Vector3 currentPos;
    [SerializeField] bool firstTime = true;
    private
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePositionRelativeToUIElement(RectTransform UIElement)
    {
        //Guardar o transform do original uma única vez
        //Verificar a diferença de posicao do original para o UIElement
        //Mudar a posicao de acordo com essa diferenca
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (firstTime) 
        {
            firstTime = false; 
            storedPos = rectTransform.localPosition;
            currentPos = rectTransform.localPosition;
        }
        else
        {
            rectTransform.localPosition = storedPos;
        }

        var diff = referencedFirstObj - UIElement.localPosition;
        Debug.LogError($"{referencedFirstObj} - {UIElement.localPosition} = {diff}");
        rectTransform.localPosition = new Vector3(
            rectTransform.localPosition.x - diff.x,
            rectTransform.localPosition.y - diff.y,
            rectTransform.localPosition.z);
        if (!firstTime) currentPos = rectTransform.localPosition;
    }
}
