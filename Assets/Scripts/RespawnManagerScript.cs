using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManagerScript : MonoBehaviour
{
    //Managers
    private ObjectManagerScript objectManager;

    private Player player;

    void Start()
    {
        //Managers
        objectManager = FindObjectOfType<ObjectManagerScript>();

        player = FindObjectOfType<Player>();
    }

    public void SetCheckpoint(Vector2 posicao, Player.Direcao direcao)
    {
        player.SetRespawn(posicao, direcao);

        foreach (ObjetoInteragivel objetoInteragivel in objectManager.listaObjetosInteragiveis)
        {
            objetoInteragivel.SetRespawn();
        }

        foreach (ParedeModel paredeQuebravel in objectManager.listaParedesQuebraveis)
        {
            paredeQuebravel.SetRespawn();
        }

        foreach (Enemy inimigo in objectManager.listaInimigos)
        {
            inimigo.SetRespawn();
        }

        foreach (LockDown alarme in objectManager.listaAlarme)
        {
            alarme.SetRespawn();
        }

        foreach (Porta porta in objectManager.listaPortas)
        {
            porta.SetRespawn();
        }
    }

    public void Respawn()
    {
        player.Respawn();

        foreach (ObjetoInteragivel objetoInteragivel in objectManager.listaObjetosInteragiveis)
        {
            objetoInteragivel.Respawn();
        }

        foreach (ParedeModel paredeQuebravel in objectManager.listaParedesQuebraveis)
        {
            paredeQuebravel.Respawn();
        }

        foreach (Enemy inimigo in objectManager.listaInimigos)
        {
            inimigo.Respawn();
        }

        foreach (LockDown alarme in objectManager.listaAlarme)
        {
            alarme.Respawn();
        }

        foreach (Porta porta in objectManager.listaPortas)
        {
            porta.Respawn();
        }
    }
}