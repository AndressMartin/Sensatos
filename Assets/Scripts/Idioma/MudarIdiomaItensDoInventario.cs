using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarIdiomaItensDoInventario : MonoBehaviour
{
    //Classe que contem os textos das melhorias
    [System.Serializable]
    public class MelhoriaTexto
    {
        public string nome;
        public string descricao;
    }

    //Classe que contem os textos de um item, ferramenta e item chave
    [System.Serializable]
    public class TextosItem
    {
        public string nome;
        public string descricao;
    }

    //Classe que contem os textos de uma arma
    [System.Serializable]
    public class TextosArma
    {
        public string nome;
        public string descricao;
        public MelhoriaTexto[] melhorias;
    }

    //Instancias da classes
    public TextosItem textosItem = new TextosItem();
    public TextosArma textosArma = new TextosArma();

    public void TrocarIdioma(ItemDoInventario item)
    {
        string caminhoDoArquivo = "";

        if (item is ArmaDeFogo)
        {
            caminhoDoArquivo = "Textos/Armas/" + IdiomaManager.GetIdioma + "/" + item.name + IdiomaManager.GetIdioma;
        }
        else if(item is RoupaDeCamuflagem)
        {
            caminhoDoArquivo = "Textos/Roupas/" + IdiomaManager.GetIdioma + "/" + item.name + IdiomaManager.GetIdioma;
        }
        else
        {
            Item itemTemp = (Item) item;

            if (itemTemp.ID == Listas.instance.ListaDeItens.GetID["ItemVazio"])
            {
                return;
            }

            switch(itemTemp.Tipo)
            {
                case Item.TipoItem.Consumivel:
                    caminhoDoArquivo = "Textos/Itens/" + IdiomaManager.GetIdioma + "/" + item.name + IdiomaManager.GetIdioma;
                    break;

                case Item.TipoItem.Ferramenta:
                    caminhoDoArquivo = "Textos/Ferramentas/" + IdiomaManager.GetIdioma + "/" + item.name + IdiomaManager.GetIdioma;
                    break;

                case Item.TipoItem.ItemChave:
                    caminhoDoArquivo = "Textos/ItensChave/" + IdiomaManager.GetIdioma + "/" + item.name + IdiomaManager.GetIdioma;
                    break;
            }
        }

        TextAsset texto = (TextAsset)Resources.Load(caminhoDoArquivo);

        if (texto != null)
        {
            if (item is ArmaDeFogo)
            {
                textosArma = JsonUtility.FromJson<TextosArma>(texto.text);

                ArmaDeFogo armaTemp = (ArmaDeFogo) item;
                armaTemp.TrocarIdioma(textosArma);
            }
            else
            {
                textosItem = JsonUtility.FromJson<TextosItem>(texto.text);

                item.TrocarIdioma(textosItem);
            }

            return;
        }

        Debug.LogWarning("O arquivo de texto nao foi encontrado!\nCaminho: " + caminhoDoArquivo);
    }
}
