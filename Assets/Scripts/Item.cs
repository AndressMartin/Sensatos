using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public virtual string nome { get; protected set; }


    public virtual void Usar(GameObject objQueChamou)
    {

    }

    public virtual void ConsumirRecurso()
    {

    }

}
