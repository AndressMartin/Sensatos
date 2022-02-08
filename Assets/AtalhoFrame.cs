using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtalhoFrame : ItemFrame
{
    ItemFrame referencia;
    internal void GuardarReferencia(ItemFrame itemFrame)
    {
        if (!image) image = GetComponent<Image>();
        referencia = itemFrame;
        image.sprite = referencia.GetSavedElement().ImagemInventario;
    }

    internal void PerderReferencia()
    {
        referencia = null;
        image.sprite = null;
    }
}
