using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chave : Item
{
    [SerializeField] private int id;

    public int GetId()
    {
        return id;
    }
}
