using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemFrame : MonoBehaviour
{
    public Image itemImage;
    private Item savedItemRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Item itemRef)
    {
        itemImage.sprite = itemRef.ImagemInventario;
    }
}
