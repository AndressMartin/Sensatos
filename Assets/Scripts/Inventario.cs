using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventario : MonoBehaviour
{
    //Componentes
    private Player player;

    //Variaveis
    [SerializeField] private List<Item> itens = new List<Item>();
    private List<Item> itensEmAtalhos = new List<Item>();
    [SerializeField] private List<ArmaDeFogo> armas = new List<ArmaDeFogo>();

    [SerializeField] private ArmaDeFogo armaSlot1;
    [SerializeField] private ArmaDeFogo armaSlot2;
    private Item itemAtual;

    [HideInInspector] public UnityEvent hasSortedWeapons;

    //Getters
    public List<Item> Itens => itens;
    public List<Item> ItensEmAtalhos => itensEmAtalhos;
    public List<ArmaDeFogo> Armas => armas;
    public ArmaDeFogo ArmaSlot1 => armaSlot1;
    public ArmaDeFogo ArmaSlot2 => armaSlot2;
    public Item ItemAtual => itemAtual;

    void Start()
    {
        //Componentes
        player = GetComponentInParent<Player>();

        if (hasSortedWeapons == null)
            hasSortedWeapons = new UnityEvent();
    }

    private void SetarArmasEquipadas()
    {
        if(armas.Count >= 1)
        {
            armaSlot1 = armas[0];
            armas[0].index = 0;
        }

        if (armas.Count >= 2)
        {
            armaSlot2 = armas[1];
            armas[1].index = 1;
        }
    }

    public void AddItem(Item item)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        Item novoItem = ScriptableObject.Instantiate(item);
        itens.Add(novoItem);
    }

    public void RemoveItem(Item item)
    {
        itens.Remove(item);
        Destroy(item);
    }

    public void AddArma(ArmaDeFogo arma)
    {
        //Cria uma nova instancia do scriptable object e a adiciona no inventario
        ArmaDeFogo novaArma = ScriptableObject.Instantiate(arma);
        armas.Add(novaArma);
        novaArma.index = armas.Count - 1;

        if(armas.Count <= 2)
        {
            SetarArmasEquipadas();
        }
        //Needs resorting indexes?
    }

    public void SetarItemAtual(Item item)
    {
        itemAtual = item;
    }

    public void UsarItemAtual()
    {
        itemAtual?.Usar(player);
    }

    public void EquiparArma(ArmaDeFogo arma)
    {
        armaSlot1 = arma;
        //Needs resorting indexes?
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
        ArmaDeFogo weaponToBeBenched = objectThatCalled.GetComponent<WeaponFrame>().GetSavedElement() as ArmaDeFogo;
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
        hasSortedWeapons.Invoke();
    }
}
