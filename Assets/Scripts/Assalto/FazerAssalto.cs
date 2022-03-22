using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FazerAssalto : MonoBehaviour
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
            if (AssaltoManager.Verificar(mapaAssalto01))
            {
                Debug.Log("Mapa01\n Pode Fazer o mapa, todas quest principais feitas");
            }
            else
            {
                Debug.Log("Mapa01\n n�o Pode Fazer o mapa");

            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (AssaltoManager.Verificar(mapaAssalto02))
            {
                Debug.Log("Mapa02\n Pode Fazer o mapa, todas quest principais feitas");

            }
            else
            {
                Debug.Log("Mapa02\n n�o Pode Fazer o mapa,");

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
