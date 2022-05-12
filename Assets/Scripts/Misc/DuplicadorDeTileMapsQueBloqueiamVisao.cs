using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DuplicadorDeTileMapsQueBloqueiamVisao : MonoBehaviour
{
    public void Duplicar()
    {
        GameObject copia = Instantiate(this.gameObject);
        copia.transform.parent = this.transform.parent;
        Destroy(copia.GetComponent<DuplicadorDeTileMapsQueBloqueiamVisao>());

        copia.layer = LayerMask.NameToLayer("ColidirComORaycast");
        copia.GetComponent<CompositeCollider2D>().isTrigger = true;
        copia.GetComponent<TilemapRenderer>().enabled = false;
        copia.name = this.name + " Para Bloquear Visao";
    }
}
