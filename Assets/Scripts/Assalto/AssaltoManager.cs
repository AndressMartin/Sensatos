using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaltoManager : MonoBehaviour
{
    [SerializeField] private Assalto assaltoAtual;
    public void SetarAssalto(Assalto _assalto)
    {
        assaltoAtual = _assalto;
    }
}
