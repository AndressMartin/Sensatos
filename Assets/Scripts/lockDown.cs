using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDown : ObjetoInteragivel
{
    //Managers
    private ObjectManagerScript objectManagerScript;
    private PauseManagerScript pauseManager;
    private LockDownManager lockDownManager;

    //Componentes
    SpriteRenderer spriteRenderer;

    //Variaveis
    public bool lockDownAtivo;

    private void Start()
    {
        //Managers
        lockDownManager = FindObjectOfType<LockDownManager>();
        pauseManager = FindObjectOfType<PauseManagerScript>();
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManagerScript.adicionarAosObjetosInteragiveis(this);
        objectManagerScript.adicionarAosAlarmes(this);

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
    private void DesativarLockDown()
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
