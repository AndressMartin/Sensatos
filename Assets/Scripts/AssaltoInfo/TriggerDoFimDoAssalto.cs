using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoFimDoAssalto : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(generalManager.AssaltoInfo.ItemPrincipalPego == true)
            {
                generalManager.AssaltoInfo.FinalizarOAssalto();
            }
        }
    }
}
