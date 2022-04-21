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
        //Nada
    }

    public void ReceberLockDown()
    {
        //Nada
    }
}
