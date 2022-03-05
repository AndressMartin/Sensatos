using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemFrame : UIFrame
{
    private AtalhoFrame atalhoFrame;
    public AtalhoFrame AtalhoFrame 
    {
        get
        {
            if (atalhoFrame) return atalhoFrame;
            else return null;
        }
        set
        {
            if (!atalhoFrame)
            {
                atalhoFrame = value;
                atalhoFrame.GuardarReferencia(this);
            }
            else
            {
                atalhoFrame.PerderReferencia();
                atalhoFrame = value;
                atalhoFrame.GuardarReferencia(this);
            }
        }
    }
    public void Init(Item usableRef)
    {
        base.Init(usableRef);
    }

    public override void RemoveElement()
    {
        base.RemoveElement();
        atalhoFrame?.PerderReferencia();
    }
}
