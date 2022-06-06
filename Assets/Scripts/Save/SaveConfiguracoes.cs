using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveConfiguracoes
{
    //Pasta dos saves
    private static readonly string pastaDosSaves = Path.Combine(Application.persistentDataPath, "Saves");

    //Classe que contem as variaveis salvas
    [System.Serializable]
    public class Configuracoes
    {
        public int volumeMusica;
        public int volumeEfeitosSonoros;
        public IdiomaManager.Idioma idioma;

        public Configuracoes(int volumeMusica, int volumeEfeitosSonoros, IdiomaManager.Idioma idioma)
        {
            this.volumeMusica = volumeMusica;
            this.volumeEfeitosSonoros = volumeEfeitosSonoros;
            this.idioma = idioma;
        }
    }

    //A instancia da classe Configuracoes
    public static Configuracoes configuracoes = new Configuracoes(50, 60, IdiomaManager.Idioma.Portugues);

    public static void AtualizarConfiguracoes()
    {
        configuracoes.volumeMusica = MusicManager.Volume;
        configuracoes.volumeEfeitosSonoros = SoundManager.Volume;
        configuracoes.idioma = IdiomaManager.GetIdiomaEnum;
    }

    public static void SalvarConfiguracoes()
    {
        //Cria a pasta de saves, se ela nao existir
        Save.IniciarPasta();

        string caminhoDoArquivo = Path.Combine(pastaDosSaves, "configuracoes.txt");

        string texto = JsonUtility.ToJson(configuracoes);

        if (texto != null)
        {
            File.WriteAllText(caminhoDoArquivo, texto);
            return;
        }

        Debug.LogWarning("Nao foi possivel salvar o arquivo!\nCaminho: " + caminhoDoArquivo);
    }

    public static void CarregarConfiguracoes()
    {
        //Cria a pasta de saves, se ela nao existir
        Save.IniciarPasta();

        string caminhoDoArquivo = Path.Combine(pastaDosSaves, "configuracoes.txt");

        string texto = "";

        try
        {
            texto = File.ReadAllText(caminhoDoArquivo);
        }
        catch (FileNotFoundException)
        {
            //Se o arquivo nao existir, o jogo criara um
            SalvarConfiguracoes();
            return;
        }


        if (texto.Length > 1)
        {
            configuracoes = JsonUtility.FromJson<Configuracoes>(texto);
            return;
        }

        Debug.LogWarning("O arquivo nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
    }
}
