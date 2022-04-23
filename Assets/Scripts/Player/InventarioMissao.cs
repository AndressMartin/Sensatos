using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioMissao : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private MudarIdiomaItensDoInventario mudarIdiomaItensDoInventario;

    //Variaveis
    private static List<ItemChave> itens = new List<ItemChave>();

    //Getters
    public List<ItemChave> Itens => itens;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Componentes
        mudarIdiomaItensDoInventario = GetComponent<MudarIdiomaItensDoInventario>();

        //Trocar o idioma uma vez para iniciar os objetos com o idioma correto
        TrocarIdioma();
    }

    public void Respawn()
    {
        CarregarSave(SaveData.InventarioRespawn);
    }

    public void AdicionarItem(ItemChave item)
    {
        //Confere se o item ja esta na lista de itens chave, se estiver, adiciona um ao numero do item
        foreach(ItemChave itemChave in itens)
        {
            if(itemChave.ID == item.ID)
            {
                itemChave.SetQuantidade(itemChave.Quantidade + 1);
                return;
            }
        }

        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        ItemChave novoItem = ScriptableObject.Instantiate(item);
        novoItem.name = item.name;
        novoItem.SetQuantidade(1);

        mudarIdiomaItensDoInventario.TrocarIdioma(novoItem);

        itens.Add(novoItem);
    }

    public void RemoverItem(ItemChave item)
    {
        //Procura um item com o mesmo ID do que foi passado no inventario para destrui-lo
        for (int i = 0; i < itens.Count; i++)
        {
            if (itens[i].ID == item.ID)
            {
                Itens.Remove(itens[i]);
                return;
            }
        }
    }

    public int ProcurarQuantidadeItem(ItemChave _item)
    {
        foreach (var item in itens)
        {
            if(item.ID == _item.ID)
            {
                return item.Quantidade;
            }
        }
        return 0;
    }

    private void TrocarIdioma()
    {
        foreach (Item item in itens)
        {
            mudarIdiomaItensDoInventario.TrocarIdioma(item);
        }
    }

    public void CarregarSave(SaveData.InventarioSave inventarioSave)
    {
        //Lista de itens
        itens.Clear();

        foreach (SaveData.ItemChaveSave item in inventarioSave.itensChave)
        {
            AdicionarItem((ItemChave)Listas.instance.ListaDeItens.GetItem[item.id]);

            itens[itens.Count - 1].SetQuantidade(item.quantidade);
        }
    }

    public void ResetarInventario()
    {
        //Lista de itens
        itens.Clear();
    }
}
