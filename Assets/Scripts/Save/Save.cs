using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Save
{
    //Pasta dos saves
    private static readonly string pastaDosSaves = Path.Combine(Application.dataPath, "Saves");

    //Save Slot atual
    private static int saveAtual = 0;

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

    public static bool Salvar(int slot)
    {
        //Classe de save
        SaveData.SaveFile save = SaveData.SaveAtual;

        string caminhoDoArquivo = Path.Combine(Application.dataPath, "Saves", "save" + slot.ToString() + ".txt");

        string texto = JsonUtility.ToJson(save, true);

        if (texto != null)
        {
            File.WriteAllText(caminhoDoArquivo, texto);

            //Se o save for bem sucedido, altera  o save atual para o que acabou de ser salvo
            saveAtual = slot;

            return true;
        }

        Debug.LogWarning("Nao foi possivel salvar o arquivo!\nCaminho: " + caminhoDoArquivo);
        return false;
    }

    public static bool Carregar(int slot)
    {
        //Classe de save
        SaveData.SaveFile save = SaveData.SaveAtual;

        string caminhoDoArquivo = Path.Combine(Application.dataPath, "Saves", "save" + slot.ToString() + ".txt");

        string texto;

        try
        {
            texto = File.ReadAllText(caminhoDoArquivo);
        }
        catch(FileNotFoundException)
        {
            texto = null;
        }

        if (texto != null)
        {
            save = JsonUtility.FromJson<SaveData.SaveFile>(texto);

            SaveData.SetSaveAtual(save);

            PassarInformacoesDoSave(SaveData.SaveAtual);

            return true;
        }

        Debug.LogWarning("O arquivo nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
        return false;
    }

    public static SaveData.SaveFile CarregarInformacoes(int slot)
    {
        //Classe de save
        SaveData.SaveFile save = SaveData.SaveAtual;

        string caminhoDoArquivo = Path.Combine(Application.dataPath, "Saves", "save" + slot.ToString() + ".txt");

        string texto;

        texto = File.ReadAllText(caminhoDoArquivo);


        if (texto != null)
        {
            save = JsonUtility.FromJson<SaveData.SaveFile>(texto);

            return save;
        }

        Debug.LogWarning("O arquivo nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
        return null;
    }

    private static void PassarInformacoesDoSave(SaveData.SaveFile save)
    {
        GameManager.instance.SetTempoDeJogo(save.informacoesSave.tempoDeJogo);

        GameManager.instance.SetCapituloAtual(save.capituloAtual);
        Missoes.PassarInformacoesSave(save);
        Flags.PassarInformacoesSave(save);
    }
}
