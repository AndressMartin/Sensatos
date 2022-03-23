using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MudarIdiomaUI : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private TMP_Text textoLockdown;

    private MudarIdiomaInventario mudarIdiomaInventario;

    //Classe que contem os textos
    [System.Serializable]
    public class TextosUI
    {
        //LockdownUI
        public string textoLockdown;

        //Titulos
        public string tituloArmas;
        public string tituloAtalhos;
        public string tituloInventario;
        public string tituloMelhorias;
        public string tituloItensChave;
        public string tituloMissoes;
        public string tituloConquistas;

        //Painel de Escolha dos Itens
        public string textoUsar;
        public string textoAdicionarAosAtalhos;
        public string textoAlterarAPosicao;
        public string textoJogarFora;

        //Painel de Escolha dos Atalhos
        public string textoRemoverDosAtalhos;
        public string textoAlterarAPosicaoAtalho;

        //Painel de Confirmacao para Jogar um Item Fora
        public string textoPerguntaJogarFora;
        public string textoNao;
        public string textoSim;

        //Textos para quando nao ha itens chave
        public string nomeSemItens;
        public string descricaoSemItens;
    }

    //Instancia da classe com os textos
    public TextosUI textosUI = new TextosUI();

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Componentes
        mudarIdiomaInventario = GetComponentInChildren<MudarIdiomaInventario>();

        //Trocar o idioma uma vez para iniciar o objeto com o idioma correto
        TrocarIdioma();
    }

    private void TrocarIdioma()
    {
        string caminhoDoArquivo = "Textos/Geral/" + IdiomaManager.GetIdioma + "/" + "TextosUI" + IdiomaManager.GetIdioma;

        TextAsset texto = (TextAsset)Resources.Load(caminhoDoArquivo);

        if (texto != null)
        {
            textosUI = JsonUtility.FromJson<TextosUI>(texto.text);
            TrocarTextos();
            return;
        }

        Debug.LogWarning("O arquivo de texto nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
    }

    private void TrocarTextos()
    {
        textoLockdown.text = "<u>" + textosUI.textoLockdown + "</u>";

        mudarIdiomaInventario.TrocarIdioma(textosUI, generalManager);
    }
}
