using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitosDeTela : MonoBehaviour
{
    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform tela;

    private void Start()
    {
        //Componentes
        animacao = GetComponent<Animator>();

        tela.gameObject.SetActive(false);
    }

    public void AnimacaoLockdown()
    {
        tela.gameObject.SetActive(true);

        animacao.Play("Lockdown");
    }

    public void DesativarTela()
    {
        animacao.Play("Vazio");

        tela.gameObject.SetActive(false);
    }
}
