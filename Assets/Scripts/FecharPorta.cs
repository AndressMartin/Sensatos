using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FecharPorta : MonoBehaviour
{
    private Porta porta;

    // Start is called before the first frame update
    void Start()
    {
        porta = GetComponentInParent<Porta>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            porta.FecharPorta();
        }
    }
}
