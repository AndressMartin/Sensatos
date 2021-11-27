using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chave : Item
{
    [SerializeField] private int id;

    // Start is called before the first frame update
    public void Iniciar()
    {
        tipo = Tipo.ItemChave;
        id = 2;
    }

    public int GetId()
    {
        return id;
    }
}
