using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MudarIdiomaMenuDePausa : MonoBehaviour
{
    //Componentes
    //Titulos
    [SerializeField] private TMP_Text tituloPausa;
    [SerializeField] private TMP_Text tituloSalvar;
    [SerializeField] private TMP_Text tituloOpcoes;

    //Painel de Escolha do Menu Inicial
    [SerializeField] private TMP_Text textoContinuar;
    [SerializeField] private TMP_Text textoSalvar;
    [SerializeField] private TMP_Text textoOpcoes;
    [SerializeField] private TMP_Text textoMenuPrincipal;

    //Texto de Confirmacao para Voltar ao Menu Principal
    [SerializeField] private TMP_Text textoPerguntaSairParaOMenuPrincipal;
    [SerializeField] private TMP_Text textoNaoMenuPrincipal;
    [SerializeField] private TMP_Text textoSimMenuPrincipal;

    //Textos das opcoes do Menu de Opcoes
    [SerializeField] private TMP_Text textoVolumeMusica;
    [SerializeField] private TMP_Text textoVolumeEfeitosSonoros;
    [SerializeField] private TMP_Text textoIdioma;

    //Menu de Salvar
    [SerializeField] private TMP_Text textoPerguntaSobrescreverOSave;
    [SerializeField] private TMP_Text textoNaoSobrescreverSave;
    [SerializeField] private TMP_Text textoSimSobrescreverSave;
    [SerializeField] private TMP_Text textoSaveSucesso;
    [SerializeField] private TMP_Text textoContinuarSaveSucesso;

    public void TrocarIdioma(MudarIdiomaUI.TextosUI textosUI, GeneralManagerScript generalManager)
    {
        this.tituloPausa.text = textosUI.tituloPausa;
        this.tituloSalvar.text = textosUI.tituloSalvar;
        this.tituloOpcoes.text = textosUI.tituloOpcoes;

        this.textoContinuar.text = textosUI.textoContinuar;
        this.textoSalvar.text = textosUI.textoSalvar;
        this.textoOpcoes.text = textosUI.textoOpcoes;
        this.textoMenuPrincipal.text = textosUI.textoMenuPrincipal;

        this.textoPerguntaSairParaOMenuPrincipal.text = textosUI.textoPerguntaSairParaOMenuPrincipal;
        this.textoNaoMenuPrincipal.text = textosUI.textoNaoMenuPrincipal;
        this.textoSimMenuPrincipal.text = textosUI.textoSimMenuPrincipal;

        this.textoVolumeMusica.text = textosUI.textoVolumeMusica;
        this.textoVolumeEfeitosSonoros.text = textosUI.textoVolumeEfeitosSonoros;
        this.textoIdioma.text = textosUI.textoIdioma;

        this.textoPerguntaSobrescreverOSave.text = textosUI.textoPerguntaSobrescreverOSave;
        this.textoNaoSobrescreverSave.text = textosUI.textoNaoSobrescreverSave;
        this.textoSimSobrescreverSave.text = textosUI.textoSimSobrescreverSave;
        this.textoSaveSucesso.text = textosUI.textoSaveSucesso;
        this.textoContinuarSaveSucesso.text = textosUI.textoContinuarSaveSucesso;

        generalManager.Hud.MenuDePausa.GetMenuSalvar.SetNomeSlot(textosUI.textoNomeSlot);
        generalManager.Hud.MenuDePausa.GetMenuSalvar.SetNomeSlotVazio(textosUI.textoNomeSlotVazio);
    }
}
