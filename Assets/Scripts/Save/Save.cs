using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Save
{
    //Pasta dos saves
    private static readonly string pastaDosSaves = Path.Combine(Application.dataPath, "Saves");

    //Save Slot atual
    private static int saveAtual = 1;

    //Getters
    public static int SaveAtual => saveAtual;

    //Setters
    public static void SetSaveAtual(int novoSaveAtual)
    {
        saveAtual = novoSaveAtual;
    }

    public static void IniciarPasta()
    {
        //Confere se a pasta dos saves existe, se nao existir, cria ela
        if (Directory.Exists(pastaDosSaves) == false)
        {
            Directory.CreateDirectory(pastaDosSaves);
        }
    }

    public static bool SaveExiste(int slot)
    {
        string caminhoDoArquivo = Path.Combine(Application.dataPath, "Saves", "save" + slot.ToString() + ".txt");

        if (File.Exists(caminhoDoArquivo))
        {
            return true;
        }

        return false;
    }

    public static void Salvar(int slot)
    {
        //Classe de save
        SaveData.SaveFile save = SaveData.SaveAtual;

        string caminhoDoArquivo = Path.Combine(Application.dataPath, "Saves", "save" + slot.ToString() + ".txt");

        string texto = JsonUtility.ToJson(save);

        if (texto != null)
        {
            File.WriteAllText(caminhoDoArquivo, texto);

            //Se o save for bem sucedido, altera  o save atual para o que acabou de ser salvo
            saveAtual = slot;

            return;
        }

        Debug.LogWarning("Nao foi possivel salvar o arquivo!\nCaminho: " + caminhoDoArquivo);
    }

    public static void Carregar(int slot)
    {
        //Classe de save
        SaveData.SaveFile save = SaveData.SaveAtual;

        string caminhoDoArquivo = Path.Combine(Application.dataPath, "Saves", "save" + slot.ToString() + ".txt");

        string texto;

        texto = File.ReadAllText(caminhoDoArquivo);


        if (texto != null)
        {
            save = JsonUtility.FromJson<SaveData.SaveFile>(texto);

            SaveData.SetSaveAtual(save);

            PassarInformacoesDoSave(SaveData.SaveAtual);

            return;
        }

        Debug.LogWarning("O arquivo nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
    }

    private static void PassarInformacoesDoSave(SaveData.SaveFile save)
    {
        Flags.PassarInformacoesSave(save);
    }
}
