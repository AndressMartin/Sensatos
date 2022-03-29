using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MudarIdiomaMenuDeOpcoes : MonoBehaviour
{
    //Titulos
    [SerializeField] private TMP_Text tituloOpcoes;

    //Textos das opcoes do Menu de Opcoes
    [SerializeField] private TMP_Text textoVolumeMusica;
    [SerializeField] private TMP_Text textoVolumeEfeitosSonoros;
    [SerializeField] private TMP_Text textoIdioma;

    public void TrocarIdioma(MudarIdiomaUI.TextosUI textosUI, GeneralManagerScript generalManager)
    {
        this.tituloOpcoes.text = textosUI.tituloOpcoes;

        this.textoVolumeMusica.text = textosUI.textoVolumeMusica;
        this.textoVolumeEfeitosSonoros.text = textosUI.textoVolumeEfeitosSonoros;
        this.textoIdioma.text = textosUI.textoIdioma;
    }
}
