using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockDown : ObjetoInteragivel
{
    //Managers
    private PauseManagerScript pauseManager;
    public bool ativo;
    SpriteRenderer spriteRenderer;
    ObjectManagerScript objectManagerScript;
    LockDownManager lockDownManager;

    private void Start()
    {
        //Managers
        lockDownManager = FindObjectOfType<LockDownManager>();
        objectManagerScript = FindObjectOfType<ObjectManagerScript>();
        pauseManager = FindObjectOfType<PauseManagerScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        objectManagerScript.adicionarAosObjetosInteragiveis(this);
        objectManagerScript.adicionarAosAlarmes(this);
    }
    void Update()
    {
        if(pauseManager.JogoPausado == false)
        {
            if (ativo)
            {
                spriteRenderer.color = Color.green;
            }
        }
    }
    public void AtivarLockDown(Vector2 posicaoDoPlayer)
    {
        lockDownManager.VerPlayer(posicaoDoPlayer);
    }
    public override void Interagir(Player player)
    {
        
    }
}
