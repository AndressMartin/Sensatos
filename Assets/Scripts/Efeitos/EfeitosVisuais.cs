using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitosVisuais : MonoBehaviour
{
    //Componentes
    private SpriteRenderer spriteRenderer;
    private MaterialTintColor materialTintColor;

    [SerializeField] private Material materialTint;
    [SerializeField] private Material materialTintSolid;

    //Variaveis
    private Color tintColor;
    private float tintFadeSpeed;

    private void Start()
    {
        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialTintColor = materialTintColor = GetComponent<MaterialTintColor>();

        materialTint = Instantiate(materialTint);
        materialTintSolid = Instantiate(materialTintSolid);

        //Variaveis
        tintColor = new Color(1, 0, 0, 0);
        tintFadeSpeed = 0;
    }

    public void SetTintEffect(Color cor, float velocidadeEfeito)
    {
        spriteRenderer.material = materialTint;
        materialTintColor.SetMaterial(materialTint);
        tintColor = cor;
        tintFadeSpeed = velocidadeEfeito;

        StartCoroutine(TintEffect());
    }

    public void SetTintSolidEffect(Color cor, float velocidadeEfeito)
    {
        spriteRenderer.material = materialTintSolid;
        materialTintColor.SetMaterial(materialTintSolid);
        tintColor = cor;
        tintFadeSpeed = velocidadeEfeito;

        StartCoroutine(TintEffect());
    }

    private IEnumerator TintEffect()
    {
        materialTintColor.SetTintColor(tintColor);

        while (tintColor.a > 0)
        {
            tintColor.a = Mathf.Clamp01(tintColor.a - tintFadeSpeed * Time.deltaTime);
            materialTintColor.SetTintColor(tintColor);

            yield return null;
        }
    }
}
