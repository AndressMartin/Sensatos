using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManagerScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void SetCheckpoint(Vector2 posicao, Player.Direcao direcao)
    {
        generalManager.Player.SetRespawn(posicao, direcao);

        generalManager.AssaltoInfo.SetRespawn();

        foreach (ObjetoInteragivel objetoInteragivel in generalManager.ObjectManager.ListaObjetosInteragiveis)
        {
            objetoInteragivel.SetRespawn();
        }

        foreach (ParedeModel paredeQuebravel in generalManager.ObjectManager.ListaParedesQuebraveis)
        {
            paredeQuebravel.SetRespawn();
        }

        foreach (Enemy inimigo in generalManager.ObjectManager.ListaInimigos)
        {
            inimigo.SetRespawn();
        }

        foreach (Porta porta in generalManager.ObjectManager.ListaPortas)
        {
            porta.SetRespawn();
        }

        generalManager.SalaSeguranca?.SetRespaw();
    }

    public void Respawn()
    {
        generalManager.Player.Respawn();

        generalManager.AssaltoInfo.Respawn();

        foreach (ObjetoInteragivel objetoInteragivel in generalManager.ObjectManager.ListaObjetosInteragiveis)
        {
            objetoInteragivel.Respawn();
        }

        foreach (ParedeModel paredeQuebravel in generalManager.ObjectManager.ListaParedesQuebraveis)
        {
            paredeQuebravel.Respawn();
        }

        foreach (Enemy inimigo in generalManager.ObjectManager.ListaInimigos)
        {
            inimigo.Respawn();
        }

        foreach (LockDownButton alarme in generalManager.ObjectManager.ListaAlarmes)
        {
            alarme.Respawn();
        }

        foreach (Porta porta in generalManager.ObjectManager.ListaPortas)
        {
            porta.Respawn();
        }

        generalManager.SalaSeguranca?.Respawn();
        generalManager.LockDownManager.Respawn();
        generalManager.BulletManager.DeletarProjeteis();
        generalManager.EnemyManager.Respawn();

        generalManager.PathfinderManager.EscanearPathfinder();
    }

    public void RespawnarInimigos()
    {
        foreach (Enemy inimigo in generalManager.ObjectManager.ListaInimigos)
        {
            if(inimigo.Morto == true)
            {
                inimigo.SetMortoRespawn(false);
                inimigo.Respawn();
            }
        }
    }
}
