using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventario : MonoBehaviour
{
    private Player player;
    public List<Item> itens = new List<Item>();
    public List<ArmaDeFogo> armas = new List<ArmaDeFogo>();
    public ArmaDeFogo armaSlot1;
    public ArmaDeFogo armaSlot2;
    public Item itemAtual;
    public List<Item> itemsEmAtalhos = new List<Item>();

    public UnityEvent sortedWeapons;
    // Start is called before the first frame update
    void Start()
    {
        if (sortedWeapons == null)
            sortedWeapons = new UnityEvent();
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
        //Needs resorting indexes?
    }

    public void SetarItemAtual(Item item)
    {
        itemAtual = item;
    }

    public void EquiparArma(ArmaDeFogo arma)
    {
        armaSlot1 = arma;
        //Needs resorting indexes?
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

    public void TrocarArmaDoInventario(ArmaDeFogo arma, GameObject objectThatCalled)
    {
        ArmaDeFogo weaponToBeBenched = objectThatCalled.GetComponent<WeaponFrame>().GetSavedWeapon();
        int temporaryIndex = arma.index; //E.g. temp = 7
        arma.index = weaponToBeBenched.index; //E.g. arma7 = 1
        weaponToBeBenched.index = temporaryIndex; //E.g. arma1 = 7
        if (arma.index == 0) armaSlot1 = arma;
        else if (arma.index == 1) armaSlot2 = arma;
        else Debug.LogError("WARNING: SWITCHING WEAPONS IN INVENTORY WENT WRONG.");
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
        sortedWeapons.Invoke();
    }
}
