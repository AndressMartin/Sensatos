using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFazerAssalto : MonoBehaviour
{
    GeneralManagerScript generalManagerScript;

    [SerializeField] private Assalto mapaAssalto01;
    [SerializeField] private Assalto mapaAssalto02;
    private void Start()
    {
        generalManagerScript = FindObjectOfType<GeneralManagerScript>();
        //assaltoManager = generalManagerScript.AssaltoManager();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            AssaltoManager.SetarAssalto(mapaAssalto01);
            generalManagerScript.Player.Inventario.SetarAssalto(mapaAssalto01);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (AssaltoManager.VerificarAssalto(mapaAssalto02,generalManagerScript.Player))
            {
                Debug.Log("Mapa02\n Pode Fazer o mapa, todas quest principais feitas");

            }
            else
            {
                Debug.Log("Mapa02\n não Pode Fazer o mapa,");

            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
            
        }
         
    }
}
