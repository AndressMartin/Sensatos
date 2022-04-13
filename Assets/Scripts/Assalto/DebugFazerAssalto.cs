using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFazerAssalto : MonoBehaviour
{
    GeneralManagerScript generalManagerScript;

    [SerializeField] private Assalto mapaAssalto01;
    [SerializeField] private Assalto mapaAssalto02;
    [SerializeField] private Assalto mapaAssalto03;

    private void Start()
    {
        generalManagerScript = FindObjectOfType<GeneralManagerScript>();
        //assaltoManager = generalManagerScript.AssaltoManager();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            generalManagerScript.AssaltoManager.SetarAssalto(mapaAssalto01);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            generalManagerScript.AssaltoManager.SetarAssalto(mapaAssalto02);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            generalManagerScript.AssaltoManager.SetarAssalto(mapaAssalto03);
        }
    }


}
