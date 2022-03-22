using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Item Chave/Item de Missao")]

public class ItemDeMissao : Item
{
    public override TipoItem Tipo => TipoItem.ItemChave;
}
