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
            return atalhoFrame;
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
}
