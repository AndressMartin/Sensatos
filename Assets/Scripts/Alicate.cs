using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alicate : Ferramenta
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Usar(GameObject objQueChamou)
    {
        Debug.Log("cortando");
    }
}
