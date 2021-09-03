using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{

    public bool strafing=false;
    public bool estadoCombate = false;
    public bool interagindo;
    public int movimento = 2;
    public float colldown;
    public float colldowMax;
    private Movement movement;
    private Inventario inventario;
    private SpriteRenderer spriteRenderer;
    public ParedeModel objetoQualEstaColidindo;

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
        Colldowns();
        BotoesPressionados();
        

    }

    

    void EstadoCombateOnOff()
    {
        estadoCombate = !estadoCombate;
        if(estadoCombate)
            spriteRenderer.color = (Color.red);
        else
            spriteRenderer.color = (Color.white);

    }
    void Colldowns()
    {
        if (colldown > 0)
            colldown = colldown - 1 * (Time.deltaTime);

        if (colldown > 0 && colldown < colldowMax)
            interagindo = true;

        else
            interagindo = false;
    }

    public void UpdateRunSpeed()
    {
        movement.UpdateRunSpeed(movimento);
    }

    void BotoesPressionados()
    {
        if (Input.GetKeyDown(KeyCode.E) && colldown <= 0)//Botão de interação
        {
            colldown = colldowMax;
        }

        if (Input.GetKeyDown(KeyCode.Space))//Botão de disparo
        {
            inventario.UsarItemAtual();
        }

        if (Input.GetKeyDown(KeyCode.G))//Botão para entar no modo combate (vermelho == combate)
        {
            EstadoCombateOnOff();
        }

        if (Input.GetKeyDown(KeyCode.B))//Botão para ativar o strafing
        {
            strafing = !strafing;

        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && movimento != 1)//Botão para agachar //se estiver correndo ou em pé, não agachado
        {
            movimento = 1;//agachado
            UpdateRunSpeed();
        }

        else if (Input.GetKeyDown(KeyCode.LeftControl) && movimento == 1)//Botão para ficar de pé //se estiver agachado
        {
            movimento = 2;//em pé
            UpdateRunSpeed();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) //Botão para correr
        {
            if (movimento == 3)
                movimento = 2;
            else
                movimento = 3;
            UpdateRunSpeed();
        }
    }
}
