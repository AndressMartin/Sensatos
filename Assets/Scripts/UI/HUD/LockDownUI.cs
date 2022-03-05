using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockDownUI : MonoBehaviour
{
    [SerializeField] TMP_Text titulo;
    [SerializeField] TMP_Text tempo;

    public void AtualizarTempo(string texto)
    {
        tempo.text = texto;
    }
}
