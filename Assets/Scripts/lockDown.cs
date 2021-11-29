using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockDown : MonoBehaviour
{
    //Managers
    private PauseManagerScript pauseManager;

    public bool ativo;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        //Managers
        pauseManager = FindObjectOfType<PauseManagerScript>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(pauseManager.GetJogoPausado() == false)
        {
            if (ativo)
            {
                spriteRenderer.color = Color.green;
            }
        }
    }
}
