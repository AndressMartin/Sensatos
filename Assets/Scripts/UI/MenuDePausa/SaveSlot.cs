using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSlot : MonoBehaviour
{
    //Componentes
    [SerializeField] private RectTransform informacoesDoSave;
    [SerializeField] private TMP_Text nome;
    [SerializeField] private TMP_Text vida;
    [SerializeField] private TMP_Text dinheiro;
    [SerializeField] private TMP_Text tempoDeJogo;
    [SerializeField] private TMP_Text data;
    [SerializeField] private TMP_Text hora;

    [SerializeField] private Animator animacao;

    //Variaveis
    private bool saveExiste = false;

    //Getters
    public bool SaveExiste => saveExiste;

    public void ZerarInformacoes(string nomeSlotVazio)
    {
        saveExiste = false;

        nome.text = nomeSlotVazio;

        informacoesDoSave.gameObject.SetActive(false);
        vida.text = "";
        dinheiro.text = "";
        tempoDeJogo.text = "";
        data.text = "";
        hora.text = "";
    }

    public void AtualizarInformacoes(SaveData.SaveFile save, string nomeSlot)
    {
        saveExiste = true;

        nome.text = nomeSlot;

        informacoesDoSave.gameObject.SetActive(true);

        vida.text = save.vidaMaxima.ToString();

        dinheiro.text = save.inventarioSave.dinheiro.ToString();

        tempoDeJogo.text = TimeSpan.FromSeconds(save.informacoesSave.tempoDeJogo).ToString(@"hh\:mm");

        DateTime dateTime2 = new DateTime(save.informacoesSave.data.year, save.informacoesSave.data.month, save.informacoesSave.data.day, save.informacoesSave.data.hour, save.informacoesSave.data.minute, save.informacoesSave.data.second);

        if(IdiomaManager.GetIdiomaEnum == IdiomaManager.Idioma.Ingles)
        {
            data.text = dateTime2.ToString("MM/dd/yyyy");
        }
        else
        {
            data.text = dateTime2.ToString("dd/MM/yyyy");
        }

        hora.text = dateTime2.ToString("HH:mm:ss");
    }

    public void Selecionado(bool selecionado)
    {
        if (selecionado == true)
        {
            animacao.SetBool("Selecionado", true);
        }
        else
        {
            animacao.SetBool("Selecionado", false);
        }
    }
}
