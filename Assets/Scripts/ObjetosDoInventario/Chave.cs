using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Item Chave/Chave")]

public class Chave : Item
{
    public override TipoItem Tipo => TipoItem.ItemChave;
}
