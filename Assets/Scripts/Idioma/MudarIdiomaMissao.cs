using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MudarIdiomaMissao : MonoBehaviour
{
    //Classe que contem o texto da missao
    [System.Serializable]
    public class MissaoTexto
    {
        public string nome;
        public string descricao;
    }

    //A instancia da classe Missao
    public MissaoTexto missaoTexto = new MissaoTexto();

    public void AtualizarIdioma(Missao missao)
    {
        string caminhoDoArquivo = Path.Combine("Textos", "Missoes", IdiomaManager.GetIdioma, missao.name + IdiomaManager.GetIdioma);

        TextAsset texto = (TextAsset)Resources.Load(caminhoDoArquivo);

        if (texto != null)
        {
            missaoTexto = JsonUtility.FromJson<MissaoTexto>(texto.text);

            missao.TrocarIdioma(missaoTexto);
            
            return;
        }

        Debug.LogWarning("O arquivo de texto nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
    }
}
