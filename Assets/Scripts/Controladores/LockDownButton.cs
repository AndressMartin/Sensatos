using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownButton : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosAlarmes(this);
    }

    public void Respawn()
    {
        DesativarLockDown();
    }

    public void AtivarLockDown(Vector2 posicaoDoPlayer)
    {
        generalManager.LockDownManager.AtivarLockDown(posicaoDoPlayer);
    }

    public void DesativarLockDown()
    {
        CorBotao(false);
    }

    public void ReceberLockDown()
    {
        CorBotao(true);
    }

    void CorBotao(bool ativo)
    {
        if (ativo)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.blue;
        }
    }
}
