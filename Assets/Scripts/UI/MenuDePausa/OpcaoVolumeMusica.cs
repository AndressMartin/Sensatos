using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpcaoVolumeMusica : Opcao
{
    //Componentes
    [SerializeField] private TMP_Text nomeOpcao;
    [SerializeField] private TMP_Text textoVolume;
    [SerializeField] private Image barraVolume;

    //Variaveis
    private int volume;
    private float intervaloInput = 0;
    private float intervaloInputMax = 0.05f;

    public override void AtualizarInformacoes(GeneralManagerScript generalManager)
    {
        volume = MusicManager.Volume;
        textoVolume.text = volume.ToString();
        barraVolume.fillAmount = (float)volume / 100;
    }

    public override void NaOpcao(GeneralManagerScript generalManager)
    {
        if(intervaloInput > 0)
        {
            //Deve-se usar o unscaledDeltaTime, pois neste menu o jogo estara pausado, e o timeScale sera 0
            intervaloInput -= Time.unscaledDeltaTime;
            return;
        }

        if(InputManager.EsquerdaSegurar())
        {
            if(volume > 0)
            {
                volume--;
                generalManager.MusicManager.SetVolume(volume);
                AtualizarInformacoes(generalManager);

                intervaloInput = intervaloInputMax;
            }
        }

        if (InputManager.DireitaSegurar())
        {
            if (volume < 100)
            {
                volume++;
                generalManager.MusicManager.SetVolume(volume);
                AtualizarInformacoes(generalManager);

                intervaloInput = intervaloInputMax;
            }
        }
    }

    public override void Selecionado(bool selecionado)
    {
        intervaloInput = 0;

        if (selecionado == true)
        {
            nomeOpcao.color = Color.red;
        }
        else
        {
            nomeOpcao.color = Color.black;
        }
    }
}
