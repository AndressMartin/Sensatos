using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FazerAssalto : MonoBehaviour
{
    GeneralManagerScript generalManagerScript;
    AssaltoManager assaltoManager;

    [SerializeField] private Assalto mapaAssalto01;
    [SerializeField] private Assalto mapaAssalto02;
    private void Start()
    {
        generalManagerScript = FindObjectOfType<GeneralManagerScript>();
        //assaltoManager = generalManagerScript.AssaltoManager();
        assaltoManager = FindObjectOfType<AssaltoManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (assaltoManager.Verificar(mapaAssalto01))
                {
                    Debug.Log("Mapa01\n Pode Fazer o mapa, todas quest principais feitas");
                }
                else
                {
                    Debug.Log("Mapa01\n não Pode Fazer o mapa");

                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (assaltoManager.Verificar(mapaAssalto02))
                {
                    Debug.Log("Mapa02\n Pode Fazer o mapa, todas quest principais feitas");

                }
                else
                {
                    Debug.Log("Mapa02\n não Pode Fazer o mapa,");

                }
            }
            
        }
         
    }
}
