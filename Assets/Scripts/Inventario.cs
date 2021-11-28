using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    private Player player;
    public List<Item> itens = new List<Item>();
    public List<ArmaDeFogo> armas = new List<ArmaDeFogo>();
    public ArmaDeFogo armaSlot1;
    public ArmaDeFogo armaSlot2;
    public Item itemAtual;
    public List<Item> itemsEmAtalhos = new List<Item>();
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
        InitWeaponConfig();
    }

    private void InitWeaponConfig()
    {
        armaSlot1 = armas[0];
        armas[0].index = 0;
        armaSlot2 = armas[1];
        armas[1].index = 1;
    }

    public void Add(Item item)
    {
        itens.Add(item);
    }

    public void Remove(Item item)
    {
        itens.Remove(item);
    }

    public void AddArma(ArmaDeFogo arma)
    {
        armas.Add(arma);
        arma.GetComponent<ArmaDeFogo>().index = armas.Count - 1;
    }

    public void EquiparItem(Item item)
    {
        itemAtual = item;
    }

    public void EquiparArma(ArmaDeFogo arma)
    {
        armaSlot1 = arma;
    }

    public void UsarItemAtual()
    {
        if (itemAtual != null)
        {
            itemAtual.Usar(player);
        }
    }
    public void TrocarArma()
    {
        foreach (var arma in armas)
        {
            if(arma.index == 0)
            {
                armaSlot2 = arma;
                arma.index = 1;
            }
            else if (arma.index == 1)
            {
                armaSlot1 = arma;
                arma.index = 0;
            }
        }
        ReSort();
    }
    public void ReSort()
    {
        List<ArmaDeFogo> armasTemp = new List<ArmaDeFogo>();
        foreach (var arma in armas)
        {
            Debug.Log(arma.index);
            if (armasTemp.Count >= arma.index) armasTemp.Insert(arma.index, arma);
            else armasTemp.Add(arma);
        }
        armas.Clear();
        foreach (var arma in armasTemp)
        {
            armas.Add(arma);
        }
    }
    public void EquiparArmaDaSelecaoInventario()
    {

    }
}
