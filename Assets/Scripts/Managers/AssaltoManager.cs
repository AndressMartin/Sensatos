using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaltoManager : MonoBehaviour
{
    private GeneralManagerScript generalManagerScript;

    [SerializeField] private Assalto assaltoAtual;

    private void Start()
    {
        generalManagerScript = transform.parent.GetComponentInChildren<GeneralManagerScript>();
    }
    public void SetarAssalto(Assalto _assalto)
    {
        assaltoAtual = _assalto;
        generalManagerScript.NpcManager.PassarAssalto(_assalto);
        VerificarAssaltoMissao.SetarAssalto(assaltoAtual);
    }
}
