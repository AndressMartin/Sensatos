using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventario : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private MudarIdiomaItensDoInventario mudarIdiomaItensDoInventario;

    //Variaveis
    [SerializeField] private Item itemVazio;

    private static Item[] itens;
    private static Item[] atalhosDeItens;

    private static List<ArmaDeFogo> armas = new List<ArmaDeFogo>();
    private static List<RoupaDeCamuflagem> roupasDeCamuflagem = new List<RoupaDeCamuflagem>();

    private static ArmaDeFogo[] armaSlot = new ArmaDeFogo[2];
    private int armaAtual;

    private static RoupaDeCamuflagem roupaAtual;
    private static Item itemAtual;

    private static int dinheiro = 0;

    //Getters
    public Item[] Itens => itens;
    public Item[] AtalhosDeItens => atalhosDeItens;
    public List<ArmaDeFogo> Armas => armas;
    public List<RoupaDeCamuflagem> RoupasDeCamuflagem => roupasDeCamuflagem;
    public ArmaDeFogo[] ArmaSlot => armaSlot;
    public int ArmaAtual => armaAtual;
    public RoupaDeCamuflagem RoupaAtual => roupaAtual;
    public Item ItemAtual => itemAtual;
    public int Dinheiro => dinheiro;

    //Setters
    public void SetRoupaAtual(RoupaDeCamuflagem roupa)
    {
        roupaAtual = roupa;
    }

    public void SetDinheiro(int novoDinheiro)
    {
        dinheiro = novoDinheiro;
    }

    private void Awake()
    {
        //Criar o inventario de itens
        if (itens == null)
        {
            itens = new Item[9];

            for (int i = 0; i < itens.Length; i++)
            {
                itens[i] = ScriptableObject.Instantiate(itemVazio);
                itens[i].name = itemVazio.name;
            }
        }

        //Criar a array dos atalhos
        if (atalhosDeItens == null)
        {
            atalhosDeItens = new Item[4];

            for (int i = 0; i < atalhosDeItens.Length; i++)
            {
                atalhosDeItens[i] = itemVazio;
            }
        }
    }

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Componentes
        mudarIdiomaItensDoInventario = GetComponent<MudarIdiomaItensDoInventario>();

        armaAtual = 0;

        //Trocar o idioma uma vez para iniciar os objetos com o idioma correto
        TrocarIdioma();
    }

    public void Respawn()
    {
        CarregarSave(SaveData.InventarioRespawn);
    }

    public void CarregarInventarioAntesDoAssalto()
    {
        CarregarSave(SaveData.InventarioAntesDoAssalto);
    }

    private void SetarArmasEquipadas()
    {
        if(armas.Count >= 1)
        {
            armaSlot[0] = armas[0];
        }

        if (armas.Count >= 2)
        {
            armaSlot[1] = armas[1];
        }
    }

    public bool AdicionarItem(Item item)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        Item novoItem = ScriptableObject.Instantiate(item);
        novoItem.name = item.name;

        novoItem.Iniciar();

        mudarIdiomaItensDoInventario.TrocarIdioma(novoItem);

        for (int i = 0; i < itens.Length; i++)
        {
            if(itens[i].ID == Listas.instance.ListaDeItens.GetID["ItemVazio"])
            {
                Destroy(itens[i]);
                itens[i] = novoItem;
                return true;
            }
        }

        return false;
    }

    //Adiciona os itens de um arquivo de save
    private void AdicionarItem(Item item, int indice)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        Item novoItem = ScriptableObject.Instantiate(item);
        novoItem.name = item.name;

        novoItem.Iniciar();

        mudarIdiomaItensDoInventario.TrocarIdioma(novoItem);

        if (itens[indice].ID == Listas.instance.ListaDeItens.GetID["ItemVazio"])
        {
            Destroy(itens[indice]);
        }
        else
        {
            RemoverItem(itens[indice]);
        }

        itens[indice] = novoItem;
    }

    public void RemoverItem(Item item)
    {
        //Procura o item nos atalhos para remove-lo
        for (int i = 0; i < atalhosDeItens.Length; i++)
        {
            if (atalhosDeItens[i] == item)
            {
                atalhosDeItens[i] = itemVazio;
                break;
            }
        }

        //Procura o item no inventario para destrui-lo
        for (int i = 0; i < itens.Length; i++)
        {
            if (itens[i] == item)
            {
                Destroy(itens[i]);
                itens[i] = ScriptableObject.Instantiate(itemVazio);
                itens[i].name = itemVazio.name;
                return;
            }
        }

        Debug.LogWarning("O item para ser excluido nao foi encontrado!");
    }

    public void MoverItem(int indiceOrigem, int indiceDestino)
    {
        if(indiceOrigem == indiceDestino)
        {
            return;
        }

        Item itemTemp = itens[indiceDestino];

        itens[indiceDestino] = itens[indiceOrigem];
        itens[indiceOrigem] = itemTemp;
    }

    public void AdicionarAtalho(int indice, Item item)
    {
        //Confere se o item ja nao esta em algum atalho, se estiver, troca a posicao dele com a do atalho selecionado
        for (int i = 0; i < atalhosDeItens.Length; i++)
        {
            if (atalhosDeItens[i] == item)
            {
                MoverAtalho(i, indice);
                return;
            }
        }

        atalhosDeItens[indice] = item;
    }

    public void RemoverAtalho(int indice)
    {
        atalhosDeItens[indice] = itemVazio;
    }

    public void MoverAtalho(int indiceOrigem, int indiceDestino)
    {
        if (indiceOrigem == indiceDestino)
        {
            return;
        }

        Item itemTemp = atalhosDeItens[indiceDestino];

        atalhosDeItens[indiceDestino] = atalhosDeItens[indiceOrigem];
        atalhosDeItens[indiceOrigem] = itemTemp;
    }

    public void AdicionarArma(ArmaDeFogo arma)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        ArmaDeFogo novaArma = ScriptableObject.Instantiate(arma);
        novaArma.name = arma.name;

        novaArma.AdicionarMunicao(novaArma.GetStatus.MunicaoMax + novaArma.GetStatus.MunicaoMaxCartucho);

        mudarIdiomaItensDoInventario.TrocarIdioma(novaArma);

        armas.Add(novaArma);

        if(armas.Count <= 2)
        {
            SetarArmasEquipadas();
        }
    }

    public void AdicionarRoupa(RoupaDeCamuflagem roupa)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        RoupaDeCamuflagem novaRoupa = ScriptableObject.Instantiate(roupa);
        novaRoupa.name = roupa.name;

        mudarIdiomaItensDoInventario.TrocarIdioma(novaRoupa);

        roupasDeCamuflagem.Add(novaRoupa);

        if (roupasDeCamuflagem.Count <= 1)
        {
            roupaAtual = roupasDeCamuflagem[0];
        }
    }

    public void SetarItemAtual(Item item)
    {
        itemAtual = item;
    }

    public void TrocarArma()
    {
        if(armas.Count < 2)
        {
            return;
        }

        if(armaAtual == 0)
        {
            armaAtual = 1;
        }
        else
        {
            armaAtual = 0;
        }
    }

    private void TrocarIdioma()
    {
        foreach (ArmaDeFogo arma in armas)
        {
            mudarIdiomaItensDoInventario.TrocarIdioma(arma);
        }

        foreach (Item item in itens)
        {
            mudarIdiomaItensDoInventario.TrocarIdioma(item);
        }

        foreach (RoupaDeCamuflagem roupa in roupasDeCamuflagem)
        {
            mudarIdiomaItensDoInventario.TrocarIdioma(roupa);
        }
    }

    public void CarregarSave(SaveData.InventarioSave inventarioSave)
    {
        dinheiro = inventarioSave.dinheiro;

        //Lista de armas
        armas.Clear();

        foreach(SaveData.ArmaDeFogoSave arma in inventarioSave.armas)
        {
            AdicionarArma(Listas.instance.ListaDeArmas.GetArma[arma.id]);

            armas[armas.Count - 1].SetNivelMelhoria(arma.nivelMelhoria);
            armas[armas.Count - 1].SetMunicoes(arma.municao, arma.municaoCartucho);
        }

        //Armas equipadas
        armaSlot[0] = null;
        armaSlot[1] = null;

        if(armas.Count > 0)
        {
            armaSlot[0] = armas[inventarioSave.armaSlot[0]];
            armaSlot[1] = armas[inventarioSave.armaSlot[1]];
        }

        //Lista de itens
        for(int i = 0; i < inventarioSave.itens.Length; i++)
        {
            AdicionarItem(Listas.instance.ListaDeItens.GetItem[inventarioSave.itens[i].id], i);

            if(itens[i].ID != Listas.instance.ListaDeItens.GetID["ItemVazio"])
            {
                itens[i].SetQuantidadeDeUsos(inventarioSave.itens[i].quantidadeDeUsos);
            }
        }

        //Atalhos
        for (int i = 0; i < inventarioSave.atalhosDeItens.Length; i++)
        {
            if(inventarioSave.atalhosDeItens[i] < 0)
            {
                RemoverAtalho(i);
            }
            else
            {
                AdicionarAtalho(i, itens[inventarioSave.atalhosDeItens[i]]);
            }
        }

        //Lista de roupas
        roupasDeCamuflagem.Clear();

        foreach (int roupa in inventarioSave.roupasDeCamuflagem)
        {
            AdicionarRoupa(Listas.instance.ListaDeRoupas.GetRoupa[roupa]);
        }

        //Roupa atual
        roupaAtual = roupasDeCamuflagem[inventarioSave.roupaAtual];
    }

    public void ResetarInventario()
    {
        dinheiro = 0;

        //Lista de armas
        armas.Clear();

        //Armas equipadas
        armaSlot[0] = null;
        armaSlot[1] = null;

        //Lista de itens
        for (int i = 0; i < itens.Length; i++)
        {
            if (itens[i].ID != Listas.instance.ListaDeItens.GetID["ItemVazio"])
            {
                RemoverItem(itens[i]);
            }
        }

        //Atalhos
        for (int i = 0; i < atalhosDeItens.Length; i++)
        {
            RemoverAtalho(i);
        }

        //Lista de roupas
        roupasDeCamuflagem.Clear();
    }
}
