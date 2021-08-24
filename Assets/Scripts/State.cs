using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public int movimento=2;
    public bool interagindo;
    public int colldown;
    public int colldowMax;
    private Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        UpdateRunSpeed();
    }

    // Update is called once per frame
    void Update()
    {   
        if(colldown>0)
            colldown--;

        if (colldown > 0 && colldown < colldowMax)
            interagindo = true;

        else
            interagindo = false;

        if(Input.GetKeyDown(KeyCode.E) && colldown <= 0)
        {
            colldown = colldowMax;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && movimento != 1)//se estiver correndo ou em pé, não agachado
        {
            movimento = 1;//agachado
            UpdateRunSpeed();
        }

        else if (Input.GetKeyDown(KeyCode.LeftControl) && movimento == 1)//se estiver agachado
        {
            movimento = 2;//em pé
            UpdateRunSpeed();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (movimento == 3)
                movimento = 2;
            else
                movimento = 3;
            UpdateRunSpeed();
        }

    }



    public void UpdateRunSpeed()
    {
        movement.UpdateRunSpeed(movimento);
    }
}
