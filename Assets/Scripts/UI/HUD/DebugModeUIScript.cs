using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugModeUIScript : MonoBehaviour
{
    [SerializeField] private GameObject debugModeUI; //Guarda o objeto da UI do Debug Mode
    [SerializeField] private TMP_Text textoFPS; //Guarda a caixa de texto do mostrador de FPS

    public void DebugModeUIAtiva(bool ativa)
    {
        debugModeUI.SetActive(ativa);
    }

    public void AtualizarTextoFPS(float fps)
    {
        textoFPS.text = fps.ToString();
    }
}
