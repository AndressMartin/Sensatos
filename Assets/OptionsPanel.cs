using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanel : MonoBehaviour
{
    private Vector3 storedPos;
    [SerializeField] private Vector3 referencedFirstObj;
    bool firstTime = true;
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
        RectTransform ParentRectTransform = transform.parent.GetComponent<RectTransform>();
        if (firstTime) 
        {
            firstTime = false; 
            storedPos = ParentRectTransform.position;
        }
        else
        {
            ParentRectTransform.position = storedPos;
        }

        var diff = referencedFirstObj - UIElement.position;
        ParentRectTransform.position = new Vector3(
            ParentRectTransform.position.x - diff.x,
            ParentRectTransform.position.y - diff.y,
            ParentRectTransform.position.z);
    }
}
