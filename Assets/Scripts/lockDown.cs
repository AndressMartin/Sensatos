using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDown : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    SpriteRenderer spriteRenderer;

    //Variaveis
    public bool lockDownAtivo;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Variaveis
        ativo = true;

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);
        generalManager.ObjectManager.AdicionarAosAlarmes(this);

        //Variaveis
        lockDownAtivo = false;
    }
    public override void Respawn()
    {
        DesativarLockDown();

    }
    public void AtivarLockDown()
    {
        lockDownAtivo = true;
        CorBotao();
    }
    public void DesativarLockDown()
    {
        lockDownAtivo = false;
        CorBotao();
    }
    public override void Interagir(Player player)
    {
        
    }
    void CorBotao()
    {
        if (lockDownAtivo)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.blue;
        }
    }
}
