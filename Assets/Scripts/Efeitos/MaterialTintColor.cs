using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTintColor : MonoBehaviour
{
    private Material material;
    private Color materialTintColor;

    private void Awake()
    {
        materialTintColor = new Color(1, 0, 0, 0);
        SetMaterial(GetComponent<SpriteRenderer>().material);
    }

    public void SetMaterial(Material material)
    {
        this.material = material;
    }

    public void SetTintColor(Color color)
    {
        materialTintColor = color;
        material.SetColor("_Tint", materialTintColor);
    }
}
