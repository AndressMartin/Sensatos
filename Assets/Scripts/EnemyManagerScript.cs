using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private int quantidadeInimigosVendoPlayer;
    public int GetQuantidadeInimigosVendoPlayer => quantidadeInimigosVendoPlayer;
    [SerializeField] private List<Transform> pontosDeProcura;

    //Getters
    public List<Transform> PontosDeProcura => pontosDeProcura;

    private void Start()
    {
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    private void Update()
    {
        if (generalManager.LockDownManager.EmLockdow)
        {
            if (quantidadeInimigosVendoPlayer > 0)
            {
                generalManager.LockDownManager.Contador();
            }
            else
            {
                generalManager.LockDownManager.ContadorLockdownInverso();
            }
        }
    }
    public void Respawn()
    {
        quantidadeInimigosVendoPlayer=0;
    }

    public void PerdiVisaoInimigo()
    {
        quantidadeInimigosVendoPlayer--;
    }
    public int AdicionarAlguemVendoPlayer()
    {
        quantidadeInimigosVendoPlayer++;
        return quantidadeInimigosVendoPlayer;
    }
    public bool VerificarUltimoVerPlayer(int indice)
    {
        if (quantidadeInimigosVendoPlayer == 1) // caso so tenha um inimigo vendo o player ele sempre vai receber que tem de lutar
        {
            return false;
        }
        else
        {
            if (indice < quantidadeInimigosVendoPlayer)
            {
                return false;
            }
            else
                return true;
        }
    }
}
