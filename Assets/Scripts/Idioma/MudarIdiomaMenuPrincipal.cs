using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MudarIdiomaMenuPrincipal : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    private MenuPrincipal menuPrincipal;

    //Titulos
    [SerializeField] private TMP_Text tituloNovoJogo;
    [SerializeField] private TMP_Text tituloCarregarJogo;

    //Painel de Escolha do Menu Inicial
    [SerializeField] private TMP_Text textoNovoJogo;
    [SerializeField] private TMP_Text textoCarregarJogo;
    [SerializeField] private TMP_Text textoOpcoes;
    [SerializeField] private TMP_Text textoSairDoJogo;

    //Menu de Novo Jogo
    [SerializeField] private TMP_Text textoContinuarSemSalvar;
    [SerializeField] private TMP_Text perguntaSobrescreverOSave;
    [SerializeField] private TMP_Text textoNaoSobrescreverSave;
    [SerializeField] private TMP_Text textoSimSobrescreverSave;
    [SerializeField] private TMP_Text perguntaContinuarSemSalvar;
    [SerializeField] private TMP_Text textoNaoContinuarSemSalvar;
    [SerializeField] private TMP_Text textoSimContinuarSemSalvar;

    //Menu de Carregar Jogo
    [SerializeField] private TMP_Text perguntaCarregarOSave;
    [SerializeField] private TMP_Text textoNaoCarregarSave;
    [SerializeField] private TMP_Text textoSimCarregarSave;
    [SerializeField] private TMP_Text textoFalhaCarregarJogo;
    [SerializeField] private TMP_Text textoContinuarFalhaCarregarJogo;

    //Painel de COnfirmacao para Sair do Jogo
    [SerializeField] private TMP_Text perguntaSairDoJogo;
    [SerializeField] private TMP_Text textoNaoSairDoJogo;
    [SerializeField] private TMP_Text textoSimSairDoJogo;

    private MudarIdiomaMenuDeOpcoes mudarIdiomaMenuDeOpcoes;

    //Classe que contem os textos
    [System.Serializable]
    public class TextosMenuPrincipal
    {
        //Titulos
        public string tituloNovoJogo;
        public string tituloCarregarJogo;
        public string tituloOpcoes;

        //Painel de Escolha do Menu Inicial
        public string textoNovoJogo;
        public string textoCarregarJogo;
        public string textoOpcoes;
        public string textoSairDoJogo;

        //Menu de Novo Jogo
        public string textoContinuarSemSalvar;
        public string perguntaSobrescreverOSave;
        public string textoNaoSobrescreverSave;
        public string textoSimSobrescreverSave;
        public string perguntaContinuarSemSalvar;
        public string textoNaoContinuarSemSalvar;
        public string textoSimContinuarSemSalvar;

        //Menu de Carregar Jogo
        public string perguntaCarregarOSave;
        public string textoNaoCarregarSave;
        public string textoSimCarregarSave;
        public string textoFalhaCarregarJogo;
        public string textoContinuarFalhaCarregarJogo;

        //Painel de Confirmacao para Sair do Jogo
        public string perguntaSairDoJogo;
        public string textoNaoSairDoJogo;
        public string textoSimSairDoJogo;

        //Saves
        public string textoNomeSlot;
        public string textoNomeSlotVazio;

        //Textos das opcoes do Menu de Opcoes
        public string textoVolumeMusica;
        public string textoVolumeEfeitosSonoros;
        public string textoIdioma;
    }

    //Instancia da classe com os textos
    public TextosMenuPrincipal textosMenuPrincipal = new TextosMenuPrincipal();

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        menuPrincipal = FindObjectOfType<MenuPrincipal>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Componentes
        mudarIdiomaMenuDeOpcoes = GetComponentInChildren<MudarIdiomaMenuDeOpcoes>(true);

        //Trocar o idioma uma vez para iniciar o objeto com o idioma correto
        TrocarIdioma();
    }

    private void TrocarIdioma()
    {
        string caminhoDoArquivo = "Textos/Geral/" + IdiomaManager.GetIdioma + "/" + "TextosMenuPrincipal" + IdiomaManager.GetIdioma;

        TextAsset texto = (TextAsset)Resources.Load(caminhoDoArquivo);

        if (texto != null)
        {
            textosMenuPrincipal = JsonUtility.FromJson<TextosMenuPrincipal>(texto.text);
            TrocarTextos();
            return;
        }

        Debug.LogWarning("O arquivo de texto nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
    }

    private void TrocarTextos()
    {
        this.tituloNovoJogo.text = textosMenuPrincipal.tituloNovoJogo;
        this.tituloCarregarJogo.text = textosMenuPrincipal.tituloCarregarJogo;

        this.textoNovoJogo.text = textosMenuPrincipal.textoNovoJogo;
        this.textoCarregarJogo.text = textosMenuPrincipal.textoCarregarJogo ;
        this.textoOpcoes.text = textosMenuPrincipal.textoOpcoes;
        this.textoSairDoJogo.text = textosMenuPrincipal.textoSairDoJogo;

        this.textoContinuarSemSalvar.text = textosMenuPrincipal.textoContinuarSemSalvar;
        this.perguntaSobrescreverOSave.text = textosMenuPrincipal.perguntaSobrescreverOSave;
        this.textoNaoSobrescreverSave.text = textosMenuPrincipal.textoNaoSobrescreverSave;
        this.textoSimSobrescreverSave.text = textosMenuPrincipal.textoSimSobrescreverSave;
        this.perguntaContinuarSemSalvar.text = textosMenuPrincipal.perguntaContinuarSemSalvar;
        this.textoNaoContinuarSemSalvar.text = textosMenuPrincipal.textoNaoContinuarSemSalvar;
        this.textoSimContinuarSemSalvar.text = textosMenuPrincipal.textoSimContinuarSemSalvar;

        this.perguntaCarregarOSave.text = textosMenuPrincipal.perguntaCarregarOSave;
        this.textoNaoCarregarSave.text = textosMenuPrincipal.textoNaoCarregarSave;
        this.textoSimCarregarSave.text = textosMenuPrincipal.textoSimCarregarSave;
        this.textoFalhaCarregarJogo.text = textosMenuPrincipal.textoFalhaCarregarJogo;
        this.textoContinuarFalhaCarregarJogo.text = textosMenuPrincipal.textoContinuarFalhaCarregarJogo;

        this.perguntaSairDoJogo.text = textosMenuPrincipal.perguntaSairDoJogo;
        this.textoNaoSairDoJogo.text = textosMenuPrincipal.textoNaoSairDoJogo;
        this.textoSimSairDoJogo.text = textosMenuPrincipal.textoSimSairDoJogo;

        menuPrincipal.GetMenuNovoJogo.SetNomeSlot(textosMenuPrincipal.textoNomeSlot);
        menuPrincipal.GetMenuNovoJogo.SetNomeSlotVazio(textosMenuPrincipal.textoNomeSlotVazio);

        menuPrincipal.GetMenuCarregarJogo.SetNomeSlot(textosMenuPrincipal.textoNomeSlot);
        menuPrincipal.GetMenuCarregarJogo.SetNomeSlotVazio(textosMenuPrincipal.textoNomeSlotVazio);

        mudarIdiomaMenuDeOpcoes.TrocarIdioma(textosMenuPrincipal, generalManager);
    }
}
