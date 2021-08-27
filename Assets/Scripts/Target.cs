using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public List<GameObject> objetos = new List<GameObject>();
    public GameObject[] varx;
    public int cont = 0;

    // Start is called before the first frame update
    void Start()
    {
        varx=GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject item in varx)
        {
            objetos.Add(item);
        }
        //objetos.Add(FindObjectOfType<State>().gameObject);
        //objetos.Add(FindObjectOfType<Porta>().gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (cont >= objetos.Count)
                cont = 0;

            transform.position = objetos[cont].transform.position;
            if (cont < objetos.Count)
                cont++;

             
        }
    }
}
