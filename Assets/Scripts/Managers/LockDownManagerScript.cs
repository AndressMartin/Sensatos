using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownManagerScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    [SerializeField] bool emLockdown;

    [SerializeField] float contadorTempoLockdown, contadorTempoLockdownMax;

    [SerializeField] AudioClip somAlarme;

    void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    private void Update()
    {
        if (emLockdown)
        {
            if (generalManager.EnemyManager.QuantidadeInimigosVendoPlayer > 0)
            {
                Contador();
            }
            else
            {
                ContadorLockdownInverso();
            }

            generalManager.Hud.AtualizarTempoLockDown(contadorTempoLockdown);
        }
    }

    public void Respawn()
    {
        DesativarLockDown();
    }

    public void Contador()
    {
        contadorTempoLockdown = contadorTempoLockdownMax;
    }

    public void ContadorLockdownInverso()
    {
        contadorTempoLockdown -= Time.deltaTime;
        if (contadorTempoLockdown <= 0)
        {
            contadorTempoLockdown = 0;
            DesativarLockDown();
        }
    }

    public void AtivarLockDown(Vector2 posicaoDoPlayer)
    {
        emLockdown = true;
        contadorTempoLockdown = contadorTempoLockdownMax;
        generalManager.EnemySpawnManager.AtivarInimigos();

        foreach (LockDownButton botao in generalManager.ObjectManager.ListaAlarmes)
        {
            botao.ReceberLockDown();
        }

        foreach (Enemy enemy in generalManager.ObjectManager.ListaInimigos)
        {
            if(enemy.Zona == generalManager.ZoneManager.ZonaAtual)
            {
                enemy.GetIAEnemy.ReceberLockDown(posicaoDoPlayer, generalManager.ZoneManager.PontosDeProcuraAtuais);
            }
            else
            {
                enemy.SeDesativarNoLockdown();
            }
        }
        foreach (CameraDeSeguranca cameraJogo in generalManager.ObjectManager.ListaDeCamerasLockdown)
        {
            cameraJogo.ReceberLockdown(true);
        }

        TrancarPortas();

        generalManager.Hud.LockDownUIAtiva(true);

        generalManager.Hud.EfeitosDeTela.AnimacaoLockdown();

        generalManager.SoundManager.TocarSom(somAlarme);
    }

    public void DesativarLockDown()
    {
        contadorTempoLockdown = 0;
        emLockdown = false;

        foreach (LockDownButton lockDown in generalManager.ObjectManager.ListaAlarmes)
        {
            lockDown.DesativarLockDown();
        }

        foreach (Enemy enemy in generalManager.ObjectManager.ListaInimigos)
        {
            enemy.GetIAEnemy.DesativarLockDown();
        }

        foreach (CameraDeSeguranca cameraJogo in generalManager.ObjectManager.ListaDeCamerasLockdown)
        {
            cameraJogo.ReceberLockdown(false);
        }

        DestrancarPortas();

        generalManager.Hud.LockDownUIAtiva(false);
    }

    private void TrancarPortas()
    {
        foreach (Porta porta in generalManager.ObjectManager.ListaPortas)
        {
            porta.AtivarLockDown();
        }     
    }

    private void DestrancarPortas()
    {
        foreach (Porta porta in generalManager.ObjectManager.ListaPortas)
        {
            porta.DesativarLockDown();
        }  
    }
}
