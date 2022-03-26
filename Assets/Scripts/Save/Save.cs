using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Save
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

    public static void Salvar(int slot)
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

    public static void Carregar(int slot)
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

    public static bool SaveExiste(int slot)
    {
        string caminhoDoArquivo = Application.dataPath + "/Saves/save" + slot.ToString() + ".txt";

        if(File.Exists(caminhoDoArquivo))
        {
            return true;
        }

        return false;
    }

    private static void PassarInformacoesDoSave(SaveFile save)
    {
        Flags.PassarInformacoesSave(save);
    }
}
