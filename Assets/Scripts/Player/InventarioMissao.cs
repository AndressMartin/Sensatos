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
    private List<Item> itens = new List<Item>();

    //Getters
    public List<Item> Itens => itens;

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

    public void AdicionarItem(Item item)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        Item novoItem = ScriptableObject.Instantiate(item);
        novoItem.name = item.name;

        mudarIdiomaItensDoInventario.TrocarIdioma(novoItem);

        itens.Add(novoItem);
    }

    public void RemoverItem(Item item)
    {
        itens.Remove(item);
        Destroy(item);
    }

    public int ProcurarQuantidadeItem(Item _item)
    {
        int i = 0;
        foreach (var x in itens)
        {
            if (x == _item)
            {
                i++;
            }
        }
        return i;
    }

    private void TrocarIdioma()
    {
        foreach (Item item in itens)
        {
            mudarIdiomaItensDoInventario.TrocarIdioma(item);
        }
    }
}
