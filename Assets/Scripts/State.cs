using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
   
    public bool estadoCombate = false;
    public bool interagindo;
    public int movimento = 2;
    public int colldown;
    public int colldowMax;
    private Movement movement;
    private Inventario inventario;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventario = GetComponent<Inventario>();
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

        if (Input.GetKeyDown(KeyCode.Space) && estadoCombate)
        {
            inventario.UsarItemAtual();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            EstadoCombateOnOff();
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
    void EstadoCombateOnOff()
    {
        estadoCombate = !estadoCombate;
        if(estadoCombate)
            spriteRenderer.color = (Color.red);
        else
            spriteRenderer.color = (Color.white);

    }




    public void UpdateRunSpeed()
    {
        movement.UpdateRunSpeed(movimento);
    }
}
