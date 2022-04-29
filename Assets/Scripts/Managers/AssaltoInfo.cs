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
    [SerializeField] private List<ItemChave> itensParaExcluirNoFimDoAssalto;
    [SerializeField] private Vector2 posicaoDoCheckpoint;
    [SerializeField] private EntityModel.Direcao direcaoDoCheckpoint;
    [SerializeField] private UnityEvent eventosItemPrincipalColetado;

    private bool itemPrincipalColetado;

    //Variaveis de respawn
    private List<ItemDeAssalto> itensSecundariosRespawn;

    //Getters
    public bool ItemPrincipalPego => itemPrincipalColetado;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Variaveis
        itensSecundarios = new List<ItemDeAssalto>();
        itensSecundariosRespawn = new List<ItemDeAssalto>();
        itemPrincipalColetado = false;

        //Trocar o idioma uma vez para iniciar os objetos com o idioma correto
        TrocarIdioma();

        SetRespawn();
    }

    public void SetRespawn()
    {
        itensSecundariosRespawn.Clear();

        foreach (ItemDeAssalto item in itensSecundarios)
        {
            //Cria uma nova instancia do scriptable object e a adiciona no inventario
            ItemDeAssalto novoItem = ScriptableObject.Instantiate(item);
            novoItem.name = item.name;

            novoItem.TrocarIdioma();
            novoItem.SetQuantidade(item.Quantidade);

            itensSecundariosRespawn.Add(novoItem);
        }
    }

    public void Respawn()
    {
        itensSecundarios.Clear();

        foreach (ItemDeAssalto item in itensSecundariosRespawn)
        {
            //Cria uma nova instancia do scriptable object e a adiciona no inventario
            ItemDeAssalto novoItem = ScriptableObject.Instantiate(item);
            novoItem.name = item.name;

            novoItem.TrocarIdioma();
            novoItem.SetQuantidade(item.Quantidade);

            itensSecundarios.Add(novoItem);
        }
    }

    public void AdicionarItemDeAssalto(ItemDeAssalto item)
    {
        if(item.name == itemPrincipal.name)
        {
            if(itemPrincipalColetado == false)
            {
                IniciarTelaItemPrincipalColetado();
            }
        }
        else
        {
            //Confere se o item ja esta na lista de itens secundarios, se estiver, adiciona um ao numero do item
            foreach (ItemDeAssalto itemNaLista in itensSecundarios)
            {
                if (item.name == itemNaLista.name)
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

    private void IniciarTelaItemPrincipalColetado()
    {
        generalManager.Hud.TelaItemPrincipalPego.IniciarTela(itemPrincipal);
    }

    public void ItemPrincipalColetado()
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

    public void FinalizarOAssalto()
    {
        CalcularDinheiroGanho();

        ItensParaExcluirNoFimDoAssalto();

        generalManager.Hud.MenuFimDoAssalto.IniciarMenuFimDoAssalto();
    }

    private void CalcularDinheiroGanho()
    {
        int dinheiroSaqueExtra = 0;

        //Calcula o dinheiro ganho com os itens secundarios
        foreach (ItemDeAssalto item in itensSecundarios)
        {
            dinheiroSaqueExtra += item.Valor * item.Quantidade;
        }

        //Soma o valor do dinheiro ganho ao dinheiro do player
        generalManager.Player.Inventario.SetDinheiro(generalManager.Player.Inventario.Dinheiro + itemPrincipal.Valor + dinheiroSaqueExtra);

        //Atualiza os valores de dinheiro no menu do fim do assalto
        generalManager.Hud.MenuFimDoAssalto.AtualizarDinheiro(itemPrincipal.Valor, dinheiroSaqueExtra);
    }

    private void ItensParaExcluirNoFimDoAssalto()
    {
        foreach(ItemChave item in itensParaExcluirNoFimDoAssalto)
        {
            generalManager.Player.InventarioMissao.RemoverItem(item);
        }
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
