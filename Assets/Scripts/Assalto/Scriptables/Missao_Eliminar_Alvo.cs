using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missao/Missao_Eliminar_Avlo")]

public class Missao_Eliminar_Alvo : Missao
{
    [SerializeField] private List<EntityModel> alvos = new List<EntityModel>();

}
