using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Item Chave/Chave")]

public class Chave : Item
{
    public override Tipo tipo { get; protected set; } = Tipo.ItemChave;

    [SerializeField] private int id;

    public int ID => id;
}
