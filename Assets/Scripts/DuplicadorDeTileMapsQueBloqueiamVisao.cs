using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicadorDeTileMapsQueBloqueiamVisao : MonoBehaviour
{
    public void Duplicar()
    {
        GameObject copia = Instantiate(this.gameObject);
        copia.transform.parent = this.transform.parent;
        Destroy(copia.GetComponent<DuplicadorDeTileMapsQueBloqueiamVisao>());

        copia.layer = LayerMask.NameToLayer("ColidirComORaycast");
        copia.GetComponent<CompositeCollider2D>().isTrigger = true;
        copia.name = this.name + " Para Bloquear Visao";
    }
}
