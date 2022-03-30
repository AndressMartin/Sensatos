using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    //Classe que contem as informacoes do Player
    [System.Serializable]
    public class SaveFile
    {
        public InformacoesSave informacoesSave = new InformacoesSave();

        public int vidaMaxima;
        public InventarioSave inventarioSave = new InventarioSave();

        public bool[] flags;
    }

    //Classe que contem as coisas do inventario
    [System.Serializable]
    public class InventarioSave
    {
        public int dinheiro;
        public List<ArmaDeFogoSave> armas = new List<ArmaDeFogoSave>();
        public int[] armaSlot = new int[2];

        public ItemSave[] itens = new ItemSave[9];
        public int[] atalhosDeItens = new int[4];

        public List<ItemChaveSave> itensChave = new List<ItemChaveSave>();

        public List<int> roupasDeCamuflagem = new List<int>();
        public int roupaAtual;

        public void AtualizarInventarioSave(Inventario inventario, InventarioMissao inventarioMissao)
        {
            dinheiro = inventario.Dinheiro;

            //Limpar a lista de armas
            armas.Clear();

            //Cria a lista de armas para o save
            foreach(ArmaDeFogo arma in inventario.Armas)
            {
                armas.Add(new ArmaDeFogoSave(arma));
            }

            //Limpar os save slots
            armaSlot[0] = 0;
            armaSlot[1] = 0;

            //Setar as armas equipadas como indices da lista de armas
            for (int i = 0; i < inventario.Armas.Count; i++)
            {
                if(inventario.Armas[i] == inventario.ArmaSlot[0])
                {
                    armaSlot[0] = i;
                }

                if (inventario.Armas[i] == inventario.ArmaSlot[1])
                {
                    armaSlot[1] = i;
                }
            }

            //Criar a lista de itens para o save
            for(int i = 0; i < inventario.Itens.Length; i++)
            {
                itens[i] = new ItemSave(inventario.Itens[i]);
            }

            //Resetar os atalhos
            for (int i = 0; i < atalhosDeItens.Length; i++)
            {
                atalhosDeItens[i] = -1;
            }

            //Criar os atalhos como indices da lista de itens
            for (int i = 0; i < inventario.AtalhosDeItens.Length; i++)
            {
                if(inventario.AtalhosDeItens[i].ID != Listas.instance.ListaDeItens.GetID["ItemVazio"])
                {
                    for (int y = 0; y < inventario.Itens.Length; y++)
                    {
                        if(inventario.AtalhosDeItens[i] == inventario.Itens[y])
                        {
                            atalhosDeItens[i] = y;
                            break;
                        }
                    }
                }
            }

            //Limpar a lista de itens chave
            itensChave.Clear();

            //Cria a lista de itens chave para o save
            foreach (ItemChave item in inventarioMissao.Itens)
            {
                itensChave.Add(new ItemChaveSave(item));
            }

            //Limpar a lista de roupas
            roupasDeCamuflagem.Clear();

            //Cria a roupas de armas para o save
            foreach (RoupaDeCamuflagem roupa in inventario.RoupasDeCamuflagem)
            {
                roupasDeCamuflagem.Add(roupa.ID);
            }

            //Resetar a roupa atual
            roupaAtual = 0;

            //Setar a roupa como um indice da lista de roupas
            for (int i = 0; i < inventario.RoupasDeCamuflagem.Count; i++)
            {
                if(inventario.RoupasDeCamuflagem[i] == inventario.RoupaAtual)
                {
                    roupaAtual = i;
                    break;
                }
            }
        }
    }

    //Classe que contem as informacoes das armas
    [System.Serializable]
    public class ArmaDeFogoSave
    {
        public int id;
        public int nivelMelhoria;
        public int municao;
        public int municaoCartucho;

        public ArmaDeFogoSave(ArmaDeFogo arma)
        {
            id = Listas.instance.ListaDeArmas.GetID[arma.name];
            nivelMelhoria = arma.NivelMelhoria;
            municao = arma.Municao;
            municaoCartucho = arma.MunicaoCartucho;
        }
    }

    //Classe que contem as informacoes dos itens
    [System.Serializable]
    public class ItemSave
    {
        public int id;
        public int quantidadeDeUsos;

        public ItemSave(Item item)
        {
            id = Listas.instance.ListaDeItens.GetID[item.name];
            quantidadeDeUsos = item.QuantidadeDeUsos;
        }
    }

    //Classe que contem as informacoes dos itens chave
    [System.Serializable]
    public class ItemChaveSave
    {
        public int id;
        public int quantidade;

        public ItemChaveSave(ItemChave item)
        {
            id = Listas.instance.ListaDeItens.GetID[item.name];
            quantidade = item.Quantidade;
        }
    }

    //Classe que contem as informacoes do save
    [System.Serializable]
    public class InformacoesSave
    {
        public float tempoDeJogo;
        public SerializableDateTime data;

        public InformacoesSave()
        {
            tempoDeJogo = 0;
            data = new SerializableDateTime(DateTime.Now);
        }
    }

    //Classe que contem a data do save
    [System.Serializable]
    public class SerializableDateTime
    {
        public int second;
        public int minute;
        public int hour;

        public int day;
        public int month;
        public int year;

        public SerializableDateTime(DateTime dateTime)
        {
            second = dateTime.Second;
            minute = dateTime.Minute;
            hour = dateTime.Hour;
            day = dateTime.Day;
            month = dateTime.Month;
            year = dateTime.Year;
        }
    }

    //Instancia das classes
    private static SaveFile saveAtual = new SaveFile();
    private static InventarioSave inventarioRespawn = new InventarioSave();

    //Getters
    public static SaveFile SaveAtual => saveAtual;
    public static InventarioSave InventarioRespawn => inventarioRespawn;

    //Setters
    public static void SetSaveAtual(SaveFile save)
    {
        saveAtual = save;
    }

    public static void AtualizarSaveFile(Player player)
    {
        saveAtual.informacoesSave.tempoDeJogo = GameManager.instance.TempoDeJogo;
        saveAtual.informacoesSave.data = new SerializableDateTime(DateTime.Now);

        saveAtual.vidaMaxima = player.VidaMaxima;
        saveAtual.inventarioSave.AtualizarInventarioSave(player.Inventario, player.InventarioMissao);

        saveAtual.flags = Flags.GetFlagList;
    }

    public static void AtualizarInventarioRespawn(Player player)
    {
        inventarioRespawn.AtualizarInventarioSave(player.Inventario, player.InventarioMissao);
    }

    public static void ResetarSaveFile()
    {
        saveAtual = new SaveFile();
    }
}