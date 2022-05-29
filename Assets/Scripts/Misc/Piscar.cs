using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Piscar : MonoBehaviour
{
    //Componentes
    private SpriteRenderer spriteRenderer;
    private TMP_Text texto;

    //Variaveis
    [SerializeField] private float tempo;
    private float tempo2;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        texto = GetComponent<TMP_Text>();

        tempo2 = 0;
    }

    void Update()
    {
        tempo2 += Time.deltaTime;

        if(tempo2 >= tempo)
        {
            tempo2 -= tempo;

            if(spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            if(texto != null)
            {
                texto.enabled = !texto.enabled;
            }
        }
    }
}
