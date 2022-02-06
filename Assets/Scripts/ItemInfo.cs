using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : InfoScreen
{
    // Start is called before the first frame update
    void Start()
    {
        if (inventario == null) inventario = FindObjectOfType<Inventario>();
    }
    public override void OpenScreen()
    {
        base.OpenScreen();
        if (transform.GetChild(0).gameObject.activeSelf) UpdateExplainPanel(objectThatCalled);
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.GetChild(0).gameObject.activeSelf);
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            UpdateExplainPanel(objectThatCalled);
        }
    }
}
