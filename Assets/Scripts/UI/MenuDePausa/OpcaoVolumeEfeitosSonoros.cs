using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpcaoVolumeEfeitosSonoros : Opcao
{
    //Componentes
    [SerializeField] private TMP_Text nomeOpcao;
    [SerializeField] private TMP_Text textoVolume;
    [SerializeField] private Image barraVolume;

    [SerializeField] private Animator animacao;

    //Variaveis
    private int volume;
    private float intervaloInput = 0;
    private float intervaloInputMax = 0.05f;

    private bool apertouOsBotoes = false;

    public override void AtualizarInformacoes(GeneralManagerScript generalManager)
    {
        volume = SoundManager.Volume;
        textoVolume.text = volume.ToString();
        barraVolume.fillAmount = (float)volume / 100;
    }

    public override void NaOpcao(GeneralManagerScript generalManager)
    {
        if(InputManager.EsquerdaSegurar() == false && InputManager.DireitaSegurar() == false)
        {
            apertouOsBotoes = false;
        }

        if (intervaloInput > 0)
        {
            //Deve-se usar o unscaledDeltaTime, pois neste menu o jogo estara pausado, e o timeScale sera 0
            intervaloInput -= Time.unscaledDeltaTime;
            return;
        }

        if (InputManager.EsquerdaSegurar() == true)
        {
            if (volume > 0)
            {
                volume--;
                generalManager.SoundManager.SetVolume(volume);
                AtualizarInformacoes(generalManager);

                intervaloInput = intervaloInputMax;
            }

            if (apertouOsBotoes == false)
            {
                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                apertouOsBotoes = true;
            }
        }

        if (InputManager.DireitaSegurar() == true)
        {
            if (volume < 100)
            {
                volume++;
                generalManager.SoundManager.SetVolume(volume);
                AtualizarInformacoes(generalManager);

                intervaloInput = intervaloInputMax;
            }

            if (apertouOsBotoes == false)
            {
                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                apertouOsBotoes = true;
            }
        }
    }

    public override void Selecionado(bool selecionado)
    {
        intervaloInput = 0;

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
