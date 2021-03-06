using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagerScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private bool controlarInimigos;
    [SerializeField] private bool controlarNpcs;

    [SerializeField] private int zonaAtual;
    [SerializeField] Zona[] zonas;

    //Getters
    public int ZonaAtual => zonaAtual;
    public List<Transform> PontosDeProcuraAtuais => GetPontosDeProcura();

    //Setters
    public void SetZonaAtual(int zona)
    {
        zonaAtual = zona;

        if (controlarInimigos == true)
        {
            AtivarEDesativarInimigos();
        }

        if (controlarNpcs == true)
        {
            AtivarEDesativarNPCs();
        }
    }

    private void Awake()
    {
        zonas = FindObjectsOfType<Zona>();
        zonaAtual = 0;

        for(int i = 0; i < zonas.Length; i++)
        {
            zonas[i].gameObject.SetActive(true);
            zonas[i].Iniciar(this, i);
        }
    }

    private void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();

        if(controlarInimigos == true)
        {
            StartCoroutine(AtivarEDesativarInimigosCorrotina());
        }
    }

    private void AtivarInimigo(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);

        if(enemy.Morto == false)
        {
            enemy.ResetarVariaveis();
        }
    }

    private void DesativarInimigo(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void AtivarNPC(NPC npc)
    {
        npc.gameObject.SetActive(true);
    }

    private void DesativarNPC(NPC npc)
    {
        npc.gameObject.SetActive(false);
    }

    public List<Transform> GetPontosDeProcura()
    {
        foreach(Zona zona in zonas)
        {
            if(zona.GetZona == zonaAtual)
            {
                return zona.PontosDeProcura;
            }
        }

        Debug.LogWarning("A zona nao existe!");

        return null;
    }

    private void AtivarEDesativarInimigos()
    {
        foreach (Enemy enemy in generalManager.ObjectManager.ListaInimigos)
        {
            if (enemy.GetIAEnemy.GetTipoInimigo == IAEnemy.TipoInimigo.Lockdown)
            {
                continue;
            }

            if (enemy.Zona != zonaAtual)
            {
                if (enemy.gameObject.activeSelf == true)
                {
                    if (enemy.GetIAEnemy.GetInimigoEstados == IAEnemy.InimigoEstados.Patrulhar && enemy.GetIAEnemy.GetEstadoDeteccaoPlayer == IAEnemy.EstadoDeteccaoPlayer.NaoToVendoPlayer && enemy.GetIAEnemy.GetEmLockdown == false)
                    {
                        DesativarInimigo(enemy);
                    }
                }
            }
            else
            {
                if (enemy.gameObject.activeSelf == false)
                {
                    AtivarInimigo(enemy);
                }
            }
        }
    }

    private IEnumerator AtivarEDesativarInimigosCorrotina()
    {
        while(true)
        {
            AtivarEDesativarInimigos();

            yield return new WaitForSeconds(1f);
        }
    }

    private void AtivarEDesativarNPCs()
    {
        foreach (NPC npc in generalManager.ObjectManager.ListaDeNPCs)
        {
            if (npc.Zona != zonaAtual)
            {
                if (npc.gameObject.activeSelf == true)
                {
                    DesativarNPC(npc);
                }
            }
            else
            {
                if (npc.gameObject.activeSelf == false)
                {
                    AtivarNPC(npc);
                }
            }
        }
    }
}
