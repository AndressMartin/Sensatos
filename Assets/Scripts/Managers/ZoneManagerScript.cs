using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagerScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private int zonaAtual;
    [SerializeField] Zona[] zonas;

    //Getters
    public int ZonaAtual => zonaAtual;

    //Setters
    public void SetZonaAtual(int zona)
    {
        zonaAtual = zona;
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

        StartCoroutine(AtivarEDesativarInimigos());
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

    private IEnumerator AtivarEDesativarInimigos()
    {
        while(true)
        {
            foreach(Enemy enemy in generalManager.ObjectManager.ListaInimigos)
            {
                if(enemy.GetIAEnemy.GetTipoInimigo == IAEnemy.TipoInimigo.Lockdown)
                {
                    continue;
                }

                if(enemy.Zona != zonaAtual)
                {
                    if(enemy.gameObject.activeSelf == true)
                    {
                        if(enemy.GetIAEnemy.GetInimigoEstados == IAEnemy.InimigoEstados.Patrulhar && enemy.GetIAEnemy.GetEstadoDeteccaoPlayer == IAEnemy.EstadoDeteccaoPlayer.NaoToVendoPlayer && enemy.GetIAEnemy.GetEmLockdown == false)
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

            yield return new WaitForSeconds(1f);
        }
    }
}
