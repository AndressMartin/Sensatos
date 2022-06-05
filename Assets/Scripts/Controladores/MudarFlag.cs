using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarFlag : MonoBehaviour
{
    [SerializeField] Flags.Flag flag;
    [SerializeField] private bool ativa;

    public void MudarFlagEvento()
    {
        Flags.SetFlag(flag, ativa);
    }
}
