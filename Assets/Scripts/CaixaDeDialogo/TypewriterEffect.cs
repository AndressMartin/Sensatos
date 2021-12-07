using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 50f;

    public Coroutine Run(string textTotype, TMP_Text textLabel)
    {
        return StartCoroutine(TypeText(textTotype, textLabel));
    }

    private IEnumerator TypeText(string textTotype, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;

        float t = 0; //Guarda o tempo que se passa para escrever o tempo
        int charIndex = 0; //Guarda o numero de caracteres que devem aparecer com base no tempo passado

        while (charIndex < textTotype.Length)
        {
            t += Time.deltaTime * typewriterSpeed; //Adiciona o tempo vezes a velocidade
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textTotype.Length); //Limita a variavel entre 0 e o numero de caracteres do texto atual

            textLabel.text = textTotype.Substring(0, charIndex); //Atualiza o texto

            yield return null;
        }

        textLabel.text = textTotype;
    }
}
