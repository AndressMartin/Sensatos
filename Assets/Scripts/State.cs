using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{

    public bool strafing=false;
    public bool estadoCombate = false;
    public bool interagindo;
    public int movimento = 2;
    public int colldown;
    public int colldowMax;
    private Movement movement;
    private Inventario inventario;
    private SpriteRenderer spriteRenderer;
    private Target target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Target>();
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            target.ChangeTarget();
        }

        if (Input.GetKeyDown(KeyCode.E) && colldown <= 0)//Bot�o de intera��o
        {
            colldown = colldowMax;
        }

        if (Input.GetKeyDown(KeyCode.Space) && estadoCombate)//Bot�o de disparo
        {
            inventario.UsarItemAtual();
        }

        if (Input.GetKeyDown(KeyCode.G))//Bot�o para entar no modo combate (vermelho == combate)
        {
            EstadoCombateOnOff();
        }

        if (Input.GetKeyDown(KeyCode.B))//Bot�o para ativar o strafing
        {
            strafing = !strafing;
            if(strafing)
            {
                UpdateStrafeSpeed(1);
            }
            else
                UpdateStrafeSpeed(-1);

        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && movimento != 1)//Bot�o para agachar //se estiver correndo ou em p�, n�o agachado
        {
            movimento = 1;//agachado
            UpdateRunSpeed();
        }

        else if (Input.GetKeyDown(KeyCode.LeftControl) && movimento == 1)//Bot�o para ficar de p� //se estiver agachado
        {
            movimento = 2;//em p�
            UpdateRunSpeed();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)) //Bot�o para correr
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

    void UpdateStrafeSpeed(int _velocidade)
    {
        movement.UpdateStrafeSPeed(_velocidade);
    }

    public void UpdateRunSpeed()
    {
        movement.UpdateRunSpeed(movimento);
    }
}
