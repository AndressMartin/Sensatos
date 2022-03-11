using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventario : MonoBehaviour
{
    //Componentes
    private Player player;

    //Variaveis
    [SerializeField] private Item itemVazio;

    [SerializeField] private Item[] itens = new Item[9];
    private Item[] atalhosDeItens = new Item[4];

    [SerializeField] private List<ArmaDeFogo> armas = new List<ArmaDeFogo>();
    [SerializeField] private List<RoupaDeCamuflagem> roupasDeCamuflagem = new List<RoupaDeCamuflagem>();

    [SerializeField] private ArmaDeFogo[] armaSlot = new ArmaDeFogo[2];
    private int armaAtual;

    private RoupaDeCamuflagem roupaAtual;
    private Item itemAtual;

    private int dinheiro;

    //Getters
    public Item[] Itens => itens;
    public Item[] AtalhosDeItens => atalhosDeItens;
    public List<ArmaDeFogo> Armas => armas;
    public ArmaDeFogo[] ArmaSlot => armaSlot;
    public int ArmaAtual => armaAtual;
    public RoupaDeCamuflagem RoupaAtual => roupaAtual;
    public Item ItemAtual => itemAtual;
    public int Dinheiro => dinheiro;

    void Start()
    {
        //Componentes
        player = GetComponentInParent<Player>();

        //Criar o inventario de itens
        for (int i = 0; i < itens.Length; i++)
        {
            itens[i] = ScriptableObject.Instantiate(itemVazio);
        }

        //Criar a array dos atalhos
        for (int i = 0; i < atalhosDeItens.Length; i++)
        {
            atalhosDeItens[i] = itemVazio;
        }

        armaAtual = 0;
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

        for(int i = 0; i < itens.Length; i++)
        {
            if(itens[i].ID == 0)
            {
                Destroy(itens[i]);
                itens[i] = novoItem;
                return true;
            }
        }

        return false;
    }

    public void RemoverItem(Item item)
    {
        for (int i = 0; i < itens.Length; i++)
        {
            if (itens[i] == item)
            {
                Destroy(itens[i]);
                itens[i] = ScriptableObject.Instantiate(itemVazio);
                return;
            }
        }

        Debug.LogWarning("O item para ser excluido nao foi encontrado!");
    }

    public void MoverItem(int indice1, int indice2)
    {
        Item itemTemp = itens[indice2];

        itens[indice2] = itens[indice1];
        itens[indice1] = itemTemp;
    }

    public void AdicionarAtalho(int indice, Item item)
    {
        atalhosDeItens[indice] = item;
    }

    public void RemoverAtalho(int indice)
    {
        atalhosDeItens[indice] = itemVazio;
    }

    public void MoverAtalho(int indice1, int indice2)
    {
        Item itemTemp = atalhosDeItens[indice2];

        atalhosDeItens[indice2] = atalhosDeItens[indice1];
        atalhosDeItens[indice1] = itemTemp;
    }

    public void AddArma(ArmaDeFogo arma)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        ArmaDeFogo novaArma = ScriptableObject.Instantiate(arma);
        armas.Add(novaArma);

        if(armas.Count <= 2)
        {
            SetarArmasEquipadas();
        }
    }

    public void AddRoupa(RoupaDeCamuflagem roupa)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        RoupaDeCamuflagem novaRoupa = ScriptableObject.Instantiate(roupa);
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

    public void UsarItemAtual()
    {
        itemAtual?.Usar(player);
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

    public void TrocarArmaDoInventario(ArmaDeFogo arma, GameObject objectThatCalled)
    {
        /*
        ArmaDeFogo weaponToBeBenched = objectThatCalled.GetComponent<WeaponFrame>().GetSavedElement() as ArmaDeFogo;
        int temporaryIndex = arma.index; //E.g. temp = 7
        arma.index = weaponToBeBenched.index; //E.g. arma7 = 1
        weaponToBeBenched.index = temporaryIndex; //E.g. arma1 = 7
        if (arma.index == 0) armaSlot1 = arma;
        else if (arma.index == 1) armaSlot2 = arma;
        else Debug.LogError("WARNING: SWITCHING WEAPONS IN INVENTORY WENT WRONG.");
        ReSort();
        */
    }

    public void ReSort()
    {
        /*
        List<ArmaDeFogo> armasTemp = new List<ArmaDeFogo>();
        foreach (var arma in armas)
        {
            //Debug.Log(arma.index);
            if (armasTemp.Count >= arma.index) armasTemp.Insert(arma.index, arma);
            else armasTemp.Add(arma);
        }
        armas.Clear();
        foreach (var arma in armasTemp)
        {
            armas.Add(arma);
        }
        hasSortedWeapons.Invoke();
        */
    }
}
