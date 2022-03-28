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
            VerificarAssaltoMissao.SetarAssalto(mapaAssalto01);
            generalManagerScript.AssaltoManager.SetarAssalto(mapaAssalto01);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
            
        }
         
    }
}
