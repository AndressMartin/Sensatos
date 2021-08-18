using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public bool interagindo;
    public int colldown;
    public int colldowMax;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        colldown--;

        if (colldown > 0 && colldown < colldowMax)
            interagindo = true;

        else
            interagindo = false;

        if(Input.GetKeyDown(KeyCode.E) && colldown <= 0)
        {
            colldown = colldowMax;
        }
        
    }
}
