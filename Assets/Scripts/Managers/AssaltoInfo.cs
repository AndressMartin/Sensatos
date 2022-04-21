using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AssaltoInfo : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private ItemDeAssalto itemPrincipal;
    [SerializeField] private List<ItemDeAssalto> itensSecundarios;
    [SerializeField] private Vector2 posicaoDoCheckpoint;
    [SerializeField] private EntityModel.Direcao direcaoDoCheckpoint;
    [SerializeField] private UnityEvent eventosItemPrincipalColetado;

    private bool itemPrincipalColetado;

    //Variaveis de respawn
    private List<ItemDeAssalto> itensSecundariosRespawn;

    //Getters
    public bool ItemPrincipalPego => itemPrincipal;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Variaveis
        itensSecundarios = new List<ItemDeAssalto>();
        itemPrincipalColetado = false;

        //Trocar o idioma uma vez para iniciar os objetos com o idioma correto
        TrocarIdioma();

        SetRespawn();
    }

    public void SetRespawn()
    {
        itensSecundariosRespawn = itensSecundarios;
    }

    public void Respawn()
    {
        itensSecundarios = itensSecundariosRespawn;
    }

    public void AdicionarItemDeAssalto(ItemDeAssalto item)
    {
        if(item == itemPrincipal)
        {
            if(itemPrincipalColetado == false)
            {
                ItemPrincipalColetado();
            }
        }
        else
        {
            //Confere se o item ja esta na lista de itens secundarios, se estiver, adiciona um ao numero do item
            foreach (ItemDeAssalto itemNaLista in itensSecundarios)
            {
                if (item == itemNaLista)
                {
                    itemNaLista.SetQuantidade(itemNaLista.Quantidade + 1);
                    return;
                }
            }

            //Inicia o nome do item
            item.TrocarIdioma();
            item.SetQuantidade(1);

            itensSecundarios.Add(item);
        }
    }

    private void ItemPrincipalColetado()
    {
        itemPrincipalColetado = true;

        eventosItemPrincipalColetado?.Invoke();

        SetarCheckpoint();
    }

    private void SetarCheckpoint()
    {
        generalManager.RespawnManager.RespawnarInimigos();
        generalManager.RespawnManager.SetCheckpoint(posicaoDoCheckpoint, direcaoDoCheckpoint);
    }

    private void TrocarIdioma()
    {
        itemPrincipal.TrocarIdioma();

        foreach (ItemDeAssalto item in itensSecundarios)
        {
            item.TrocarIdioma();
        }
    }
}
