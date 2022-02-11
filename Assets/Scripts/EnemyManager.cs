using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    LockDownManager lockDownManager;
    [SerializeField] private int quantidadeInimigosVendoPlayer;
    public int GetQuantidadeInimigosVendoPlayer => quantidadeInimigosVendoPlayer;

    private void Start()
    {
        lockDownManager = transform.parent.GetComponentInChildren<LockDownManager>();
    }
    private void Update()
    {
        if (lockDownManager.EmLockdow)
        {
            if (quantidadeInimigosVendoPlayer > 0)
            {
                lockDownManager.Contador();
            }
            else
            {
                lockDownManager.ContadorLockdownInverso();
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
    public int AddicionarAlguemVendoPlayer()
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
