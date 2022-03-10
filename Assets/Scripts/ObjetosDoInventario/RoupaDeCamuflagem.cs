using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Roupa de Camuflagem")]

public class RoupaDeCamuflagem : ItemDoInventario
{
    [SerializeField] private float fatorDePercepcao;

    public float FatorDePercepcao => fatorDePercepcao;
}
