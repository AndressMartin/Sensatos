using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    //Classe que contem as variaveis salvas
    [System.Serializable]
    public class SaveFile
    {
        public bool[] flags;

        public SaveFile()
        {
            flags = Flags.GetFlagList;
        }
    }

    public void Salvar(int slot)
    {
        //A instancia da classe
        SaveFile save = new SaveFile();

        string caminhoDoArquivo = Application.dataPath + "/Saves/save" + slot.ToString() + ".txt";

        string texto = JsonUtility.ToJson(save);

        if (texto != null)
        {
            File.WriteAllText(caminhoDoArquivo, texto);
            return;
        }

        Debug.LogWarning("Nao foi possivel salvar o arquivo!\nCaminho: " + caminhoDoArquivo);
    }

    public void Carregar(int slot)
    {
        //A instancia da classe
        SaveFile save = new SaveFile();

        string caminhoDoArquivo = Application.dataPath + "/Saves/save" + slot.ToString() + ".txt";

        string texto;

        texto = File.ReadAllText(caminhoDoArquivo);


        if (texto != null)
        {
            save = JsonUtility.FromJson<SaveFile>(texto);

            PassarInformacoesDoSave(save);

            return;
        }

        Debug.LogWarning("O arquivo nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
    }

    private void PassarInformacoesDoSave(SaveFile save)
    {
        Flags.PassarInformacoesSave(save);
    }
}
