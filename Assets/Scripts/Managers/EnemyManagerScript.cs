using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    //Variaveis
    [SerializeField] private int quantidadeInimigosVendoPlayer;
    public int GetQuantidadeInimigosVendoPlayer => quantidadeInimigosVendoPlayer;

    //Getters
    public int QuantidadeInimigosVendoPlayer => quantidadeInimigosVendoPlayer;

    private void Start()
    {
        StartCoroutine(IgnorarColisaoEntreOsInimigosCorrotina());
    }

    public void Respawn()
    {
        quantidadeInimigosVendoPlayer = 0;
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

    private void IgnorarColisaoEntreOsInimigos()
    {
        GeneralManagerScript generalManager = FindObjectOfType<GeneralManagerScript>();
        
        foreach(Enemy inimigo in generalManager.ObjectManager.ListaInimigos)
        {
            foreach(Enemy inimigo2 in generalManager.ObjectManager.ListaInimigos)
            {
                if(inimigo != inimigo2)
                {
                    Physics2D.IgnoreCollision(inimigo.gameObject.GetComponent<Collider2D>(), inimigo2.gameObject.GetComponent<Collider2D>(), true);
                }
            }
        }
    }

    private IEnumerator IgnorarColisaoEntreOsInimigosCorrotina()
    {
        yield return new WaitForSeconds(0.5f);

        IgnorarColisaoEntreOsInimigos();
    }
}
