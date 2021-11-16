using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockDawn : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ativo;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(ativo)
        {
            spriteRenderer.color = Color.green;
        }
    }
}
