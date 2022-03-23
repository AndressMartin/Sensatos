using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DialogueJSONReader : MonoBehaviour
{
    //Classe que contem o texto do dialogo
    [System.Serializable]
    public class Dialogo
    {
        public string texto;
    }

    //Classe que contem as listas com os textos dos dialogos
    [System.Serializable]
    public class DataDeDialogo
    {
        public Dialogo[] dialogos;
        public Dialogo[] respostas;
    }

    //A instancia da classe DataDeDialogo
    public DataDeDialogo dataDeDialogo = new DataDeDialogo();

    public bool CarregarDialogo(DialogueObject dialogueObject)
    {
        string caminhoDoArquivo = "Dialogos/" + IdiomaManager.GetIdioma + "/" + dialogueObject.name + IdiomaManager.GetIdioma;

        TextAsset texto = (TextAsset) Resources.Load(caminhoDoArquivo);

        if(texto != null)
        {
            dataDeDialogo = JsonUtility.FromJson<DataDeDialogo>(texto.text);
            return true;
        }

        Debug.LogWarning("O arquivo de texto nao foi encontrado!");
        return false;
    }
}
