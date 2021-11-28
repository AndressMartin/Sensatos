using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Tipo { Consumivel, Ferramenta, ItemChave };

    [SerializeField] public Tipo tipo;
    [SerializeField] public string nome;

    virtual public void Usar(Player player)
    {
        //Nada.
    }

    virtual public void ConsumirRecurso()
    {
        //Nada.
    }
}
