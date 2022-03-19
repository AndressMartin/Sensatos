using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missao")]
public class Missao : ScriptableObject
{
    [SerializeField] private string nomeMissao;
    [SerializeField] private bool concluida;
    public string GetNomeMissao => nomeMissao;
    public bool GetConcluida => concluida;
}
